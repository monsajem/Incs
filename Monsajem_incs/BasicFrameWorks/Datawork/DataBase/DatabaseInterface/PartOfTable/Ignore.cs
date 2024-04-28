using System;
using System.Collections.Generic;
using static Monsajem_Incs.Database.Base.Runer;
using static System.Runtime.Serialization.FormatterServices;

namespace Monsajem_Incs.Database.Base
{
    public partial class PartOfTable<ValueType, KeyType>
    {
        public partial class TableExtras
        {
            public Action<KeyInfo> Ignoring;
            public Action<KeyInfo> Ignored;
        }

        public int Ignore(KeyType Key)
        {
            using (Run.UseBlock())
            {
                var Keyinfo = new TableExtras.KeyInfo
                {
                    Key = Key
                };
                Extras.Ignoring?.Invoke(Keyinfo);
                Keyinfo.Pos = KeysInfo.Keys.BinaryDelete(Key).Index;
                Extras.Ignored?.Invoke(Keyinfo);
                return Keyinfo.Pos;
            }
        }

        public void Ignore(Action<ValueType> ValueCreator)
        {
            var Value = (ValueType)GetUninitializedObject(typeof(ValueType));
            ValueCreator(Value);
            Ignore(Value);
        }

        public void Ignore(ValueType Value)
        {
            var Key = base.GetKey(Value);
            _ = Ignore(Key);
        }

        public void Ignore(Table<ValueType, KeyType> Values)
        {
            foreach (var Key in Values.KeysInfo.Keys)
                _ = Ignore(Key);
        }

        public void Ignore(IEnumerable<ValueType> Values)
        {
            foreach (var Value in Values)
                Ignore(Value);
        }
        public void Ignore(IEnumerable<KeyType> Keys)
        {
            foreach (var Key in Keys)
                _ = Ignore(Key);
        }

        public void Ignore()
        {
            foreach (var Key in KeysInfo.Keys.ToArray())
                _ = Ignore(Key);
        }
    }
}
