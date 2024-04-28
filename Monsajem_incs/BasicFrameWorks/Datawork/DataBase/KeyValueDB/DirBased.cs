using Monsajem_Incs.Collection.Array.Base;
using Monsajem_Incs.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Monsajem_Incs.Database.KeyValue.DirBased
{
    public class FileDictionary<KeyType, ValueType> :
        Collection.IDictionary<KeyType, ValueType>
    {
        private string Dir;
        [Serialization.NonSerialized]
        private Collection.Array.Base.IArray<KeyType> _Keys;
        public FileDictionary(string Dir)
        {
            this.Dir = Dir + "\\";
            _ = System.IO.Directory.CreateDirectory(Dir);
            var DirInfo = new DirectoryInfo(Dir);
            var Files = DirInfo.GetFiles().Select(
                (c) => System.Convert.FromBase64String(c.Name).Deserialize<KeyType>()).ToArray();
            _Keys = new Collection.Array.TreeBased.Array<KeyType>(Files);
        }

        public ValueType this[KeyType Key]
        {
            get => File.ReadAllBytes(Dir + System.Convert.ToBase64String(Key.Serialize())).Deserialize<ValueType>();
            set => File.WriteAllBytes(Dir + System.Convert.ToBase64String(Key.Serialize()), value.Serialize());
        }
        public ValueType this[int Position]
        {
            get => this[Keys[Position]];
            set => this[Keys[Position]] = value;
        }

        public IArray<KeyType> Keys { get => _Keys; set => _Keys = value; }

        public int Count => Keys.Length;

        public void Add(KeyType key, ValueType value)
        {
            if (Keys.BinarySearch(key).Index > -1)
                throw new Exception("Value Is Exist");
            _ = Keys.BinaryInsert(key);
            this[key] = value;
        }

        public bool ContainKey(KeyType Key)
        {
            return Keys.BinarySearch(Key).Index > -1;
        }

        public bool Remove(KeyType key)
        {
            if (Keys.BinaryDelete(key).Index > -1)
            {
                File.Delete(Dir + System.Convert.ToBase64String(key.Serialize()));
                return true;
            }
            return false;
        }

        public bool TryGetValue(KeyType key, out ValueType value)
        {
            if (Keys.BinarySearch(key).Index > -1)
            {
                value = this[key];
                return true;
            }
            value = default;
            return false;
        }

        public IEnumerator<KeyValuePair<KeyType, ValueType>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class Table<ValueType, KeyType> :
        Base.Table<ValueType, KeyType>
        where KeyType : IComparable<KeyType>
    {
        public Table(string Dir,
            Func<ValueType, KeyType> GetKey, bool IsUpdatAble) :
            base((b) => File.WriteAllBytes(Dir + "\\K", b),
                 () => File.Exists(Dir + "\\K") ? File.ReadAllBytes(Dir + "\\K") : null,
                 new FileDictionary<KeyType, (ValueType, ulong)>(Dir + "\\V"), GetKey, IsUpdatAble)
        {
            TableName = new DirectoryInfo(Dir).Name;
        }
    }
}
