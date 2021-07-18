using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Monsajem_Incs.Net.Base.Service;
using Monsajem_Incs.Database;
using System.Linq;
using Monsajem_Incs.Collection.Array.TreeBased;
using System.Linq.Expressions;
using static Monsajem_Incs.Collection.Array.Extentions;

namespace Monsajem_Incs.Database.Base
{
    public static partial class Extentions
    {
        private static async Task<bool> I_GetUpdate<DataType, KeyType>(
            this IAsyncOprations Client,
            Table<DataType, KeyType> Table,
            Action<DataType> MakeingUpdate,
            Action<KeyType> Deleted,
            bool IsPartOfTable)
            where KeyType : IComparable<KeyType>
        {
            static async Task<int> FindLastTrue(
            Table<DataType, KeyType> Table,
            IRemoteUpdateSender<DataType, KeyType> service,
            int StartPos, int EndPos)
            {
                var EndPosParameter = EndPos;
                var Update = Table.UpdateAble;
                while (StartPos < EndPos&&
                       EndPos <= EndPosParameter)
                {
                    if (Update.UpdateCodes[EndPos].UpdateCode !=
                        await service.GetUpdateCodeAtPos(EndPos))
                        EndPos = (EndPos + StartPos) / 2;
                    else
                    {
                        var OldEnd = EndPos;
                        EndPos = (EndPos * 2) - StartPos;
                        StartPos = OldEnd;
                    }
                }
                if (EndPos > EndPosParameter)
                    return EndPosParameter;
                else
                    return EndPos;
            }

            Table<DataType, KeyType> ParentTable = Table;
            PartOfTable<DataType, KeyType> PartTable = null;
            if (Table._UpdateAble == null)
                Table._UpdateAble = new UpdateAbles<KeyType>(0);

            var Result = false;

            await Client.SendData(Table.UpdateAble.UpdateCode);//1
            var LastUpdateCode = await Client.GetData<ulong>();//2
            if (Table.UpdateAble.UpdateCode > LastUpdateCode)
            {
                Table.UpdateAble.UpdateCode = 0;
                Table.UpdateAble.UpdateCodes = new UpdateAble<KeyType>[0];
                Table.UpdateAble.UpdateKeys = new UpdateAble<KeyType>[0];
                if (IsPartOfTable)
                {
                    ParentTable.UpdateAble.UpdateCode = 0;
                    ParentTable.UpdateAble.UpdateCodes = new UpdateAble<KeyType>[0];
                    ParentTable.UpdateAble.UpdateKeys = new UpdateAble<KeyType>[0];
                }
            }
            if (Table.UpdateAble.UpdateCode != LastUpdateCode)
            {
                var ShouldDelete = new Collection.Array.TreeBased.Array<KeyType>();
                var Remote = new IRemoteUpdateSender<DataType, KeyType>();
                var ServerItemsCount = await Client.GetData<int>();
                var StartPos = 0;
                var EndPos = Math.Min(Table.UpdateAble.UpdateCodes.Length, ServerItemsCount) - 1;

                Func<Task> GetNextItems = async () =>
                {
                    for (; StartPos < ServerItemsCount; StartPos++)
                    {
                        var UpCode = await Client.GetData<ulong>();
                        if (UpCode == 0)
                            return;
                        var Value = await Client.GetData<DataType>();
                        var Key = Table.GetKey(Value);
                        var Update = new UpdateAble<KeyType>()
                        { Key = Key, UpdateCode = UpCode };
                        if (ShouldDelete.BinarySearch(Update.Key).Index > -1)
                        {
                            ShouldDelete.BinaryDelete(Update.Key);
                            Table.Update(Update.Key, (c) =>
                            {
                                Table.MoveRelations(c, Value);
                                return Value;
                            });
                            Table.UpdateAble.Changed(Key, Key, Update.UpdateCode);
                        }
                        else
                        {
                            Table.Insert(Value);
                            Table.UpdateAble.Insert(Key, Update.UpdateCode);
                        }
                        if (IsPartOfTable)
                        {
                            var ParentUpCode = await Client.GetData<ulong>();
                            ParentTable.UpdateAble.Changed(Key, Key, Update.UpdateCode);
                        }
                    }
                };


                await Client.Remote(Remote,
                async (Remote) =>
                {
                    while (StartPos <= EndPos)
                    {
                        var LastTruePos = await FindLastTrue(Table, Remote, StartPos, EndPos);

                        if (LastTruePos == ServerItemsCount - 1)
                            break;
                        else
                        {
                            LastTruePos++;
                            var NextTrue = await Remote.GetUpdateCodeAtPos(LastTruePos);
                            UpdateAble<KeyType> MyUpCode = null;
                            while (Table.UpdateAble.UpdateCodes.Length > LastTruePos)
                            {
                                MyUpCode = Table.UpdateAble.UpdateCodes[LastTruePos];
                                if (MyUpCode.UpdateCode < NextTrue)
                                {
                                    Table.UpdateAble.DeleteDontUpdate(MyUpCode.Key);
                                    ShouldDelete.Insert(MyUpCode.Key);
                                }
                                else
                                    break;
                            }
                            StartPos = LastTruePos;
                            if (LastTruePos >= Table.UpdateAble.UpdateCodes.Length)
                                break;
                            MyUpCode = Table.UpdateAble.UpdateCodes[LastTruePos];
                            //if (MyUpCode.UpdateCode>NextTrue)
                            //    await Remote.OutOfRemote(1,// Get From pos to update code
                            //async () =>
                            //{
                            //    await Client.SendData((StartPos, MyUpCode.UpdateCode));
                            //    await GetNextItems();
                            //});
                        }
                    }
                    //await Remote.OutOfRemote(1,// Get From pos to end
                    //async () =>
                    //{
                    //    await Client.SendData(StartPos);
                    //    await GetNextItems();
                    //});


                    foreach (var Delete in ShouldDelete)
                        Table.Delete(Delete);
                });
            }


            Table.UpdateAble.UpdateCode = LastUpdateCode;
            if (IsPartOfTable)
                PartTable.SaveToParent();
            return Result;
        }
    }
}
