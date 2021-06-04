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
            Func<(int Position, object Data), Task<object>> service,
            int StartPos, int EndPos)
            {
                var Update = Table.UpdateAble;
                while (StartPos < EndPos)
                {
                    if (Update.UpdateCodes[EndPos].UpdateCode != 
                        (ulong) await service(((int)UpdateCMD.GetUpdateCodeAtPos,EndPos)))
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

            static async Task DeleteFrom(
                Table<DataType, KeyType> Table,
                Func<KeyType, Task> Delete,
                ulong UpdateCode)
            {
                var UpdateCodes = Table.UpdateAble.UpdateCodes;
                var Place = System.Array.BinarySearch(UpdateCodes,
                            new UpdateAble<KeyType>() { UpdateCode = UpdateCode },
                            UpdateAble<KeyType>.CompareCode);
                if (Place < 0)
                    Place = (Place * -1) - 1;
                else
                    Place += 1;
                foreach (var Update in UpdateCodes.Skip(Place))
                {
                    await Delete(Update.Key);
                }
            }


            Table<DataType, KeyType> ParentTable = Table;
            PartOfTable<DataType, KeyType> PartTable = null;
            Func<KeyType, Task> Delete;
            if (Table._UpdateAble == null)
                Table._UpdateAble = new UpdateAbles<KeyType>(0);

            if (IsPartOfTable)
            {
                PartTable = (PartOfTable<DataType, KeyType>)Table;
                ParentTable = PartTable.Parent;
                if (ParentTable._UpdateAble == null)
                    ParentTable._UpdateAble = new UpdateAbles<KeyType>(0);
                var Part_UpdateAble = PartTable.UpdateAble;
                var Parent_UpdateAble = ParentTable.UpdateAble;
                Delete = async (key) =>
                {
                    await Client.SendData(-2);
                    await Client.SendData(key);
                    Part_UpdateAble.DeleteDontUpdate(key);
                    if (await Client.GetData<bool>())
                        PartTable.Ignore(key);
                    else
                    {
                        Parent_UpdateAble.DeleteDontUpdate(key);
                        PartTable.Delete(key);
                        Deleted?.Invoke(key);
                    }
                };
            }
            else
            {
                var UpdateAble = Table.UpdateAble;
                Delete = async (key) =>
                {
                    UpdateAble.DeleteDontUpdate(key);
                    Table.Delete(key);
                    Deleted?.Invoke(key);
                };
            }

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
                await Client.RunServices(async (c) =>
                {
                    var ServerItemsCount = await Client.GetData<int>();

                    var StartPos = 0;
                    var EndPos = Math.Min(Table.UpdateAble.UpdateCodes.Length, ServerItemsCount) - 1;

                    while (EndPos != ServerItemsCount - 1)
                    {
                        var ServerValues = new ulong?[ServerItemsCount];
                        var LastTruePos = await FindLastTrue(Table,c, StartPos, EndPos);

                        if (LastTruePos == ServerItemsCount - 1)
                            break;

                    }
                });

               

                await DeleteFrom(Table, Delete, await Client.GetData<ulong>());//3
                if (IsPartOfTable)
                    await Client.SendData(-1);

                bool[] NeedUpdate;
                var UpdateCodes = await Client.GetData<ulong[]>();//4
                ulong[] PartUpdateCodes = null;
                if (IsPartOfTable)
                    PartUpdateCodes = await Client.GetData<ulong[]>();//5

                NeedUpdate = new bool[UpdateCodes.Length];

                if (IsPartOfTable)
                    for (int i = 0; i < UpdateCodes.Length; i++)
                        NeedUpdate[i] = ParentTable.UpdateAble.IsExist(UpdateCodes[i]) == false;
                else
                    for (int i = 0; i < UpdateCodes.Length; i++)
                        NeedUpdate[i] = Table.UpdateAble.IsExist(UpdateCodes[i]) == false;

                await Client.SendData(NeedUpdate);//5

                for (int i = 0; i < NeedUpdate.Length; i++)
                {
                    if (NeedUpdate[i] == true)
                    {
                        var Data = await Client.GetData<DataType>();//6
                        MakeingUpdate?.Invoke(Data);
                        var Key = Table.GetKey(Data);
                        if (ParentTable.PositionOf(Key) > -1)
                        {
                            ParentTable.Update(Key, (c) =>
                            {
                                ParentTable.MoveRelations(c, Data);
                                return Data;
                            });
                            ParentTable.UpdateAble.Changed(Key, Key, UpdateCodes[i]);
                        }
                        else
                        {
                            ParentTable.Insert(Data);
                            ParentTable.UpdateAble.Insert(Key, UpdateCodes[i]);
                        }
                        if (IsPartOfTable)
                        {
                            if (PartTable.PositionOf(Key) > -1)
                            {
                                PartTable.UpdateAble.Changed(Key, Key, PartUpdateCodes[i]);
                            }
                            else
                            {
                                PartTable.UpdateAble.Insert(Key, PartUpdateCodes[i]);
                                PartTable.Accept(Key);
                            }
                        }
                    }
                    else if (IsPartOfTable)
                    {
                        var Data = ParentTable[ParentTable.UpdateAble[UpdateCodes[i]].Key];//6
                        MakeingUpdate?.Invoke(Data);
                        var Key = Table.GetKey(Data);
                        if (PartTable.PositionOf(Key) > -1)
                            PartTable.UpdateAble.Changed(Key, Key, PartUpdateCodes[i]);
                        else
                        {
                            PartTable.UpdateAble.Insert(Key, PartUpdateCodes[i]);
                            PartTable.Accept(Key);
                        }
                    }
                    Table.UpdateAble.UpdateCode = UpdateCodes[i];
                    Result = true;
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
