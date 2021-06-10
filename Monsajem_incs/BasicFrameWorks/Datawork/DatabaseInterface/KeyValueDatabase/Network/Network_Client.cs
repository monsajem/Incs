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
                var Update = Table.UpdateAble;
                while (StartPos < EndPos)
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
                var Remote = new IRemoteUpdateSender<DataType, KeyType>(Table,IsPartOfTable);
                var ServerItemsCount = await Client.GetData<int>();
                var StartPos = 0;
                var EndPos = Math.Min(Table.UpdateAble.UpdateCodes.Length, ServerItemsCount) - 1;
                while (StartPos <= EndPos)
                {
                    var Break = await Client.Remote(Remote,
                    async (Remote) =>
                    {
                        var LastTruePos = await FindLastTrue(Table, Remote.Obj, StartPos, EndPos);

                        if (LastTruePos == ServerItemsCount - 1)
                            return true;
                        else
                        {
                            LastTruePos++;
                            var NextTrue = await Remote.Obj.GetUpdateCodeAtPos(LastTruePos);
                            UpdateAble<KeyType> MyUpCode = null;
                            while (Table.UpdateAble.UpdateCodes.Length > LastTruePos)
                            {
                                MyUpCode = Table.UpdateAble.UpdateCodes[LastTruePos];
                                if (MyUpCode.UpdateCode < NextTrue)
                                    await Remote.Obj.Delete(MyUpCode.Key);
                                else
                                    break;
                            }
                            if (Table.UpdateAble.UpdateCodes.Length > LastTruePos &&
                               Table.UpdateAble.UpdateCodes[LastTruePos].UpdateCode == MyUpCode.UpdateCode)
                                StartPos = LastTruePos + 1;
                            else
                                StartPos = LastTruePos;
                        }
                        return false;
                    });

                    if (Break || StartPos <= EndPos)
                        break;

                    await Client.SendData((StartPos, Table.UpdateAble.UpdateCodes[StartPos].UpdateCode));
                    var Len = await Client.GetData<int>();
                    for(int i=0;i<Len; i++)
                    {
                        var Data = await Client.GetData<(ulong, DataType)>();
                    }
                }
            }

            //var TrueLen = await Client.GetData<int>();//6
            //if (Table.UpdateAble.UpdateCodes.Length != TrueLen)
            //{
            //    var ServerValues = new ulong?[TrueLen];
            //    await CompareToOther(async (pos) =>
            //    {
            //        var Value = ServerValues[pos];
            //        if (Value == null)
            //        {
            //            await Client.SendData(pos);//7
            //            Value = await Client.GetData<ulong>();//8
            //            ServerValues[pos] = Value;
            //        }
            //        return Value.Value;
            //    }, Table, Delete, TrueLen);
            //}
            await Client.SendData(-1);//7+


            Table.UpdateAble.UpdateCode = LastUpdateCode;
            if (IsPartOfTable)
                PartTable.SaveToParent();
            return Result;
        }
    }
}
