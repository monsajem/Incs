using System;
using static Monsajem_Incs.Database.Base.Runer;
using static System.Runtime.Serialization.FormatterServices;

namespace Monsajem_Incs.Database.Base
{
    public static partial class Extentions
    {
    }
}

namespace Monsajem_Incs.Database.Base
{
    public partial class Events<ValueType>
    {
        public class UpdateInfo : ValueInfo
        {
            public object OldKey;
            public int OldPos;
        }

        public Action<(ValueType Value, UpdateInfo[] Info)> Updating;
        public Action<(ValueType Value, UpdateInfo[] Info)> Updated;
    }

    public partial class SecurityEvents<ValueType>
    {
        public Action<(ValueType Value, Events<ValueType>.UpdateInfo[] Info)> Updating;
        internal Action<(ValueType Value, Events<ValueType>.UpdateInfo[] Info)> MakeKeys;
    }

    public partial class Table<ValueType, KeyType>
    {
        public class KeyChangeInfo
        {
            public KeyType OldKey;
            public KeyType NewKey;
            public ValueType Value;
        }

        [Serialization.NonSerialized]
        public Action<KeyChangeInfo> KeyChanging;
        [Serialization.NonSerialized]
        public Action<KeyChangeInfo> KeyChanged;

        private ValueType I_Update(KeyType OldKey, Func<ValueType, ValueType> NewValueMaker)
        {
            lock (this)
            {
                using (Run.UseBlock())
                {
                    var NewValue = GetItem(OldKey, true);
                    AutoFillRelations?.Invoke(NewValue.Value);
                    var KeysLen = BasicActions.Keys;
                    var Info = (NewValue, new Events<ValueType>.UpdateInfo[KeysLen]);
                    for (int i = 0; i < KeysLen; i++)
                        Info.Item2[i] = new Events<ValueType>.UpdateInfo();
                    SecurityEvents.MakeKeys?.Invoke(Info);
                    NewValue.Value = NewValueMaker(NewValue);
                    Info.NewValue = NewValue;
                    SecurityEvents.Updating?.Invoke(Info);
                    Events.Updating?.Invoke(Info);
                    Events.Saving?.Invoke(NewValue);
                    Events.loading?.Invoke(NewValue);
                    Events.Updated?.Invoke(Info);
                    return NewValue;
                }
            }
        }


        public void Update(int Position, ValueType NewValue)
        {
            _ = I_Update(KeysInfo.Keys[Position], (c) => NewValue);
        }
        public void Update(int Position, Func<ValueType, ValueType> NewValueCreator)
        {
            _ = I_Update(KeysInfo.Keys[Position], NewValueCreator);
        }
        public void Update(int Position, Action<ValueType> NewValueCreator)
        {
            _ = I_Update(KeysInfo.Keys[Position], (c) => { NewValueCreator(c); return c; });
        }

        public void Update(ValueType OldValue)
        {
            _ = I_Update(GetKey(OldValue), (c) => OldValue);
        }

        public void Update(KeyType OldKey, ValueType NewValue)
        {
            _ = I_Update(OldKey, (c) => NewValue);
        }

        public void Update(ValueType OldValue, ValueType NewValue)
        {
            _ = I_Update(GetKey(OldValue), (c) => NewValue);
        }
        public ValueType Update(ValueType OldValue, Func<ValueType, ValueType> NewValueCreator)
        {
            return I_Update(GetKey(OldValue), NewValueCreator);
        }
        public ValueType Update(KeyType OldKey, Func<ValueType, ValueType> NewValueCreator)
        {
            return I_Update(OldKey, NewValueCreator);
        }

        public ValueType Update(KeyType OldKey, Action<ValueType> NewValueCreator)
        {
            return I_Update(OldKey, (c) => { NewValueCreator(c); return c; });
        }
        public ValueType Update(ValueType OldValue, Action<ValueType> NewValueCreator)
        {
            return I_Update(GetKey(OldValue), (c) => { NewValueCreator(c); return c; });
        }

        public void Update(
            Action<ValueType> CreateOldValue,
            ValueType NewValue)
        {
            var OldValue = (ValueType)GetUninitializedObject(typeof(ValueType));
            CreateOldValue(OldValue);
            Update(OldValue, NewValue);
        }

        public ValueType Update(
            Action<ValueType> CreateOldValue,
            Action<ValueType> CreateNewValue)
        {
            var OldValue = (ValueType)GetUninitializedObject(typeof(ValueType));
            CreateOldValue(OldValue);
            return Update(OldValue, CreateNewValue);
        }

        public void Update(Action<ValueType> NewValueCreator)
        {
            foreach (var OldKey in KeysInfo.Keys)
                _ = I_Update(OldKey, (c) => { NewValueCreator(c); return c; });
        }

        public void Update(Table<ValueType, KeyType> Values, Action<ValueType> NewValueCreator)
        {
            foreach (var Key in Values.KeysInfo.Keys)
            {
                _ = I_Update(Key, (c) => { NewValueCreator(c); return c; });
            }
        }

    }
}
