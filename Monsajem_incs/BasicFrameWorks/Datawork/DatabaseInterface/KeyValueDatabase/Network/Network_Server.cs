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
        private enum UpdateCMD:int
        {
            GetUpdateCodeAtPos,

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

                await Client.RemoteServices(
                async (c) =>
                {
                    return Table.UpdateAble.UpdateCodes[(int)c];
                });
                
            }
        }
    }
}
