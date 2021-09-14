using Monsajem_Incs.Net.Base.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAssembly.Browser.DOM;
using Monsajem_Incs.Views.Maker.Database;
using static Monsajem_Client.Network;

namespace Monsajem_Incs.Database.Base
{
    internal class TableFinder
    {
        private static HashSet<TableInfo> Tables = new HashSet<TableInfo>();

        public static void AddTable<ValueType,KeyType>(Table<ValueType,KeyType> Table)
            where KeyType:IComparable<KeyType>
        {
            var TableInfo = new TableInfo<KeyType,ValueType>() {TableName = Table.TableName,Table = Table};
            if (Tables.Contains(TableInfo) == true)
                throw new Exception($"Table with name '{Table.TableName}' is exist at TableFinder.");
            else
                Tables.Add(TableInfo);
        }
        public static TableInfo FindTable(string TableName)
        {
            var TableInfo = new TableInfo() { TableName = TableName };
            if (Tables.TryGetValue(TableInfo,out TableInfo) == false)
                throw new Exception($"Table with name '{TableName}' is not found at TableFinder.");
            else
                return TableInfo;
        }

        public class TableInfo:IEquatable<TableInfo>
        {
            public string TableName;
            public object Table;

            private HashSet<RelationInfo> Relations = new HashSet<RelationInfo>();

            public RelationInfo<HolderKeyType, RelationKeyType, RelationValueType>
                AddRelation<HolderKeyType, RelationKeyType, RelationValueType>(
                string RelationName)
                where HolderKeyType:IComparable<HolderKeyType>
                where RelationKeyType : IComparable<RelationKeyType>
            {
                var RelationInfo = new RelationInfo<HolderKeyType, RelationKeyType, RelationValueType>() 
                                        { RelationName = RelationName };
                if (Relations.Contains(RelationInfo) == true)
                    throw new Exception($"Relation with name '{RelationName}' is exist at '{TableName}' of TableFinder.");
                else
                    Relations.Add(RelationInfo);
                return RelationInfo;
            }
            public RelationInfo FindRelation(string RelationName)
            {
                var RelationInfo = new RelationInfo() { RelationName = RelationName };
                if (Relations.TryGetValue(RelationInfo, out RelationInfo) == false)
                    throw new Exception($"Relation with name '{RelationName}' is not found at '{TableName}' of TableFinder.");
                else
                    return RelationInfo;
            }

            public virtual Task Insert(object Data)=>
                throw new Exception("SyncInsert not implemented in table finder!");
            public virtual Task Update(object Key, object Data)=>
                throw new Exception("SyncUpdate not implemented in table finder!");
            public virtual Task Delete(object Key)=>
                throw new Exception("SyncDelete not implemented in table finder!");


            public virtual Task SendUpdate(IAsyncOprations Client)=>
                throw new Exception("SendUpdate not implemented in table finder!");
            public virtual Task GetUpdate(IAsyncOprations Client)=>
                throw new Exception("GetUpdate not implemented in table finder!");
            public virtual Task SyncUpdate()=>
                throw new Exception("SyncUpdate not implemented in table finder!");

            public virtual HTMLElement MakeShowView(
                   object Key,
                   Action OnUpdate = null,
                   Action OnDelete = null)=>
                throw new Exception("MakeShowView for item not implemented in table finder!");
            public virtual Task<HTMLElement> MakeShowView(
                    Action<object> OnUpdate = null,
                    Action<object> OnDelete = null)=>
                throw new Exception("MakeShowView for table not implemented in table finder!");

            public bool Equals(TableInfo other)
            {
                return TableName == other.TableName;
            }

            public override int GetHashCode()
            {
                return TableName.GetHashCode();
            }
        }

        public class TableInfo<KeyType,ValueType>:TableInfo
            where KeyType:IComparable<KeyType>
        {
            private new Table<ValueType, KeyType> Table 
                        { get => base.Table as Table<ValueType, KeyType>; }

            public override async Task Insert(object Data)
            {
                await Remote(
                async (Client) =>
                {
                    await Client.GetData<(string TableName, ValueType Data)>();

                },
                async (Client)=>
                { 
                });
            }

            public override async Task SendUpdate(IAsyncOprations Client)
            {
                await Client.SendUpdate(Table);
            }
            public override async Task GetUpdate(IAsyncOprations Client)
            {
                await Client.GetUpdate(Table);
            }

            public async override Task SyncUpdate()
            {
                await Table.SyncUpdate();
            }

            public async override Task<HTMLElement> MakeShowView(
                Action<object> OnUpdate = null, 
                Action<object> OnDelete = null)
            {
                return await Table.MakeShowView(
                                    OnUpdate:(c)=>OnUpdate?.Invoke(c),
                                    OnDelete:(c)=> OnDelete?.Invoke(c));
            }

            public override HTMLElement MakeShowView(
                object Key, 
                Action OnUpdate = null, 
                Action OnDelete = null)
            {
                return Table.MakeShowView(
                                Key:(KeyType)Key,
                                OnUpdate:OnUpdate,
                                OnDelete:OnDelete);
            }
        }

