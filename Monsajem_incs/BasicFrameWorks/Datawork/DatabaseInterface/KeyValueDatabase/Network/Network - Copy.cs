//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Monsajem_Incs.Net.Base.Service;
//using Monsajem_Incs.Database;
//using System.Linq;
//using Monsajem_Incs.Array.Hyper;
//using System.Linq.Expressions;
//using static Monsajem_Incs.ArrayExtentions.ArrayExtentions;

//namespace Monsajem_Incs.Database.Base
//{
//    public static partial class Extentions
//    {
//        private static (IEnumerable<UpdateAble<KeyType>> UpdateCodes, IEnumerable<ValueType> Values, ulong FirstCode)
//            Base_GetNexts<KeyType, ValueType>(
//            Table<ValueType, KeyType> Table,
//            ulong UpdateCode)
//            where KeyType : IComparable<KeyType>
//        {
//            var UpdateCodes = Table.UpdateAble.UpdateCodes;
//            var Place = System.Array.BinarySearch(UpdateCodes,
//                        new UpdateAble<KeyType>() { UpdateCode = UpdateCode },
//                        UpdateAble<KeyType>.CompareCode);
//            if (Place < 0)
//                Place = (Place * -1) - 1;
//            else
//                Place += 1;

//            ulong FirstCode;
//            if (Place == 0)
//                FirstCode = UpdateCodes[0].UpdateCode;
//            else
//                FirstCode = UpdateCodes[Place - 1].UpdateCode;

//            var ResultCodes = UpdateCodes.Skip(Place);
//            var Values = ResultCodes.Select((c) => {
//                var Data = Table[c.Key];
//                Table.ClearRelations?.Invoke(Data);
//                return Data.Value;
//            });
//            return (ResultCodes, Values, FirstCode);
//        }
//        private static (ulong[] UpdateCodes, IEnumerable<ValueType> Values, ulong FirstCode)
//            GetNexts<KeyType, ValueType>(
//            Table<ValueType, KeyType> Table,
//            ulong UpdateCode)
//            where KeyType : IComparable<KeyType>
//        {
//            var UpdateCodes = Base_GetNexts(Table, UpdateCode);

//            var Updates = UpdateCodes.UpdateCodes.Select((c) => c.UpdateCode).ToArray();
//            return (Updates, UpdateCodes.Values, UpdateCodes.FirstCode);
//        }
//        private static ((ulong ParentCode, ulong PartCode)[] UpdateCodes, IEnumerable<ValueType> Values, ulong FirstCode)
//            GetNexts<KeyType, ValueType>(
//            PartOfTable<ValueType, KeyType> Table,
//            ulong UpdateCode)
//            where KeyType : IComparable<KeyType>
//        {
//            var UpdateCodes = Base_GetNexts(Table, UpdateCode);
//            var ParentUpdateAble = Table.Parent.UpdateAble;
//            var Updates = UpdateCodes.UpdateCodes.Select((c) =>
//                (ParentUpdateAble[c.Key].UpdateCode, c.UpdateCode)).ToArray();

//            return (Updates, UpdateCodes.Values, UpdateCodes.FirstCode);
//        }

//        private static async Task I_SendUpdate<ValueType, KeyType>
//                   (this IAsyncOprations Client, Table<ValueType, KeyType> Table, bool IsPartOfTable)
//                   where KeyType : IComparable<KeyType>
//        {
//            var LastUpdateCode = await Client.GetData<ulong>();//1
//            await Client.SendData(Table.UpdateAble.UpdateCode);//2
//            if (Table.UpdateAble.UpdateCode != LastUpdateCode)
//            {
//                IEnumerator<ValueType> IE_Values;
//                if (IsPartOfTable)
//                {
//                    var NextUpdates = GetNexts((PartOfTable<ValueType, KeyType>)Table, LastUpdateCode);
//                    await Client.SendData(NextUpdates.FirstCode);//3
//                    await Client.SendData(NextUpdates.UpdateCodes);//4
//                    IE_Values = NextUpdates.Values.GetEnumerator();
//                }
//                else
//                {
//                    var NextUpdates = GetNexts(Table, LastUpdateCode);
//                    await Client.SendData(NextUpdates.FirstCode);//3
//                    await Client.SendData(NextUpdates.UpdateCodes);//4
//                    IE_Values = NextUpdates.Values.GetEnumerator();
//                }
//                var SendToClient = await Client.GetData<bool[]>();//5

//                IE_Values.MoveNext();
//                var Len = SendToClient.Length;
//                for (int i = 0; i < Len; i++)
//                {
//                    if (SendToClient[i] == true)
//                        await Client.SendData(IE_Values.Current);//6
//                    IE_Values.MoveNext();
//                }
//                IE_Values.Dispose();
//            }

//            var UpdateAble = Table.UpdateAble.UpdateCodes;
//            await Client.SendData(UpdateAble.Length);
//            var Pos = await Client.GetData<int>();
//            while (Pos != -1)
//            {
//                await Client.SendData(UpdateAble[Pos].UpdateCode);
//                Pos = await Client.GetData<int>();
//            }
//        }


