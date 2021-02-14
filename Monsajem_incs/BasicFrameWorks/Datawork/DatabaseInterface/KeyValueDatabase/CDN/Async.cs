using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Monsajem_Incs.Net.Base.Service;
using Monsajem_Incs.Database;
using System.Linq;
using Monsajem_Incs.Array.Hyper;
using System.Linq.Expressions;
using Monsajem_Incs.Serialization;
using System.Net;

namespace Monsajem_Incs.Database.Base
{
    public static partial class Extentions
    {
        public static async Task<bool> GetUpdate<ValueType, KeyType>(
            this Uri CDN,
            Table<ValueType, KeyType> Table,
            Action<ValueType> MakeingUpdate = null)
            where KeyType : IComparable<KeyType>
        {
            CDN = new Uri($"{CDN.ToString()}/{Table.TableName}");
            var Socket = new Net.Virtual.Socket();
            var Server = new Net.Virtual.AsyncOprations(Socket);
            var Client = new Net.Virtual.AsyncOprations(Socket.OtherSide);

            var WebClient = new WebClient();
            var ServerTable =(await WebClient.DownloadDataTaskAsync(CDN.ToString()+"/K")).Deserialize(Table);
            ServerTable.ClearRelations = Table.ClearRelations;

            Server.I_SendUpdate(ServerTable, ServerTable.UpdateAble.UpdateCodes,
                async(key) =>
                {
                    return (await WebClient.DownloadDataTaskAsync(CDN.ToString() + "/V/" +
                        Convert.ToBase64String(key.Serialize()))).Deserialize<ValueType>();
                },false);

            return await Client.I_GetUpdate(Table, MakeingUpdate, false);
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
            var PartTable = GetRelation(RLNTable[RLNKey]);
            var RootCDN = new Uri( $"{CDN.ToString()}/{PartTable.Parent.TableName}");
            var RelationCDN = new Uri($"{CDN.ToString()}/{RLNTable.TableName}");
            return GetUpdate(RootCDN, RelationCDN, RLNTable, RLNKey, GetRelation, MakeingUpdate);
        }

        public static async Task<bool> GetUpdate<ValueType_RLN,KeyType_RLN,ValueType, KeyType>(
            this Uri RootCDN,
            Uri RelationCDN,
            Table<ValueType_RLN,KeyType_RLN> RLNTable,
            KeyType_RLN RLNKey,
            Func<ValueType_RLN, PartOfTable<ValueType, KeyType>> GetRelation,
            Action<ValueType> MakeingUpdate = null)
            where KeyType : IComparable<KeyType>
            where KeyType_RLN : IComparable<KeyType_RLN>
        {
            var Socket = new Net.Virtual.Socket();
            var Server = new Net.Virtual.AsyncOprations(Socket);
            var Client = new Net.Virtual.AsyncOprations(Socket.OtherSide);

            var WebClient = new WebClient();

            var ServerTable = (await WebClient.DownloadDataTaskAsync(
                                RootCDN.ToString() + "/K")).Deserialize<Table<ValueType,KeyType>>();
            var ServerPartTable =
                GetRelation((await WebClient.DownloadDataTaskAsync(
                    RelationCDN.ToString() + "/V/"+Convert.ToBase64String(RLNKey.Serialize()))).Deserialize<ValueType_RLN>());

            Server.I_SendUpdate(ServerPartTable, ServerTable.UpdateAble.UpdateCodes,
                async (key) =>
                {
                    return (await WebClient.DownloadDataTaskAsync(RootCDN.ToString() + "/V/" +
                        Convert.ToBase64String(key.Serialize()))).Deserialize<ValueType>();
                }, true);

            var ClientTable = GetRelation(RLNTable[RLNKey]);
            ServerTable.ClearRelations = ClientTable.Parent.ClearRelations;
            ServerPartTable.Parent = ServerTable;

            return await Client.I_GetUpdate(ClientTable, MakeingUpdate, true);
        }
    }
}