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
            private IAsyncOprations Client;
            private Table<ValueType, KeyType> Table;
            private Table<ValueType, KeyType> ParentTable;
            private UpdateAble<KeyType>[] UpdateCodes;
            private Func<KeyType, Task<ValueType>> GetItem;
            private bool IsPartOfTable;
            private PartOfTable<ValueType, KeyType> PartTable;
            public IRemoteUpdateSender() { }
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


                GetUpdateCodeAtPos = async (c) => UpdateCodes[c].UpdateCode;

            }

            [Remotable]
            public Func<int, Task<ulong>> GetUpdateCodeAtPos;

            public Func<KeyType, Task> Delete;
        }

        private static async Task I_SendUpdate<ValueType, KeyType>
                   (this IAsyncOprations Client,
                    Table<ValueType, KeyType> Table,
                    UpdateAble<KeyType>[] UpdateCodes,
                    Func<KeyType, Task<ValueType>> GetItem,
                    bool IsPartOfTable)
                   where KeyType : IComparable<KeyType>
        {
            Table<ValueType, KeyType> ParentTable = Table;
            PartOfTable<ValueType, KeyType> PartTable = null;
            if (IsPartOfTable)
            {
                PartTable = (PartOfTable<ValueType, KeyType>)Table;
                ParentTable = PartTable.Parent;
            }

            var LastUpdateCode = await Client.GetData<ulong>();//1
            await Client.SendData(Table.UpdateAble.UpdateCode);//2
            if (Table.UpdateAble.UpdateCode < LastUpdateCode)
                LastUpdateCode = 0;
            if (Table.UpdateAble.UpdateCode != LastUpdateCode)
            {
                await Client.SendData(Table.UpdateAble.UpdateCodes.Length);//3
                await Client.Remote(new IRemoteUpdateSender<ValueType, KeyType>(
                                     Client, Table, UpdateCodes, GetItem, IsPartOfTable),
                async () =>// Get From pos to update code
                {
                    var Data = await Client.GetData<(int, ulong)>();
                    var len = UpdateCodes.Length;
                    var Pos = Data.Item1;
                    var ClientUpCode = Data.Item2;
                    while (Pos < len)
                    {
                        var MyUpCode = UpdateCodes[Pos];
                        if(ClientUpCode>=MyUpCode.UpdateCode)
                        {
                            await Client.SendData(ulong.MinValue);
                            return;
                        }
                        await Client.SendData(MyUpCode.UpdateCode);
                        await Client.SendData( await GetItem(MyUpCode.Key));
                    }
                },
                async () =>// Get From pos to end
                {
                    var Pos = await Client.GetData<int>();
                    var len = UpdateCodes.Length;
                    for (int i = Pos; i < len; i++)
                    {
                        var MyUpCode = UpdateCodes[i];
                        await Client.SendData((MyUpCode.UpdateCode, await GetItem(MyUpCode.Key)));
                    }
                });
            }
        }
    }
}
