using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Monsajem_Incs.Net.Base.Service;
using Monsajem_Incs.Database;
using System.Linq;
using Monsajem_Incs.Array.Hyper;
using System.Linq.Expressions;
using static Monsajem_Incs.ArrayExtentions.ArrayExtentions;

namespace Monsajem_Incs.Database.Base
{
    public static partial class Extentions
    {

        private static async Task I_SendUpdate<DataType, KeyType>
                   (this IAsyncOprations Client, Table<DataType, KeyType> Table)
                   where KeyType : IComparable<KeyType>
        {
            await Client.SendData(Table.UpdateAble.UpdateCode);
            if (await Client.GetCondition())
            {
                var Len = Table.KeysInfo.Keys.Length;
                var UpdateAble = Table.UpdateAble;
                var Keys = Table.KeysInfo.Keys;
                await Client.SendData((Table.KeysInfo.Keys, UpdateAble.UpdateCodes));
                var IsOk = await Client.GetData<bool[]>();
                for (int i = 0; i < Len; i++)
                {
                    if (IsOk[i])
                    {
                        var Data = Table.GetItem(i);
                        Table.ClearRelations?.Invoke(Data);
                        await Client.SendData(Data.Value);
                    };
                }
            }
        }

        private static async Task<bool> I_GetUpdate<DataType, KeyType>(
            this IAsyncOprations Client,
            Table<DataType, KeyType> Table,
            Action<DataType> MakeingUpdate = null)
            where KeyType : IComparable<KeyType>
        {
            var Result = false;
            if (Table.UpdateAble == null)
                Table.UpdateAble = new UpdateAble<DataType, KeyType>()
                { UpdateCode = 0, UpdateCodes = new ulong[Table.KeysInfo.Keys.Length] };
            var TableUpdateCode = await Client.GetData<ulong>();
            if (await Client.SendCondition(TableUpdateCode != Table.UpdateAble.UpdateCode))
            {
                var UpdateList = await Client.GetData((
                    Keys: Table.KeysInfo.Keys,
                    UpdateCodes: Table.UpdateAble.UpdateCodes));
                Table.GetElseItems(UpdateList.Keys).Delete((info) =>
                    DeleteByPosition(ref Table.UpdateAble.UpdateCodes, info.Pos));
                var Conditions = (
                    NeedUpdate: new bool[UpdateList.Keys.Length],
                    Pos: new int[UpdateList.Keys.Length]);
                for (int i = 0; i < UpdateList.Keys.Length; i++)
                {
                    var Pos = Table.PositionOf(UpdateList.Keys[i]);
                    if (Pos > -1)
                    {
                        Conditions.NeedUpdate[i] = Table.UpdateAble.UpdateCodes[Pos] < UpdateList.UpdateCodes[i];
                    }
                    else
                    {
                        Conditions.NeedUpdate[i] = true;
                    }
                    Conditions.Pos[i] = Pos;
                }
                await Client.SendData(Conditions.NeedUpdate);
                for (int i = 0; i < UpdateList.Keys.Length; i++)
                {
                    if (Conditions.NeedUpdate[i] == true)
                    {
                        var Data = await Client.GetData<DataType>();
                        MakeingUpdate?.Invoke(Data);
                        if (Conditions.Pos[i] > -1)
                        {
                            Table.Update(UpdateList.Keys[i], (c) =>
                            {
                                Table.MoveRelations(c, Data);
                                return Data;
                            });
                            Table.UpdateAble.UpdateCodes[Conditions.Pos[i]] = UpdateList.UpdateCodes[i];
                        }
                        else
                        {
                            var Pos = Table.Insert(Data);
                            ArrayExtentions.ArrayExtentions.Insert(
                                ref Table.UpdateAble.UpdateCodes, UpdateList.UpdateCodes[i], Pos);
                        }
                        Result = true;
                    }
                }
                Table.UpdateAble.UpdateCode = TableUpdateCode;
                if (Table.Length > 0)
                    Table.Update(Table.KeysInfo.Keys[0], (c) => { });
            }
            return Result;
        }

