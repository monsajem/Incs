using System;
using System.Net.Http;
using System.Threading.Tasks;
using WebAssembly.Browser.DOM;

namespace Monsajem_Incs.Database.Base
{
    public static partial class Extentions
    {
        public static async Task<bool> GetUpdate<ValueType, KeyType>(
            this (string DBName, Uri CDN) CDN,
            Table<ValueType, KeyType> Table,
            Action<ValueType> MakeingUpdate = null)
            where KeyType : IComparable<KeyType>
        {
            var db = await IndexedDB.Open(CDN.DBName, 1, async (c) =>
            {
                var Table = c.CreateObjectStore("CDN");
                _ = Table.CreateIndex("Data", "Data");
            });
            try
            {
                _ = await GetUpdate(async (c) =>
                {
                    var trs = db.Transaction("CDN");
                    var tbl = trs.ObjectStore("CDN");
                    return await tbl.Get<byte[]>(c);
                }, Table, MakeingUpdate, null);
            }
            catch { }
            var Result = await GetUpdate(async (c) =>
            {
                var WebClient = new HttpClient();
                var Data = await WebClient.GetByteArrayAsync($"{CDN.CDN}{c}");

                var trs = db.Transaction("CDN");
                var tbl = trs.ObjectStore("CDN");

                _ = tbl.Put(Data, c);
                await trs.Commit();
                return Data;
            }, Table, MakeingUpdate,
            async (c) =>
            {
                var trs = db.Transaction("CDN");
                var tbl = trs.ObjectStore("CDN");
                await tbl.Delete(c);
                await trs.Commit();
            });
            db.Close();
            return Result;
        }

        public static async Task<bool> GetUpdate<ValueType_RLN, KeyType_RLN, ValueType, KeyType>(
            this (string DBName, Uri CDN) CDN,
            Table<ValueType_RLN, KeyType_RLN> RLNTable,
            KeyType_RLN RLNKey,
            Func<ValueType_RLN, PartOfTable<ValueType, KeyType>> GetRelation,
            Action<ValueType> MakeingUpdate = null)
            where KeyType : IComparable<KeyType>
            where KeyType_RLN : IComparable<KeyType_RLN>
        {
            var db = await IndexedDB.Open(CDN.DBName, 1, async (c) =>
            {
                var Table = c.CreateObjectStore("CDN");
                _ = Table.CreateIndex("Data", "Data");
            });
            try
            {
                _ = await GetUpdate(async (c) =>
                {
                    var trs = db.Transaction("CDN");
                    var tbl = trs.ObjectStore("CDN");
                    return await tbl.Get<byte[]>(c);
                }, RLNTable, RLNKey, GetRelation, MakeingUpdate, null);
            }
            catch { }
            var Result = await GetUpdate(async (c) =>
            {
                var WebClient = new HttpClient();
                var Data = await WebClient.GetByteArrayAsync($"{CDN.CDN}{c}");

                var trs = db.Transaction("CDN");
                var tbl = trs.ObjectStore("CDN");

                _ = tbl.Put(Data, "Data");
                await trs.Commit();
                return Data;
            }, RLNTable, RLNKey, GetRelation, MakeingUpdate,
            async (c) =>
            {
                var trs = db.Transaction("CDN");
                var tbl = trs.ObjectStore("CDN");
                await tbl.Delete(c);
                await trs.Commit();
            });
            db.Close();
            return Result;
        }

        public static Task<bool> GetUpdate<ValueType, KeyType>(
            this (Uri CDN, string DBName) CDN,
            Table<ValueType, KeyType> Table,
            Action<ValueType> MakeingUpdate = null)
            where KeyType : IComparable<KeyType>
        {
            return GetUpdate((CDN.DBName, CDN.CDN), Table, MakeingUpdate);
        }


        public static Task<bool> GetUpdate<ValueType_RLN, KeyType_RLN, ValueType, KeyType>(
            this (Uri CDN, string DBName) CDN,
            Table<ValueType_RLN, KeyType_RLN> RLNTable,
            KeyType_RLN RLNKey,
            Func<ValueType_RLN, PartOfTable<ValueType, KeyType>> GetRelation,
            Action<ValueType> MakeingUpdate = null)
            where KeyType : IComparable<KeyType>
            where KeyType_RLN : IComparable<KeyType_RLN>
        {
            return GetUpdate((CDN.DBName, CDN.CDN), RLNTable, RLNKey, GetRelation, MakeingUpdate);
        }
    }
}