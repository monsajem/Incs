using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Serialization;
using Monsajem_Incs.Database;
using Monsajem_Incs.Array.Hyper;
using System.Diagnostics.CodeAnalysis;
using System.Collections;
using Monsajem_Incs.StreamCollection;

namespace Monsajem_Incs.Collection
{

    public partial class DynamicDictionary<KeyType, ValueType> :
        IDictionary<KeyType, ValueType>
    {
        public Func<KeyType, ValueType> _GetItem;
        public Func<KeyType, bool> _Remove;
        public Action<KeyType, ValueType> _AddItem;
        public Action<KeyType, ValueType> _SetItem;
        public Func<ICollection<KeyType>> _GetKeys;
        public Func<ICollection<ValueType>> _GetValues;
        public Func<int> _Count;

        public ValueType this[KeyType key] { get =>_GetItem(key); set =>_SetItem(key,value); }

        public ICollection<KeyType> Keys => _GetKeys();

        public ICollection<ValueType> Values => _GetValues();

        public int Count => _Count();

        public bool IsReadOnly =>_AddItem==null && _SetItem==null && _Remove==null;

        public void Add(KeyType key, ValueType value) => _AddItem(key,value);

        public void Add(KeyValuePair<KeyType, ValueType> item) => Add(item.Key, item.Value);

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<KeyType, ValueType> item) => ContainsKey(item.Key);

        public bool ContainsKey(KeyType key)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<KeyType, ValueType>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<KeyType, ValueType>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyType key) => _Remove(key);

        public bool Remove(KeyValuePair<KeyType, ValueType> item) => Remove(item.Key);

        public bool TryGetValue(KeyType key, out ValueType value)
        {
            if (ContainsKey(key))
            {
                value = _GetItem(key);
                return true;
            }
            value = default;
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
