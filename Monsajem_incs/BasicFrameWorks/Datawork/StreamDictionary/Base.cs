using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Serialization;
using Monsajem_Incs.Database;
using Monsajem_Incs.Collection.Array.TreeBased;
using System.Diagnostics.CodeAnalysis;
using System.Collections;
using Monsajem_Incs.Collection;

namespace Monsajem_Incs.Collection
{

    public partial class StreamDictionary<KeyType,ValueType>:
        IDictionary<KeyType,ValueType>
    { 
        public StreamCollection Collection;
        public Array<KeyType> MyKeys;
        
        public StreamDictionary(int MinCount = 5000)
        {
            Collection = new StreamCollection(MinCount);
            MyKeys = new Array<KeyType>();
        }

        public ValueType this[KeyType key] 
        {
            get
            {
                var Position = MyKeys.BinarySearch(key).Index;
                if (Position < 0)
                    throw new Exception("Key not found!");
                return Collection[Position].Deserialize<ValueType>();
            }
            set
            {
                var Position = MyKeys.BinarySearch(key).Index;
                if (Position < 0)
                    throw new Exception("Key not found!");
                Collection[Position] = value.Serialize();
            } 
        }

        public ICollection<KeyType> Keys => throw new NotImplementedException();

        public ICollection<ValueType> Values => throw new NotImplementedException();

        public int Count => Collection.Length;

        public bool IsReadOnly => false;

        public void Add(KeyType key, ValueType value)
        {
            var Position = MyKeys.BinarySearch(key).Index;
            if (Position > -1)
                throw new Exception("Key is exist!");
            Position =~Position;
            MyKeys.Insert(key,Position);
            Collection.Insert(value.Serialize(),Position);
        }

        public void Add(KeyValuePair<KeyType, ValueType> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            Collection.Clear();
            MyKeys.Clear();
        }

        public bool Contains(KeyValuePair<KeyType, ValueType> item)
        {
            return ContainsKey(item.Key);
        }

        public bool ContainsKey(KeyType key)
        {
            return MyKeys.BinarySearch(key).Index > -1;
        }

        public void CopyTo(KeyValuePair<KeyType, ValueType>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<KeyType, ValueType>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyType key)
        {
            var Position = MyKeys.BinaryDelete(key).Index;
            if(Position>-1)
            {
                Collection.DeleteByPosition(Position);
                return true;
            }
            return false;
        }

        public bool Remove(KeyValuePair<KeyType, ValueType> item)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(KeyType key, [MaybeNullWhen(false)] out ValueType value)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
