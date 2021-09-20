using Monsajem_Incs.Net.Base.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAssembly.Browser.DOM;
using Monsajem_Incs.Views.Maker.Database;
using static Monsajem_Client.Network;
using Monsajem_Incs.Convertors;

namespace Monsajem_Incs.Database.Base
{
    public class TableFinder
    {
        private static HashSet<TableInfo> Tables = new HashSet<TableInfo>();

        public static void AddTable<ValueType, KeyType>(Table<ValueType, KeyType> Table)
            where KeyType : IComparable<KeyType>
        {
            var TableInfo = new TableInfo<KeyType, ValueType>() { TableName = Table.TableName, Table = Table };
            if (Tables.Contains(TableInfo) == true)
                throw new Exception($"Table with name '{Table.TableName}' is exist at TableFinder.");
            else
                Tables.Add(TableInfo);
        }
        public static TableInfo FindTable(string TableName)
        {
            var TableInfo = new TableInfo() { TableName = TableName };
            if (Tables.TryGetValue(TableInfo, out TableInfo) == false)
                throw new Exception($"Table with name '{TableName}' is not found at TableFinder.");
            else
                return TableInfo;
        }
        public static TableInfo<KeyType, ValueType> FindTable<ValueType, KeyType>(string TableName)
            where KeyType : IComparable<KeyType>
            => FindTable(TableName) as TableInfo<KeyType, ValueType>;

        public class TableInfo : IEquatable<TableInfo>
        {
            public string TableName;
            public object Table;

            private HashSet<RelationInfo> Relations = new HashSet<RelationInfo>();

            public void AddRelation<HolderKeyType, HolderValueType, RelationKeyType, RelationValueType>(
                Table<HolderValueType,HolderKeyType> Holder,
                Table<HolderValueType, HolderKeyType>.RelationTableInfo<RelationValueType,RelationKeyType> Relation)
                where HolderKeyType : IComparable<HolderKeyType>
                where RelationKeyType : IComparable<RelationKeyType>
            {
                var RelationInfo = new RelationInfo<HolderKeyType,HolderValueType, RelationKeyType, RelationValueType>()
                { RelationName = Relation.Field.Field.Name };
                if (Relations.Contains(RelationInfo) == true)
                    throw new Exception($"Relation with name '{Relation.Field.Field.Name}' is exist at '{TableName}' of TableFinder.");
                else
                    Relations.Add(RelationInfo);
            }
            public RelationInfo FindRelation(string RelationName)
            {
                var RelationInfo = new RelationInfo() { RelationName = RelationName };
                if (Relations.TryGetValue(RelationInfo, out RelationInfo) == false)
                    throw new Exception($"Relation with name '{RelationName}' is not found at '{TableName}' of TableFinder.");
                else
                    return RelationInfo;
            }

            public virtual string ConvertKeyToString(object Key)=>
                throw new Exception("ConvertKeyToString not implemented in table finder!");
            public virtual object ConvertStringToKey(string Key)=>
                throw new Exception("ConvertKeyToString not implemented in table finder!");
            public virtual Task Insert(object Data) =>
                throw new Exception("SyncInsert not implemented in table finder!");
            public virtual Task Update(object Key, object Data) =>
                throw new Exception("SyncUpdate not implemented in table finder!");
            public virtual Task Delete(object Key) =>
                throw new Exception("SyncDelete not implemented in table finder!");

            public virtual Task SendUpdate(IAsyncOprations Client) =>
                throw new Exception("SendUpdate not implemented in table finder!");
            public virtual Task GetUpdate(IAsyncOprations Client) =>
                throw new Exception("GetUpdate not implemented in table finder!");
            public virtual Task SyncUpdate() =>
                throw new Exception("SyncUpdate not implemented in table finder!");

            public virtual HTMLElement MakeShowView(
                   object Key,
                   Action<(TableInfo TableInfo, object Key)> OnUpdate = null,
                   Action<(TableInfo TableInfo, object Key)> OnDelete = null) =>
                throw new Exception("MakeShowView for item not implemented in table finder!");
            public virtual Task<HTMLElement> MakeShowView(
                    Action<(TableInfo TableInfo, object Key)> OnUpdate = null,
                    Action<(TableInfo TableInfo, object Key)> OnDelete = null) =>
                throw new Exception("MakeShowView for table not implemented in table finder!");

