﻿using Monsajem_Incs.Collection.Array.TreeBased;
using System;

namespace Monsajem_Incs.Database.Base
{
    public partial class PartOfTable<ValueType, KeyType> :
        Table<ValueType, KeyType>
        where KeyType : IComparable<KeyType>
    {

        [Serialization.NonSerialized]
        public TableExtras Extras;

        [Serialization.NonSerialized]
        public Table<ValueType, KeyType> Parent;

        [Serialization.NonSerialized]
        internal Action SaveToParent;

        [Serialization.NonSerialized]
        public (dynamic Table, object Key) HolderTable;

        public PartOfTable(KeyType[] NewKEys, Table<ValueType, KeyType> Parent)
        {
            this.Parent = Parent;

            base.BasicActions = new BasicActions<ValueType>();
            base.Events = new Events<ValueType>();
            base.SecurityEvents = new SecurityEvents<ValueType>();

            base.BasicActions.Items = new Collection.Array.Base.DynamicArray<(ValueType, ulong)>()
            {
                _GetItem = (pos) =>
                {
                    var Value = Parent.GetItem(KeysInfo.Keys[pos]);
                    return (Value.Value, Value.UpdateCode);
                }
            };
            base.BasicActions.Keys = Parent.BasicActions.Keys;

            base.Events.Inserting += (inf) => Parent.Events.Inserting?.Invoke(inf);
            base.SecurityEvents.Inserting += (inf) => Parent.SecurityEvents.Inserting?.Invoke(inf);
            base.Events.Inserted += (inf) => Parent.Events.Inserted?.Invoke(inf);

            base.SecurityEvents.Deleting += (inf) => Parent.SecurityEvents.Deleting?.Invoke(inf);
            base.Events.Deleting += (inf) => Parent.Events.Deleting?.Invoke(inf);
            base.Events.Deleted += (inf) => Parent.Events.Deleted?.Invoke(inf);

            base.SecurityEvents.MakeKeys += (inf) => Parent.SecurityEvents.MakeKeys?.Invoke(inf);
            base.SecurityEvents.Updating += (inf) => Parent.SecurityEvents.Updating?.Invoke(inf);
            base.Events.Updating += (inf) => Parent.Events.Updating?.Invoke(inf);
            base.Events.Updated += (inf) =>
            {
                var OldKey = (KeyType)inf.Info[KeyPos].OldKey;
                var NewKey = (KeyType)inf.Info[KeyPos].Key;
                if (OldKey.CompareTo(NewKey) != 0)
                {
                    _ = KeysInfo.Keys.BinaryDelete(OldKey);
                    _ = KeysInfo.Keys.BinaryInsert(NewKey);
                }
                Parent.Events.Updated?.Invoke(inf);
            };

            base.Events.loading = Parent.Events.loading;
            base.Events.Saving = Parent.Events.Saving;

            base.GetKey = Parent.GetKey;


            base.KeysInfo.Keys = new Array<KeyType>(NewKEys);

            base.Events.Inserted += (inf) =>
            {
                _ = Accept(GetKey(inf.Value));
            };

            base.Events.Deleted += (info) =>
            {
                _ = Ignore(GetKey(info.Value));
            };

            Extras = new TableExtras();

            Extras.Accepting += (Key) =>
            {
                if (KeysInfo.Keys.BinarySearch(Key.Key).Index > -1)
                    throw new InvalidOperationException("Value be exist!");
                if (Parent.IsExist(Key.Key) == false)
                    throw new InvalidOperationException("Value not exist at Parent!");
            };

            UpdateAbleChanged += (_UpdateAble) =>
            {
                if (_UpdateAble != null)
                    ReadyForUpdateAble();
            };

            IUpdateOrInsert = (OldKey, NewCreator) =>
            {
                if (PositionOf(OldKey) < 0)
                {
                    var Result = Parent.IUpdateOrInsert(OldKey, NewCreator);
                    Accept(Result);
                    return Result;
                }
                else
                    return Parent.IUpdateOrInsert(OldKey, NewCreator);
            };
        }

        internal new void ReadyForUpdateAble()
        {
            Extras.Accepting += (TableExtras.KeyInfo info) =>
            {
                _UpdateAble.Insert(info.Key);
            };

            Extras.Ignoring += (TableExtras.KeyInfo info) =>
            {
                _UpdateAble.Delete(info.Key);
            };
        }

        public override string ToString()
        {
            return "PartTable " + typeof(ValueType).ToString();
        }

    }
}