//        private static async Task CompareToOther<KeyType, ValueType>(
//            Func<int, Task<ulong>> Ar_SV,
//            Table<ValueType, KeyType> Table,
//            int TrueLen)
//            where KeyType : IComparable<KeyType>
//        {
//            while (Table.UpdateAble.UpdateCodes.Length != TrueLen)
//            {
//                var UpdateCodes = Table.UpdateAble.UpdateCodes;
//                int EndPos = TrueLen - 1;
//                int BeginPos = 0;
//                int MidPos = (EndPos + BeginPos + 1) / 2;
//                while (EndPos != BeginPos &&
//                      EndPos != MidPos &&
//                      BeginPos != MidPos)
//                {
//                    var E = await Ar_SV(EndPos) == UpdateCodes[EndPos].UpdateCode;
//                    var B = await Ar_SV(BeginPos) == UpdateCodes[BeginPos].UpdateCode;
//                    var M = await Ar_SV(MidPos) == UpdateCodes[MidPos].UpdateCode;
//                    if (E == false && M == false && B == true)
//                    {
//                        EndPos = BeginPos - 1;
//                        MidPos = (EndPos + BeginPos + 1) / 2;
//                    }
//                    else if (E == false && M == true && B == true)
//                    {
//                        BeginPos = MidPos + 1;
//                        MidPos = (EndPos + BeginPos + 1) / 2;
//                    }
//                    else if (E == true && M == true && B == true)
//                    {
//                        EndPos = EndPos + 1;
//                        MidPos = EndPos;
//                        BeginPos = EndPos;
//                    }
//                    else if (E == false && M == false && B == false)
//                    {
//                        EndPos = BeginPos;
//                        MidPos = BeginPos;
//                    }
//                }
//                var Key = UpdateCodes[EndPos].Key;
//                Table.Delete(Key);
//                Table.UpdateAble.Delete(Key);
//            }
//        }

//        private static void DeleteFrom<KeyType, ValueType>(
//            Table<ValueType, KeyType> Table,
//            ulong UpdateCode)
//            where KeyType : IComparable<KeyType>
//        {
//            var UpdateCodes = Table.UpdateAble.UpdateCodes;
//            var Place = System.Array.BinarySearch(UpdateCodes,
//                        new UpdateAble<KeyType>() { UpdateCode = UpdateCode },
//                        UpdateAble<KeyType>.CompareCode);
//            if (Place < 0)
//                Place = (Place * -1) - 1;
//            else
//                Place += 1;
//            var UpdateAble = Table.UpdateAble;
//            foreach (var Update in UpdateCodes.Skip(Place))
//            {
//                UpdateAble.Delete(Update.Key, 0);
//                Table.Delete(Update.Key);
//            }
//        }
//        private static async Task<bool> I_GetUpdate<DataType, KeyType>(
//            this IAsyncOprations Client,
//            Table<DataType, KeyType> Table,
//            Action<DataType> MakeingUpdate,
//            bool IsPartOfTable)
//            where KeyType : IComparable<KeyType>
//        {
//            var Result = false;
//            if (Table.UpdateAble == null)
//                Table.UpdateAble = new UpdateAbles<KeyType>(0);

//            await Client.SendData(Table.UpdateAble.UpdateCode);//1
//            var LastUpdateCode = await Client.GetData<ulong>();//2
//            if (Table.UpdateAble.UpdateCode != LastUpdateCode)
//            {
//                DeleteFrom(Table, await Client.GetData<ulong>());//3

//                bool[] NeedUpdate;
//                if (IsPartOfTable)
//                {
//                    var UpdateCodes = await Client.GetData<(ulong ParentCode, ulong PartCode)[]>();//4
//                    NeedUpdate = new bool[UpdateCodes.Length];
//                    for (int i = 0; i < UpdateCodes.Length; i++)
//                        NeedUpdate[i] = Table.UpdateAble.IsExist(UpdateCodes[i]) == false;
//                    await Client.SendData(NeedUpdate);//5
//                }
//                else
//                {
//                    var UpdateCodes = await Client.GetData<ulong[]>();//4
//                    NeedUpdate = new bool[UpdateCodes.Length];
//                    for (int i = 0; i < UpdateCodes.Length; i++)
//                        NeedUpdate[i] = Table.UpdateAble.IsExist(UpdateCodes[i]) == false;
//                    await Client.SendData(NeedUpdate);//5
//                }

//                for (int i = 0; i < NeedUpdate.Length; i++)
//                {
//                    if (NeedUpdate[i] == true)
//                    {
//                        var Data = await Client.GetData<DataType>();//6
//                        MakeingUpdate?.Invoke(Data);
//                        var Key = Table.GetKey(Data);
//                        if (Table.PositionOf(Data) > -1)
//                        {
//                            Table.Update(Key, (c) =>
//                            {
//                                Table.MoveRelations(c, Data);
//                                return Data;
//                            });
//                            Table.UpdateAble.Changed(Key, Key, UpdateCodes[i]);
//                        }
//                        else
//                        {
//                            Table.Insert(Data);
//                            Table.UpdateAble.Insert(Key, UpdateCodes[i]);
//                        }
//                    }
//                    Table.UpdateAble.UpdateCode = UpdateCodes[i];
//                    Result = true;
//                }
//            }

//            var TrueLen = await Client.GetData<int>();//6
//            if (Table.UpdateAble.UpdateCodes.Length != TrueLen)
//            {
//                var ServerValues = new ulong?[TrueLen];
//                await CompareToOther(async (pos) =>
//                {
//                    var Value = ServerValues[pos];
//                    if (Value == null)
//                    {
//                        await Client.SendData(pos);//7
//                        Value = await Client.GetData<ulong>();//8
//                        ServerValues[pos] = Value;
//                    }
//                    return Value.Value;
//                }, Table, TrueLen);
//            }
//            await Client.SendData(-1);//7+


//            Table.UpdateAble.UpdateCode = LastUpdateCode;
//            return Result;
//        }
//    }
//}
