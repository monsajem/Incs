using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Monsajem_Incs.Net.Base.Service;
using Monsajem_Incs.Database;
using System.Linq;
using Monsajem_Incs.Array.Hyper;
using System.Linq.Expressions;
using Monsajem_Incs.Serialization;

namespace Monsajem_Incs.Database.Base
{
    public static partial class Extentions
    {
        private static async Task<bool> GetUpdate<ValueType, KeyType>(
            Func<string, Task<byte[]>> CDN,
            Table<ValueType, KeyType> Table,
            Action<ValueType> MakeingUpdate,
            Action<string> Deleted)
            where KeyType : IComparable<KeyType>
        {

            if (typeof(PartOfTable<ValueType, KeyType>).IsAssignableFrom(Table.GetType()))
                throw new Exception("Type of Main Table is (part of table) but expected orginal Table.");
            if (Table.TableName == null)
                throw new Exception("Table Name not found!");
            var OldCDN = CDN;
            CDN =async (c)=> await OldCDN($"/{Table.TableName}/{c}");
            var Socket = new Net.Virtual.Socket();
            var Server = new Net.Virtual.AsyncOprations(Socket);
            var Client = new Net.Virtual.AsyncOprations(Socket.OtherSide);

            var ServerTable = (await CDN("K")).Deserialize<KeyValue.Base.Table<ValueType, KeyType>>();
            ServerTable.ClearRelations = Table.ClearRelations;

            if (ServerTable.UpdateAble == null)
                throw new Exception("UpdateAble at Server Not Found!");

            var ServerTask = 
                Server.I_SendUpdate(ServerTable, ServerTable.UpdateAble.UpdateCodes,
                async (key) =>
                {
                    return (await CDN("V/" +
                        Convert.ToBase64String(key.Serialize()))).Deserialize<ValueType>();
                }, false);

            var ClientTask = Client.I_GetUpdate(Table, MakeingUpdate,
                (key) =>Deleted?.Invoke("V/"+Convert.ToBase64String(key.Serialize())), false);

             await Threading.Task_EX.CheckAll(ServerTask, ClientTask);

            return await ClientTask;
        }

        private static Task<bool> GetUpdate<ValueType_RLN, KeyType_RLN, ValueType, KeyType>(
            Func<string, Task<byte[]>> CDN,
            Table<ValueType_RLN, KeyType_RLN> RLNTable,
            KeyType_RLN RLNKey,
            Func<ValueType_RLN, PartOfTable<ValueType, KeyType>> GetRelation,
            Action<ValueType> MakeingUpdate,
            Action<string> Deleted)
            where KeyType : IComparable<KeyType>
            where KeyType_RLN : IComparable<KeyType_RLN>
        {
            var PartTable = GetRelation(RLNTable[RLNKey]);
            Func<string, Task<byte[]>> RootCDN =
                async (c)=>await CDN($"/{PartTable.Parent.TableName}{c}");
            Func<string, Task<byte[]>> RelationCDN =
                async (c)=>await CDN($"/{RLNTable.TableName}{c}");
            return GetUpdate(RootCDN, RelationCDN, RLNTable, RLNKey, GetRelation, MakeingUpdate,Deleted);
        }

        private static async Task<bool> GetUpdate<ValueType_RLN, KeyType_RLN, ValueType, KeyType>(
            Func<string, Task<byte[]>> RootCDN,
            Func<string, Task<byte[]>> RelationCDN,
            Table<ValueType_RLN, KeyType_RLN> RLNTable,
            KeyType_RLN RLNKey,
            Func<ValueType_RLN, PartOfTable<ValueType, KeyType>> GetRelation,
            Action<ValueType> MakeingUpdate,
            Action<string> Deleted)
            where KeyType : IComparable<KeyType>
            where KeyType_RLN : IComparable<KeyType_RLN>
        {
            if (typeof(PartOfTable<ValueType, KeyType>).IsAssignableFrom(RLNTable.GetType()))
                throw new Exception("Type of Main Table is (part of table) but expected orginal Table.");
            if (GetRelation == null)
                throw new Exception("Get Update in part of table need to known relation.");

            var Socket = new Net.Virtual.Socket();
            var Server = new Net.Virtual.AsyncOprations(Socket);
            var Client = new Net.Virtual.AsyncOprations(Socket.OtherSide);

            var ServerTable = (await RootCDN("/K")).Deserialize<KeyValue.Base.Table<ValueType, KeyType>>();
            var ServerPartTable =
                GetRelation((await RelationCDN("/V/" + Convert.ToBase64String(RLNKey.Serialize()))).Deserialize<ValueType_RLN>());

            if (ServerPartTable == null)
                return false;
            ValueType_RLN RLNValue;
            try
            {
                RLNValue = RLNTable[RLNKey].Value;
            }
            catch (Exception ex)
            {
                throw new Exception("Get Value of the key have exception when update.", ex);
            }

            var ClientTable = GetRelation(RLNValue);
            ServerTable.ClearRelations = ClientTable.Parent.ClearRelations;
            ServerPartTable.Parent = ServerTable;

            var ServerTask = Server.I_SendUpdate(ServerPartTable, ServerTable.UpdateAble.UpdateCodes,
                async (key) =>
                {
                    return (await RootCDN("/V/" +
                        Convert.ToBase64String(key.Serialize()))).Deserialize<ValueType>();
                }, true);

            var ClientTask = Client.I_GetUpdate(ClientTable, MakeingUpdate,
                (key)=>Deleted?.Invoke("/V/" +Convert.ToBase64String(key.Serialize())), true);

            await Threading.Task_EX.CheckAll(ServerTask, ClientTask);

            return await ClientTask;

        }
    }
}