using Monsajem_Incs.Convertors;
using Monsajem_Incs.Net.Base.Service;
using Monsajem_Incs.Views.Maker.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAssembly.Browser.DOM;
using static Monsajem_Incs.WasmClient.Network;

namespace Monsajem_Incs.Database.Base
{
    public class TableFinder
    {
        private static HashSet<TableInfo> Tables = [];

        public static void AddTable<ValueType, KeyType>(Table<ValueType, KeyType> Table)
            where KeyType : IComparable<KeyType>
        {
            var TableInfo = new TableInfo<KeyType, ValueType>() { TableName = Table.TableName, Table = Table };
            _ = Tables.Contains(TableInfo) == true
                ? throw new Exception($"Table with name '{Table.TableName}' is exist at TableFinder.")
                : Tables.Add(TableInfo);
        }
        public static TableInfo FindTable(string TableName)
        {
            var TableInfo = new TableInfo() { TableName = TableName };
            return Tables.TryGetValue(TableInfo, out TableInfo) == false
                ? throw new Exception($"Table with name '{TableName}' is not found at TableFinder.")
                : TableInfo;
        }
        public static TableInfo<KeyType, ValueType> FindTable<ValueType, KeyType>(string TableName)
            where KeyType : IComparable<KeyType>
            => FindTable(TableName) as TableInfo<KeyType, ValueType>;

        public static TableInfo<KeyType, ValueType> FindTable<ValueType, KeyType>(
            Table<ValueType, KeyType> SampleType, string TableName)
            where KeyType : IComparable<KeyType>
            => FindTable(TableName) as TableInfo<KeyType, ValueType>;

        public class TableInfo : IEquatable<TableInfo>
        {
            public string TableName;
            public object Table;

            private HashSet<RelationInfo> Relations = [];

            public Func<object, object> SelectorItems { get; protected set; }

            public void AddRelation<HolderKeyType, HolderValueType, RelationKeyType, RelationValueType>(
                Table<HolderValueType, HolderKeyType> Holder,
                Table<HolderValueType, HolderKeyType>.RelationTableInfo<RelationValueType, RelationKeyType> Relation)
                where HolderKeyType : IComparable<HolderKeyType>
                where RelationKeyType : IComparable<RelationKeyType>
            {
                var RelationInfo = new RelationInfo<HolderKeyType, HolderValueType, RelationKeyType, RelationValueType>()
                {
                    RelationName = Relation.Field.Field.Name,
                    Holder = Holder,
                    Relation = Relation
                };
                _ = Relations.Contains(RelationInfo) == true
                    ? throw new Exception($"Relation with name '{Relation.Field.Field.Name}' is exist at '{TableName}' of TableFinder.")
                    : Relations.Add(RelationInfo);
            }
            public RelationInfo FindRelation(string RelationName)
            {
                var RelationInfo = new RelationInfo() { RelationName = RelationName };
                return Relations.TryGetValue(RelationInfo, out RelationInfo) == false
                    ? throw new Exception($"Relation with name '{RelationName}' is not found at '{TableName}' of TableFinder.")
                    : RelationInfo;
            }

            public virtual string ConvertKeyToString(object Key) =>
                throw new Exception("ConvertKeyToString not implemented in table finder!");
            public virtual object ConvertStringToKey(string Key) =>
                throw new Exception("ConvertKeyToString not implemented in table finder!");
            public virtual void Insert(object Data) =>
                throw new Exception("SyncInsert not implemented in table finder!");
            public virtual void Update(object Key, object Data) =>
                throw new Exception("SyncUpdate not implemented in table finder!");
            public virtual void Delete(object Key) =>
                throw new Exception("SyncDelete not implemented in table finder!");

            public virtual Task SendUpdate(IAsyncOprations Client) =>
                throw new Exception("SendUpdate not implemented in table finder!");
            public virtual Task GetUpdate(IAsyncOprations Client) =>
                throw new Exception("GetUpdate not implemented in table finder!");
            public virtual Task SyncUpdate() =>
                throw new Exception("SyncUpdate not implemented in table finder!");

