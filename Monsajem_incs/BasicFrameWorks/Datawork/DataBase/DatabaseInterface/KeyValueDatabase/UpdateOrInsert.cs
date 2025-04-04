using System;
using static System.Runtime.Serialization.FormatterServices;

namespace Monsajem_Incs.Database.Base
{
    public static partial class Extentions
    {

        public static void UpdateOrInsert<ValueType, KeyType>
            (this Table<ValueType, KeyType> Table,
            Action<ValueType> CreateOldValue,
            ValueType NewValue)
            where ValueType : new()
            where KeyType : IComparable<KeyType>
        {
            var OldValue = new ValueType();
            CreateOldValue(OldValue);
            if (Table.PositionOf(OldValue) > -1)
                Table.Update(OldValue, NewValue);
            else
                _ = Table.Insert(NewValue);
        }

        public static void UpdateOrInsert<ValueType, KeyType>
            (this Table<ValueType, KeyType> Table,
            Action<ValueType> CreateOldValue,
            Action<ValueType> CreateNewValue)
            where ValueType : new()
            where KeyType : IComparable<KeyType>
        {
            var OldValue = new ValueType();
            CreateOldValue(OldValue);
            if (Table.PositionOf(OldValue) > -1)
                _ = Table.Update(OldValue, CreateNewValue);
            else
            {
                _ = Table.Insert(OldValue);
                _ = Table.Update(OldValue, CreateNewValue);
            }
        }
    }
}

namespace Monsajem_Incs.Database.Base
{
    public partial class Table<ValueType, KeyType>
    {
        [Serialization.NonSerialized]
        internal Func<KeyType, Func<ValueType, ValueType>, ValueType> IUpdateOrInsert;

        public void UpdateOrInsert(ValueType OldValue) =>
            IUpdateOrInsert(GetKey(OldValue), (c) => OldValue);

        public void UpdateOrInsert(ValueType OldValue, ValueType NewValue) =>
            IUpdateOrInsert(GetKey(OldValue), (c) => NewValue);

        public void UpdateOrInsert(KeyType OldKey, ValueType NewValue) =>
            IUpdateOrInsert(OldKey, (c) => NewValue);

        public ValueType UpdateOrInsert(KeyType OldKey, Func<ValueType, ValueType> NewValueCreator) =>
            IUpdateOrInsert(OldKey, NewValueCreator);

        public ValueType UpdateOrInsert(KeyType OldKey, Action<ValueType> NewValueCreator) =>
            IUpdateOrInsert(OldKey, (c) => { NewValueCreator(c); return c; });

        public ValueType UpdateOrInsert(ValueType OldValue, Func<ValueType, ValueType> NewValueCreator) =>
            UpdateOrInsert(GetKey(OldValue), NewValueCreator);

        public ValueType UpdateOrInsert(ValueType OldValue, Action<ValueType> NewValueCreator) =>
            UpdateOrInsert(GetKey(OldValue), NewValueCreator);

        public void UpdateOrInsert(Action<ValueType> NewValueCreator)
        {
            var Value = (ValueType)GetUninitializedObject(typeof(ValueType));
            NewValueCreator(Value);
            _ = IUpdateOrInsert(GetKey(Value), (c) => Value);
        }

        public void UpdateOrInsert(Action<ValueType> NewValueCreator, Action<ValueType> Updator)
        {
            var Value = (ValueType)GetUninitializedObject(typeof(ValueType));
            NewValueCreator(Value);
            _ = IUpdateOrInsert(GetKey(Value), (c) => { Updator(c); return c; });
        }
    }
}
