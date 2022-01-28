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
        private class IRemoteUpdateSender<ValueType, KeyType>
            where KeyType : IComparable<KeyType>
        {
            protected IAsyncOprations Client;
            protected Table<ValueType, KeyType> Table;
            protected Table<ValueType, KeyType> ParentTable;
            protected UpdateAble<KeyType>[] UpdateCodes;
            protected Func<KeyType, Task<ValueType>> GetItem;
            protected bool IsPartOfTable;
            protected PartOfTable<ValueType, KeyType> PartTable;
            public IRemoteUpdateSender(
                IAsyncOprations Client,
                Table<ValueType, KeyType> Table,
                UpdateAble<KeyType>[] UpdateCodes,
                Func<KeyType, Task<ValueType>> GetItem,
                bool IsPartOfTable)
            {
                this.Client = Client;
                this.Table = Table;
                this.UpdateCodes = UpdateCodes;
                this.GetItem = GetItem;
                this.IsPartOfTable = IsPartOfTable;
                if (IsPartOfTable)
                {
                    PartTable = (PartOfTable<ValueType, KeyType>)Table;
                    ParentTable = PartTable.Parent;
                    Table.MoveRelations = ParentTable.MoveRelations;
                }

                GetUpdateCodeAtPos = async (c) => UpdateCodes[c].UpdateCode;
                IsExistAtParent = async (c) => ParentTable.IsExist(c);

                Func<UpdateAble<KeyType>[], Task> SendUpdate = async (MyUpCodes) =>
                {
                    var IsPartOfTable = this.IsPartOfTable;
                    var len = MyUpCodes.Length;
                    await Client.SendData(len);
                    for (int i = 0; i < len; i++)
                    {
                        var MyUpCode = MyUpCodes[i];
                        await Client.SendData(MyUpCode.UpdateCode);
                        if(IsPartOfTable)
                            await Client.SendData(ParentTable.UpdateAble[MyUpCode.Key].UpdateCode);
                    }
                    for (int i = 0; i < len; i++)
                    {
                        if(await Client.GetData<bool>())
                            await Client.SendData(await GetItem(MyUpCodes[i].Key));
                    }
                };

                UpdateFromPosToUpCode = async (Pos, ClientUpCode) =>
                {
                    var MyUpCodes = UpdateCodes.
                                    Skip(Pos).
                                    TakeWhile((c) => ClientUpCode >= c.UpdateCode).ToArray();
                    await SendUpdate(MyUpCodes);
                };
                UpdateFromPosToEnd = async (Pos) =>
                {
                    var MyUpCodes = UpdateCodes.Skip(Pos).ToArray();
                    await SendUpdate(MyUpCodes);
                };

#if DEBUG_UpdateAble
                Debuger = async () =>
                {
                    await Client.SendData(Table.UpdateAble.UpdateCodes);//1
                    await Client.SendData(Table.KeysInfo.Keys.ToArray());//2
                };
#endif
            }

            [Remotable]
            protected Func<int, Task<ulong>> GetUpdateCodeAtPos;
            [Remotable]
            protected Func<KeyType, Task<bool>> IsExistAtParent;

            [Syncable]
            protected Func<int,ulong, Task> UpdateFromPosToUpCode;
            [Syncable]
            protected Func<int, Task> UpdateFromPosToEnd;

#if DEBUG_UpdateAble
            [Syncable]
            protected Func<Task> Debuger;
#endif

        }

        private static async Task I_SendUpdate<ValueType, KeyType>
                   (this IAsyncOprations Client,
                    Table<ValueType, KeyType> Table,
                    UpdateAble<KeyType>[] UpdateCodes,
                    Func<KeyType, Task<ValueType>> GetItem,
                    bool IsPartOfTable)
                   where KeyType : IComparable<KeyType>
        {
            var LastUpdateCode = await Client.GetData<ulong>();//1
            await Client.SendData(Table.UpdateAble.UpdateCode.Value);//2
            if (Table.UpdateAble.UpdateCode < LastUpdateCode)
                LastUpdateCode = 0;
            if (Table.UpdateAble.UpdateCode != LastUpdateCode)
            {
                await Client.SendData(Table.UpdateAble.UpdateCodes.Length);//3
                var Remote = new IRemoteUpdateSender<ValueType, KeyType>(
                                    Client, Table, UpdateCodes, GetItem, IsPartOfTable);
                await Client.Remote(Remote);
            }
        }
    }
}