            public virtual HTMLElement MakeEditView(
                    object Key,
                    Action Done = null) =>
                throw new Exception("MakeShowView for item not implemented in table finder!");
            public virtual HTMLElement MakeInsertView(
                    Action Done = null) =>
                throw new Exception("MakeShowView for item not implemented in table finder!");

            public HTMLElement MakeShowView(
                    string Key,
                    Action<(TableInfo TableInfo, object Key)> OnUpdate = null,
                    Action<(TableInfo TableInfo, object Key)> OnDelete = null) =>
                MakeShowView(ConvertStringToKey(Key));

            public HTMLElement MakeEditView(
                    string Key,
                    Action Done = null) =>
                MakeEditView(ConvertStringToKey(Key),Done);

            public bool Equals(TableInfo other)
            {
                return TableName == other.TableName;
            }

            public override int GetHashCode()
            {
                return TableName.GetHashCode();
            }
        }

        public class TableInfo<KeyType, ValueType> : TableInfo
            where KeyType : IComparable<KeyType>
        {
            private new Table<ValueType, KeyType> Table
            { get => base.Table as Table<ValueType, KeyType>; }

            public override string ConvertKeyToString(object Key) =>
                ((KeyType)Key).ConvertToString();

            public override object ConvertStringToKey(string Key) =>
                Key.ConvertFromString<KeyType>();

            public override async Task Insert(object Value)
            {
                await Remote(
                async (Client) =>
                {
                    var Info = await Client.GetData<(string TableName, ValueType Value)>();
                    FindTable<ValueType, KeyType>(Info.TableName).Table.Insert(Info.Value);

                },
                async (Client) =>
                {
                    await Client.SendData((TableName, (ValueType)Value));
                });
            }

            public override async Task Update(object Key, object Value)
            {
                await Remote(
                async (Client) =>
                {
                    var Info = await Client.GetData<(string TableName,KeyType Key, ValueType Value)>();
                    var Table = FindTable<ValueType, KeyType>(Info.TableName).Table;
                    Table.Update(Info.Key,
                            (c)=> 
                            {
                                var Value = Info.Value;
                                Table.MoveRelations(c, Value);
                                return Value;
                            });
                },
                async (Client) =>
                {
                    await Client.SendData((TableName,(KeyType) Key, (ValueType)Value));
                });
            }

            public override async Task Delete(object Key)
            {
                await Remote(
                async (Client) =>
                {
                    var Info = await Client.GetData<(string TableName, KeyType Key)>();
                    FindTable<ValueType, KeyType>(Info.TableName).Table.Delete(Info.Key);
                },
                async (Client) =>
                {
                    await Client.SendData((TableName, (KeyType)Key));
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
                Action<(TableInfo TableInfo, object Key)> OnUpdate = null,
                Action<(TableInfo TableInfo, object Key)> OnDelete = null)
            {
                return await Table.MakeShowView(
                                    OnUpdate: (c) => OnUpdate?.Invoke(c),
                                    OnDelete: (c) => OnDelete?.Invoke(c));
            }

            public override HTMLElement MakeShowView(
                object Key,
                Action<(TableInfo TableInfo, object Key)> OnUpdate = null,
                Action<(TableInfo TableInfo, object Key)> OnDelete = null)
            {
                return Table.MakeShowView(
                                Key: (KeyType)Key,
                                OnUpdate:(c)=> OnUpdate?.Invoke((c.TableInfo,c.Key)),
                                OnDelete:(c) => OnDelete?.Invoke((c.TableInfo, c.Key)));
            }

            public override HTMLElement MakeEditView(
                object Key,
                Action Done = null)
            {
                return Table.MakeEditView(
                                Key: (KeyType)Key,
                                Done: Done);
            }

            public override HTMLElement MakeInsertView(Action Done = null)
            {
                return Table.MakeInsertView(Done: Done);
            }
        }

        public class RelationInfo : IEquatable<RelationInfo>
        {
            public string RelationName;

            public virtual string ConvertKeyToString(object Key) =>
                throw new Exception("ConvertKeyToString not implemented in table finder!");
            public virtual object ConvertStringToKey(string Key) =>
                throw new Exception("ConvertKeyToString not implemented in table finder!");
            public virtual string ConvertHolderKeyToString(object Key) =>
                throw new Exception("ConvertKeyToString not implemented in table finder!");
            public virtual object ConvertStringToHolderKey(string Key) =>
                throw new Exception("ConvertKeyToString not implemented in table finder!");

            public virtual object GetRelationTableByKey(object HolderKey) =>
                throw new Exception("Not impelemented.");

            public virtual Task Insert(object HolderKey , object Data) =>
                throw new Exception("SyncInsert not implemented in table finder!");
            public virtual Task Delete(object HolderKey, object Key) =>
                throw new Exception("SyncDelete not implemented in table finder!");
            public virtual Task Accept(object HolderKey, object Key) =>
                throw new Exception("SyncDelete not implemented in table finder!");
            public virtual Task Ignore(object HolderKey, object Key) =>
                throw new Exception("SyncDelete not implemented in table finder!");

            public virtual Task SendUpdate(object HolderKey, IAsyncOprations Client) =>
                throw new Exception("Not impelemented.");
            public virtual Task GetUpdate(object HolderKey, IAsyncOprations Client) =>
                throw new Exception("Not impelemented.");
            public virtual Task SyncUpdate(object HolderKey) =>
                throw new Exception("SyncUpdate not implemented in table finder!");

            public virtual HTMLElement MakeShowView(
                   object HolderKey,
                   object Key,
                   Action<(TableInfo TableInfo, object Key)> OnUpdate = null,
                   Action<(TableInfo TableInfo, object Key)> OnDelete = null) =>
                throw new Exception("MakeShowView for item not implemented in table finder!");

            public virtual Task<HTMLElement> MakeShowView(
                object HolderKey,
                Action<(TableInfo TableInfo, object Key)> OnUpdate = null,
                Action<(TableInfo TableInfo, object Key)> OnDelete = null) =>
                throw new Exception("MakeShowView for table not implemented in table finder!");

            public virtual HTMLElement MakeInsertView(
                object HolderKey,
                Action Done = null) =>
                    throw new Exception(nameof(MakeInsertView) + " for item not implemented in table finder!");

            public HTMLElement MakeShowView(
                string HolderKey,
                string Key,
                Action<(TableInfo TableInfo, object Key)> OnUpdate = null,
                Action<(TableInfo TableInfo, object Key)> OnDelete = null) =>
                    MakeShowView(ConvertStringToHolderKey(HolderKey),
                                 ConvertStringToKey(Key),OnUpdate,OnDelete);

            public Task<HTMLElement> MakeShowView(
                string HolderKey,
                Action<(TableInfo TableInfo, object Key)> OnUpdate = null,
                Action<(TableInfo TableInfo, object Key)> OnDelete = null) =>
                     MakeShowView(ConvertStringToHolderKey(HolderKey),
                     OnUpdate, OnDelete);

            public virtual HTMLElement MakeInsertView(
                string HolderKey,
                Action Done = null) =>
                    MakeInsertView(ConvertStringToHolderKey(HolderKey),Done);

            public bool Equals(RelationInfo other)
            {
                return RelationName == other.RelationName;
            }

            public override int GetHashCode()
            {
                return RelationName.GetHashCode();
            }
        }

        public class RelationInfo<HolderKeyType,HolderValueType, RelationKeyType, RelationValueType> : RelationInfo
            where HolderKeyType : IComparable<HolderKeyType>
            where RelationKeyType : IComparable<RelationKeyType>
        {
            Table<HolderValueType, HolderKeyType> Holder;
            Table<HolderValueType, HolderKeyType>.RelationTableInfo<RelationValueType, RelationKeyType> Relation;

            public PartOfTable<RelationValueType, RelationKeyType>
                GetterRealtionByKey(HolderKeyType HolderKey) => Relation.Field.Value(Holder[HolderKey]);

            public override string ConvertKeyToString(object Key) => ((RelationKeyType)Key).ConvertToString();
            public override object ConvertStringToKey(string Key) => Key.ConvertFromString<RelationKeyType>();
            public override string ConvertHolderKeyToString(object Key) => ((HolderKeyType)Key).ConvertToString();
            public override object ConvertStringToHolderKey(string Key) => Key.ConvertFromString<HolderKeyType>();
            public override async Task Insert(object HolderKey,object Value)
            {
                await Remote(
                async (Client) =>
                {
                    var Info = await Client.GetData<(string HolderName, string RelationName, HolderKeyType HolderKey, RelationValueType Value)>();
                    var RelationInfo = FindTable(Info.HolderName).FindRelation(Info.RelationName) 
                                        as RelationInfo<HolderKeyType, HolderValueType, RelationKeyType, RelationValueType>;
                    RelationInfo.GetterRealtionByKey(Info.HolderKey).Insert(Info.Value);
                },
                async (Client) =>
                {
                    await Client.SendData((Holder.TableName,RelationName,(HolderKeyType)HolderKey,(RelationValueType) Value));
                });
            }

            public override async Task Delete(object HolderKey, object Key)
            {
                var Table = GetterRealtionByKey((HolderKeyType)HolderKey);
                await FindTable(Table.Parent.TableName).Delete(Key);
            }

            public override async Task Accept(object HolderKey, object Key)
            {
                await Remote(
                async (Client) =>
                {
                    var Info = await Client.GetData<(string HolderName, string RelationName, HolderKeyType HolderKey, RelationKeyType Key)>();
                    var RelationInfo = FindTable(Info.HolderName).FindRelation(Info.RelationName)
                                        as RelationInfo<HolderKeyType, HolderValueType, RelationKeyType, RelationValueType>;
                    RelationInfo.GetterRealtionByKey(Info.HolderKey).Accept(Info.Key);
                },
                async (Client) =>
                {
                    await Client.SendData((Holder.TableName, RelationName, (HolderKeyType)HolderKey, (RelationValueType)Key));
                });
            }

            public override async Task Ignore(object HolderKey, object Key)
            {
                await Remote(
                async (Client) =>
                {
                    var Info = await Client.GetData<(string HolderName, string RelationName, HolderKeyType HolderKey, RelationKeyType Key)>();
                    var RelationInfo = FindTable(Info.HolderName).FindRelation(Info.RelationName)
                                        as RelationInfo<HolderKeyType, HolderValueType, RelationKeyType, RelationValueType>;
                    RelationInfo.GetterRealtionByKey(Info.HolderKey).Ignore(Info.Key);
                },
                async (Client) =>
                {
                    await Client.SendData((Holder.TableName, RelationName, (HolderKeyType)HolderKey, (RelationValueType)Key));
                });
            }

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
                Action<(TableInfo TableInfo, object Key)> OnUpdate = null,
                Action<(TableInfo TableInfo, object Key)> OnDelete = null)
            {
                var Table = GetterRealtionByKey((HolderKeyType)HolderKey);
                return await Table.MakeShowView(
                               OnUpdate: (c) => OnUpdate?.Invoke(c),
                               OnDelete: (c) => OnDelete?.Invoke(c));
            }

            public override HTMLElement MakeShowView(
                object HolderKey,
                object Key,
                Action<(TableInfo TableInfo, object Key)> OnUpdate = null,
                Action<(TableInfo TableInfo, object Key)> OnDelete = null)
            {
                var Table = GetterRealtionByKey((HolderKeyType)HolderKey);
                return Table.MakeShowView(
                                Key: (RelationKeyType)Key,
                                OnUpdate:(c)=> OnUpdate?.Invoke((c.TableInfo,c.Key)),
                                OnDelete:(c) => OnDelete?.Invoke((c.TableInfo, c.Key)));
            }

            public override HTMLElement MakeInsertView(object HolderKey, Action Done = null)
            {
                var Table = GetterRealtionByKey((HolderKeyType)HolderKey);
                return Table.MakeInsertView(Done);
            }

            public override object GetRelationTableByKey(object HolderKey)
            {
                return GetterRealtionByKey((HolderKeyType)HolderKey);
            }
        }
    }
}