            public virtual HTMLElement MakeShowViewForItem(
                   object Key,
                   Action<(TableInfo TableInfo, object Key)> OnUpdate = null,
                   Action<(TableInfo TableInfo, object Key)> OnDelete = null) =>
                throw new Exception("MakeShowView for item not implemented in table finder!");
            public virtual HTMLElement MakeShowViewForItems(
                    string Query = null,
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

            public HTMLElement MakeShowViewForItem(
                    string Key,
                    Action<(TableInfo TableInfo, object Key)> OnUpdate = null,
                    Action<(TableInfo TableInfo, object Key)> OnDelete = null) =>
                MakeShowViewForItem(ConvertStringToKey(Key));

            public HTMLElement MakeEditView(
                    string Key,
                    Action Done = null) =>
                MakeEditView(ConvertStringToKey(Key), Done);

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
            public TableInfo()
            {
                SelectorItems = (c) => c.Values;
            }

            private new Table<ValueType, KeyType> Table
            { get => (Table<ValueType, KeyType>)base.Table; }

            public new Func<(IEnumerable<ValueType> Values, string Query), IEnumerable<ValueType>>
                SelectorItems
            {

                get => (c) => (IEnumerable<ValueType>)base.SelectorItems(c);
                set => base.SelectorItems = (c) => value(((IEnumerable<ValueType> Values, string Query))c);
            }

            public override string ConvertKeyToString(object Key) =>
                ((KeyType)Key).ConvertToString();

            public override object ConvertStringToKey(string Key) =>
                Key.ConvertFromString<KeyType>();

            public override void Insert(object Value) =>
                Table.Insert((ValueType)Value);


            public override void Update(object Key, object Value) =>
                Table.Update((KeyType)Key,
                            (c) =>
                            {
                                var NewValue = (ValueType)Value;
                                Table.MoveRelations?.Invoke(c, NewValue);
                                return NewValue;
                            });

            public override void Delete(object Key) =>
                Table.Delete((KeyType)Key);

            public override async Task SendUpdate(IAsyncOprations Client)
            {
                await Client.SendUpdate(Table);
            }
            public override async Task GetUpdate(IAsyncOprations Client)
            {
                _ = await Client.GetUpdate(Table);
            }

            public async override Task SyncUpdate()
            {
                await Remote(
                            async (c) =>
                            {
                                var TableName = await c.GetData<string>();
                                await TableFinder.FindTable(TableName).SendUpdate(c);
                            },
                            async (c) =>
                            {
                                _ = await c.SendData(Table.TableName);
                                _ = await c.GetUpdate(Table);
                            });
            }

            public override HTMLElement MakeShowViewForItems(
                string Query,
                Action<(TableInfo TableInfo, object Key)> OnUpdate = null,
                Action<(TableInfo TableInfo, object Key)> OnDelete = null)
            {
                return Table.MakeShowView(
                                    Query: Query,
                                    OnUpdate: (c) => OnUpdate?.Invoke(c),
                                    OnDelete: (c) => OnDelete?.Invoke(c));
            }

            public override HTMLElement MakeShowViewForItem(
                object Key,
                Action<(TableInfo TableInfo, object Key)> OnUpdate = null,
                Action<(TableInfo TableInfo, object Key)> OnDelete = null)
            {
                return Table.MakeShowView(
                                Key: (KeyType)Key,
                                OnUpdate: (c) => OnUpdate?.Invoke((c.TableInfo, c.Key)),
                                OnDelete: (c) => OnDelete?.Invoke((c.TableInfo, c.Key)));
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

            public virtual void Insert(object HolderKey, object Data) =>
                throw new Exception("SyncInsert not implemented in table finder!");
            public virtual void Delete(object HolderKey, object Key) =>
                throw new Exception("SyncDelete not implemented in table finder!");
            public virtual void Accept(object HolderKey, object Key) =>
                throw new Exception("SyncDelete not implemented in table finder!");
            public virtual void Ignore(object HolderKey, object Key) =>
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

            public virtual HTMLElement MakeShowViewForItems(
                object HolderKey,
                string Query,
                Action<(TableInfo TableInfo, object Key)> OnUpdate = null,
                Action<(TableInfo TableInfo, object Key)> OnDelete = null) =>
                throw new Exception("MakeShowView for table not implemented in table finder!");

            public virtual HTMLElement MakeInsertView(
                object HolderKey,
                Action Done = null) =>
                    throw new Exception(nameof(MakeInsertView) + " for item not implemented in table finder!");

            public HTMLElement MakeShowViewForItem(
                string HolderKey,
                string Key,
                Action<(TableInfo TableInfo, object Key)> OnUpdate = null,
                Action<(TableInfo TableInfo, object Key)> OnDelete = null) =>
                    MakeShowView(ConvertStringToHolderKey(HolderKey),
                                 ConvertStringToKey(Key), OnUpdate, OnDelete);

            public HTMLElement MakeShowViewForItems(
                string HolderKey,
                string Query = null,
                Action<(TableInfo TableInfo, object Key)> OnUpdate = null,
                Action<(TableInfo TableInfo, object Key)> OnDelete = null) =>
                     MakeShowViewForItems(ConvertStringToHolderKey(HolderKey),
                     Query, OnUpdate, OnDelete);

            public virtual HTMLElement MakeInsertView(
                string HolderKey,
                Action Done = null) =>
                    MakeInsertView(ConvertStringToHolderKey(HolderKey), Done);

            public bool Equals(RelationInfo other)
            {
                return RelationName == other.RelationName;
            }

            public override int GetHashCode()
            {
                return RelationName.GetHashCode();
            }
        }

        public class RelationInfo<HolderKeyType, HolderValueType, RelationKeyType, RelationValueType> : RelationInfo
            where HolderKeyType : IComparable<HolderKeyType>
            where RelationKeyType : IComparable<RelationKeyType>
        {
            public Table<HolderValueType, HolderKeyType> Holder;
            public Table<HolderValueType, HolderKeyType>.RelationTableInfo<RelationValueType, RelationKeyType> Relation;

            public PartOfTable<RelationValueType, RelationKeyType>
                GetterRealtionByKey(HolderKeyType HolderKey) => Relation.Field.Value(Holder[HolderKey]);

            public override string ConvertKeyToString(object Key) => ((RelationKeyType)Key).ConvertToString();
            public override object ConvertStringToKey(string Key) => Key.ConvertFromString<RelationKeyType>();
            public override string ConvertHolderKeyToString(object Key) => ((HolderKeyType)Key).ConvertToString();
            public override object ConvertStringToHolderKey(string Key) => Key.ConvertFromString<HolderKeyType>();
            public override void Insert(object HolderKey, object Value) =>
                GetterRealtionByKey((HolderKeyType)HolderKey).Insert((RelationValueType)Value);

            public override void Delete(object HolderKey, object Key) =>
                GetterRealtionByKey((HolderKeyType)HolderKey).Delete((RelationKeyType)Key);

            public override void Accept(object HolderKey, object Key) =>
                GetterRealtionByKey((HolderKeyType)HolderKey).Accept((RelationKeyType)Key);

            public override void Ignore(object HolderKey, object Key) =>
                GetterRealtionByKey((HolderKeyType)HolderKey).Ignore((RelationKeyType)Key);

            public override async Task SendUpdate(object HolderKey, IAsyncOprations Client)
            {
                var Table = GetterRealtionByKey((HolderKeyType)HolderKey);
                await Client.SendUpdate(Table);
            }

            public override async Task GetUpdate(object HolderKey, IAsyncOprations Client)
            {
                var Table = GetterRealtionByKey((HolderKeyType)HolderKey);
                _ = await Client.GetUpdate(Table);
            }

            public override async Task SyncUpdate(object HolderKey)
            {
                var PartTable = GetterRealtionByKey((HolderKeyType)HolderKey); ;
                await Remote(
                        async (c) =>
                        {
                            var Info = await c.GetData<(string TableName, HolderKeyType Key, string RealtionName)>();
                            await TableFinder.FindTable(Info.TableName)
                                             .FindRelation(Info.RealtionName)
                                             .SendUpdate(Info.Key, c);
                        },
                        async (c) =>
                        {
                            var HolderName = Holder.TableName;
                            var HolderKey = (HolderKeyType)PartTable.HolderTable.Key;
                            _ = await c.SendData((Holder.TableName,
                                              HolderKey,
                                              PartTable.TableName));
                            _ = await c.GetUpdate(PartTable);
                        });
            }

            public override HTMLElement MakeShowViewForItems(
                object HolderKey,
                string Query = null,
                Action<(TableInfo TableInfo, object Key)> OnUpdate = null,
                Action<(TableInfo TableInfo, object Key)> OnDelete = null)
            {
                var Table = GetterRealtionByKey((HolderKeyType)HolderKey);
                return Table.MakeShowView(
                               Query: Query,
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
                                OnUpdate: (c) => OnUpdate?.Invoke((c.TableInfo, c.Key)),
                                OnDelete: (c) => OnDelete?.Invoke((c.TableInfo, c.Key)));
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
