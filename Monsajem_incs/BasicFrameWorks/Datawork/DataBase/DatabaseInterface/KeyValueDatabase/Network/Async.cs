﻿using Monsajem_Incs.Net.Base.Service;
using System;
using System.Threading.Tasks;

namespace Monsajem_Incs.Database.Base
{
    public static partial class Extentions
    {
        public static Task SendUpdate<DataType, KeyType>
          (this IAsyncOprations Client, Table<DataType, KeyType> Table)
          where KeyType : IComparable<KeyType>
          => Client.I_SendUpdate(
              Table,
              Table.UpdateAble.UpdateCodes,
              async (key) => Table[key].Value, false);

        public static Task<bool> GetUpdate<DataType, KeyType>(
            this IAsyncOprations Client,
            Table<DataType, KeyType> Table,
            Action<DataType> MakeingUpdate = null)
            where KeyType : IComparable<KeyType>
            => Client.I_GetUpdate(Table, MakeingUpdate, null, false);

        public static Task SendUpdate<DataType, KeyType>
            (this IAsyncOprations Client,
            PartOfTable<DataType, KeyType> Table)
            where KeyType : IComparable<KeyType>
            => Client.I_SendUpdate(
                Table,
                Table.UpdateAble.UpdateCodes,
                async (key) => Table[key].Value, true);

        public static Task<bool> GetUpdate<DataType, KeyType>(
            this IAsyncOprations Client,
            PartOfTable<DataType, KeyType> RelationTable,
            Action<DataType> MakeingUpdate = null)
            where KeyType : IComparable<KeyType>
            => Client.I_GetUpdate(RelationTable, MakeingUpdate, null, true);
    }
}
