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

    public interface IDictionary<KeyType, ValueType> :
        IEnumerable<KeyValuePair<KeyType, ValueType>>
    {
        ValueType this[KeyType Key] { get; set; }
        Array.Base.IArray<KeyType> Keys { get; set; }
        void Add(KeyType key, ValueType value);
        bool Remove(KeyType key);
        int Count { get; }
        bool ContainsKey(KeyType key) => Keys.BinarySearch(key).Index > 0;
        ValueType this[int Position]{get;set;}
        bool TryGetValue(KeyType key, out ValueType value);
    }

    public class Dictionary<KeyType, ValueType>:
        IDictionary<KeyType,ValueType>
    {
        public Dictionary(Array.Base.IArray<KeyType> Keys,
                          Array.Base.IArray<ValueType> Values)
        {
            this.Keys = Keys;
            this.Values = Values;
        }

        public Array.Base.IArray<KeyType> Keys;
        public Array.Base.IArray<ValueType> Values;
        Array.Base.IArray<KeyType> IDictionary<KeyType, ValueType>.Keys { get => Keys; set => Keys = value; }

        public void Add(KeyType key, ValueType value) 
        {
            var Position = Keys.BinarySearch(key).Index;
            if (Position > -1)
                throw new Exception("Key is exist!");
            Position = ~Position;
            Keys.Insert(key, Position);
            Values.Insert(value, Position);
        }
        public virtual bool Remove(KeyType key)
        {
            var Position = Keys.BinaryDelete(key).Index;
            if (Position > -1)
            {
                Keys.DeleteByPosition(Position);
                return true;
            }
            return false;
        }
        public virtual int Count { get=>Values.Length; }
        public virtual bool ContainsKey(KeyType key)=> Keys.BinarySearch(key).Index>0;
        public virtual ValueType this[KeyType key]
        {
            get
            {
                var Position = Keys.BinarySearch(key).Index;
                if (Position < 0)
                    throw new Exception("Key not found!");
                return Values[Position];
            }
            set
            {
                var Position = Keys.BinarySearch(key).Index;
                if (Position < 0)
                    throw new Exception("Key not found!");
                Values[Position] = value;
            }
        }
        public virtual ValueType this[int Position]
        {
            get => this.Values[Position];
            set => this.Values[Position] = value;
        }
        public virtual bool TryGetValue(KeyType key, out ValueType value)
        {
            if (ContainsKey(key))
            {
                value = this[key];
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        public IEnumerator<KeyValuePair<KeyType, ValueType>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
