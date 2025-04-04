﻿using System;
using System.Collections.Generic;
using static Monsajem_Incs.Database.Base.Runer;
using static System.Runtime.Serialization.FormatterServices;

namespace Monsajem_Incs.Database.Base
{
    public partial class PartOfTable<ValueType, KeyType>
    {
        public partial class TableExtras
        {
            public Action<KeyInfo> Accepting;
            public Action<KeyInfo> Accepted;

            public class KeyInfo
            {
                public KeyType Key;
                public int Pos;
            }
        }

        public int Accept(KeyType Key)
        {
            using (Run.UseBlock())
            {
                var KeyInfo = new TableExtras.KeyInfo
                {
                    Key = Key
                };
                Extras.Accepting?.Invoke(KeyInfo);
                KeyInfo.Pos = KeysInfo.Keys.BinaryInsert(Key);
                Extras.Accepted?.Invoke(KeyInfo);
                return KeyInfo.Pos;
            }
        }

        public void Accept(Action<ValueType> ValueCreator)
        {
            var Value = (ValueType)GetUninitializedObject(typeof(ValueType));
            ValueCreator(Value);
            Accept(Value);
        }

        public void Accept(ValueType Value)
        {
            var Key = base.GetKey(Value);
            _ = Accept(Key);
        }

        public void Accept(Table<ValueType, KeyType> Values)
        {
            foreach (var Key in Values.KeysInfo.Keys)
                _ = Accept(Key);
        }

        public void Accept(IEnumerable<ValueType> Values)
        {
            foreach (var Value in Values)
                Accept(Value);
        }
        public void Accept(IEnumerable<KeyType> Keys)
        {
            foreach (var Key in Keys)
                _ = Accept(Key);
        }

    }
}
