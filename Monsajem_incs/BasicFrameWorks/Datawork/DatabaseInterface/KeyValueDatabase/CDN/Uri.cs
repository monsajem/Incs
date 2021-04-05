using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Monsajem_Incs.Net.Base.Service;
using Monsajem_Incs.Database;
using System.Linq;
using Monsajem_Incs.Collection.Array.TreeBased;
using System.Linq.Expressions;
using Monsajem_Incs.Serialization;
using System.Net.Http;

namespace Monsajem_Incs.Database.Base
{
    public static partial class Extentions
    {
        public static Task<bool> GetUpdate<ValueType, KeyType>(
            this Uri CDN,
            Table<ValueType, KeyType> Table,
            Action<ValueType> MakeingUpdate = null)
            where KeyType : IComparable<KeyType>
        {

            return GetUpdate(async (c) =>
            {
                var WebClient = new HttpClient();
                return await WebClient.GetByteArrayAsync($"{CDN}{c}");
            }, Table, MakeingUpdate,null);
        }

        public static Task<bool> GetUpdate<ValueType_RLN, KeyType_RLN, ValueType, KeyType>(
            this Uri CDN,
            Table<ValueType_RLN, KeyType_RLN> RLNTable,
            KeyType_RLN RLNKey,
            Func<ValueType_RLN, PartOfTable<ValueType, KeyType>> GetRelation,
            Action<ValueType> MakeingUpdate = null)
            where KeyType : IComparable<KeyType>
            where KeyType_RLN : IComparable<KeyType_RLN>
        {
            return GetUpdate(async (c) =>
            {
                var WebClient = new HttpClient();
                return await WebClient.GetByteArrayAsync($"{CDN}{c}");
            }, RLNTable,RLNKey,GetRelation, MakeingUpdate,null);
        }
    }
}