        public class RelationInfo : IEquatable<RelationInfo>
        {
            public string RelationName;

            public virtual object GetRelationTableByKey(object HolderKey)=>
                throw new Exception("Not impelemented.");


            public virtual Task Insert(object Data) =>
                throw new Exception("SyncInsert not implemented in table finder!");
            public virtual Task Update(object Key, object Data) =>
                throw new Exception("SyncUpdate not implemented in table finder!");
            public virtual Task Delete(object Key) =>
                throw new Exception("SyncDelete not implemented in table finder!");
            public virtual Task Accept(object Key) =>
                throw new Exception("SyncDelete not implemented in table finder!");
            public virtual Task Ignore(object Key) =>
                throw new Exception("SyncDelete not implemented in table finder!");

            public virtual Task SendUpdate(object HolderKey, IAsyncOprations Client) =>
                throw new Exception("Not impelemented.");
            public virtual Task GetUpdate(object HolderKey, IAsyncOprations Client) =>
                throw new Exception("Not impelemented.");
            public virtual Task SyncUpdate(object HolderKey)=>
                throw new Exception("SyncUpdate not implemented in table finder!");

            public virtual HTMLElement MakeShowView(
                   object HolderKey,
                   object Key,
                   Action OnUpdate = null,
                   Action OnDelete = null) =>
                throw new Exception("MakeShowView for item not implemented in table finder!");

            public virtual Task<HTMLElement> MakeShowView(
                object HolderKey,
                Action<object> OnUpdate = null,
                Action<object> OnDelete = null) =>
                throw new Exception("MakeShowView for table not implemented in table finder!");

            public bool Equals(RelationInfo other)
            {
                return RelationName == other.RelationName;
            }

            public override int GetHashCode()
            {
                return RelationName.GetHashCode();
            }
        }

        public class RelationInfo<HolderKeyType,RelationKeyType,RelationValueType> : RelationInfo
            where HolderKeyType : IComparable<HolderKeyType>
            where RelationKeyType : IComparable<RelationKeyType>
        {
            public Func<HolderKeyType, PartOfTable<RelationValueType, RelationKeyType>>
                    GetterRealtionByKey = (c) => throw new Exception("Not impelemented.");

            public override async Task SendUpdate(object HolderKey, IAsyncOprations Client)
            {
                var Table = GetterRealtionByKey((HolderKeyType)HolderKey);
                await Client.SendUpdate(Table);
            }

            public override async Task GetUpdate(object HolderKey, IAsyncOprations Client)
            {
                var Table = GetterRealtionByKey((HolderKeyType)HolderKey);
                await Client.GetUpdate(Table);
            }

            public override async Task SyncUpdate(object HolderKey)
            {
                var Table = GetterRealtionByKey((HolderKeyType)HolderKey);
                await Table.SyncUpdate();
            }

            public override async Task<HTMLElement> MakeShowView(
                object HolderKey,
                Action<object> OnUpdate = null, 
                Action<object> OnDelete = null)
            {
                var Table = GetterRealtionByKey((HolderKeyType)HolderKey);
                return await Table.MakeShowView(
                               OnUpdate:(c)=>OnUpdate?.Invoke(c),
                               OnDelete:(c)=>OnDelete?.Invoke(c));
            }

            public override HTMLElement MakeShowView(
                object HolderKey,
                object Key, 
                Action OnUpdate = null,
                Action OnDelete = null)
            {
                var Table = GetterRealtionByKey((HolderKeyType)HolderKey);
                return Table.MakeShowView(
                                Key:(RelationKeyType)Key,
                                OnUpdate:OnUpdate,
                                OnDelete:OnDelete);
            }

            public override object GetRelationTableByKey(object HolderKey)
            {
                return GetterRealtionByKey((HolderKeyType)HolderKey);
            }
        }
    }
}