        private static async Task I_SendUpdate<DataType, KeyType>
            (this IAsyncOprations Client,
            PartOfTable<DataType, KeyType> Table)
            where KeyType : IComparable<KeyType>
        {
            Func<Task> SendUpdate = async () =>
            {
                var Len = Table.KeysInfo.Keys.Length;
                var UpdateAble = Table.Parent.UpdateAble;
                var Keys = Table.KeysInfo.Keys;
                var UpdateCodes = new ulong[Keys.Length];
                for (int i = 0; i < Len; i++)
                {
                    UpdateCodes[i] = Table.Parent.UpdateAble.UpdateCodes[Table.Parent.PositionOf(Keys[i])];
                }
                await Client.SendData((Table.KeysInfo.Keys, UpdateCodes));
                var IsOk = await Client.GetData<bool[]>();
                for (int i = 0; i < Len; i++)
                {
                    if (IsOk[i])
                    {
                        var Data = Table.GetItem(i);
                        Table.Parent.ClearRelations?.Invoke(Data);
                        await Client.SendData(Data.Value);
                    };
                }
            };

            if (await Client.SendCondition(Table.UpdateAble != null))
            {
                await Client.SendData(Table.UpdateAble.UpdateCode);
                if (await Client.GetCondition())
                {
                    await SendUpdate();
                }
            }
            else
            {
                await SendUpdate();
            }
        }

        private static async Task<bool> I_GetUpdate<DataType, KeyType>(
            this IAsyncOprations Client,
            PartOfTable<DataType, KeyType> RelationTable,
            Action<DataType> MakeingUpdate = null)
            where KeyType : IComparable<KeyType>
        {
            var Result = false;
            Func<Task> Update = async () =>
            {
                var UpdateList = await Client.GetData((
                    Keys: RelationTable.KeysInfo.Keys,
                    UpdateCodes: RelationTable.UpdateAble.UpdateCodes));
                var Igonres = RelationTable.GetElseItems(UpdateList.Keys);

                RelationTable.Ignore(RelationTable.GetElseItems(UpdateList.Keys).KeysInfo.Keys.ToArray());

                var Conditions = (
                    NeedUpdate: new bool[UpdateList.Keys.Length],
                    Pos: new int[UpdateList.Keys.Length]);
                for (int i = 0; i < UpdateList.Keys.Length; i++)
                {
                    var Pos = RelationTable.Parent.PositionOf(UpdateList.Keys[i]);
                    if (Pos > -1)
                    {
                        Conditions.NeedUpdate[i] = RelationTable.Parent.UpdateAble.UpdateCodes[Pos] < UpdateList.UpdateCodes[i];
                    }
                    else
                    {
                        Conditions.NeedUpdate[i] = true;
                    }
                    Conditions.Pos[i] = Pos;
                }
                await Client.SendData(Conditions.NeedUpdate);
                for (int i = 0; i < UpdateList.Keys.Length; i++)
                {
                    int Pos;
                    if (Conditions.NeedUpdate[i] == true)
                    {
                        var Data = await Client.GetData<DataType>();
                        MakeingUpdate?.Invoke(Data);
                        if (Conditions.Pos[i] > -1)
                        {
                            RelationTable.Parent.Update(UpdateList.Keys[i], (c) =>
                            {
                                RelationTable.Parent.MoveRelations(c, Data);
                                return Data;
                            });
                            RelationTable.Parent.UpdateAble.UpdateCodes[Conditions.Pos[i]] = UpdateList.UpdateCodes[i];
                        }
                        else
                        {
                            Pos = RelationTable.Insert(Data);
                            ArrayExtentions.ArrayExtentions.Insert(
                                    ref RelationTable.Parent.UpdateAble.UpdateCodes, UpdateList.UpdateCodes[i], Pos);
                        }
                        Result = true;
                    }
                    Pos = RelationTable.PositionOf(UpdateList.Keys[i]);
                    if (Pos < 0)
                    {
                        Result = true;
                        RelationTable.Accept(UpdateList.Keys[i]);
                    }
                }
            };


            var Table = RelationTable.Parent;
            if (Table.UpdateAble == null)
                Table.UpdateAble = new UpdateAble<DataType, KeyType>()
                {
                    UpdateCode = 0,
                    UpdateCodes = new ulong[Table.KeysInfo.Keys.Length]
                };

            if (RelationTable.UpdateAble == null)
                RelationTable.UpdateAble = new UpdateAble<DataType, KeyType>();

            if (await Client.GetCondition())
            {
                var TableUpdateCode = await Client.GetData<ulong>();
                if (await Client.SendCondition(TableUpdateCode != RelationTable.UpdateAble.UpdateCode))
                {
                    await Update();
                    RelationTable.UpdateAble.UpdateCode = TableUpdateCode;
                }
            }
            else
            {
                await Update();
            }
            return Result;
        }

    }
}
