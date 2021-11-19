using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Monsajem_Incs.Net.Base.Service;
using Monsajem_Incs.Database;
using System.Linq;
using Monsajem_Incs.Collection.Array.TreeBased;
using System.Linq.Expressions;
using Monsajem_Incs.Serialization;
using WebAssembly.Browser.DOM;
using System.Runtime.InteropServices.JavaScript;
using System.Net.Http;

namespace Monsajem_Incs.Database.Base
{
    public static partial class Extentions
    {
        public static async Task<bool> GetUpdate<ValueType, KeyType>(
            this (string DBName,Uri CDN) CDN,
            Table<ValueType, KeyType> Table,
            Action<ValueType> MakeingUpdate = null)
            where KeyType : IComparable<KeyType>
        {
            var db = await IndexedDB.Open(CDN.DBName, 1, async (c) =>
            { 
                var Table = c.CreateObjectStore("CDN");
                Table.CreateIndex("Data", "Data");
            });
            try
            {
                await GetUpdate(async (c) =>
                {
                    var trs = db.Transaction("CDN");
                    var tbl = trs.ObjectStore("CDN");
                    return (await tbl.Get<Uint8Array>(c)).ToArray();
                }, Table, MakeingUpdate,null);
            }catch{}
            var Result = await GetUpdate(async (c) =>
            {
                var WebClient = new HttpClient();
                var Data = await WebClient.GetByteArrayAsync($"{CDN.CDN}{c}");

                var trs = db.Transaction("CDN");
                var tbl = trs.ObjectStore("CDN");

                tbl.Put(Uint8Array.From(Data), c);
                await trs.Commit();
                return Data;
            }, Table, MakeingUpdate,
            async (c)=> {
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
                Table.CreateIndex("Data", "Data");
            });
            try
            {
                await GetUpdate(async (c) =>
                {
                    var trs = db.Transaction("CDN");
                    var tbl = trs.ObjectStore("CDN");
                    return (await tbl.Get<Uint8Array>(c)).ToArray();
                }, RLNTable, RLNKey, GetRelation, MakeingUpdate,null);
            }
            catch{}
            var Result = await GetUpdate(async (c) =>
            {
                var WebClient = new HttpClient();
                var Data = await WebClient.GetByteArrayAsync($"{CDN.CDN}{c}");

                var trs = db.Transaction("CDN");
                var tbl = trs.ObjectStore("CDN");

                tbl.Put(Uint8Array.From(Data), "Data");
                await trs.Commit();
                return Data;
            }, RLNTable, RLNKey, GetRelation, MakeingUpdate, 
            async (c) => {
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
            return GetUpdate((CDN.DBName, CDN.CDN), RLNTable,RLNKey,GetRelation, MakeingUpdate);
        }
    }
}