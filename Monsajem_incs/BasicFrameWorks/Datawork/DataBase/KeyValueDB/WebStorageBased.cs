using Monsajem_Incs.Collection.Array.Base;
using Monsajem_Incs.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using WebAssembly.Browser.DOM;
using static System.Text.Encoding;

namespace Monsajem_Incs.Database.KeyValue.WebStorageBased
{
    internal class MyUTF
    {
        public static byte[] GetBytes(string str) =>
            System.Convert.FromBase64String(ASCII.GetString(Unicode.GetBytes(str)));

        public static string GetString(byte[] bytes) =>
            Unicode.GetString(ASCII.GetBytes(System.Convert.ToBase64String(bytes)));
    }

    public class StorageDictionary<KeyType, ValueType> :
        Collection.IDictionary<KeyType, ValueType>
    {
        public Storage WebStorage;
        private string StorageKey;
        public StorageDictionary(string Key, Storage WebStorage)
        {
            StorageKey = Key;
            this.WebStorage = WebStorage;
        }

        private string GetKey(KeyType Key) => StorageKey + MyUTF.GetString(Key.Serialize());

        public ValueType this[KeyType Key]
        {
            get
            {
                var Str_Key = WebStorage.GetItem(GetKey(Key));
                return MyUTF.GetBytes(Str_Key).Deserialize<ValueType>();
            }
            set
            {
                WebStorage.SetItem(GetKey(Key), MyUTF.GetString(value.Serialize()));
            }
        }

        public ValueType this[int Position] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IArray<KeyType> Keys { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Count => Keys.Length;

        public void Add(KeyType key, ValueType value)
        {
            if (ContainKey(key))
                throw new Exception("Value Is Exist");
            this[key] = value;
        }

        public bool ContainKey(KeyType Key)
        {
            return WebStorage.Contains(GetKey(Key));
        }

        public IEnumerator<KeyValuePair<KeyType, ValueType>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyType key)
        {
            var StrKey = GetKey(key);
            if (WebStorage.Contains(StrKey))
            {
                WebStorage.RemoveItem(StrKey);
                return true;
            }
            return false;
        }

        public bool TryGetValue(KeyType key, out ValueType value)
        {
            var StrKey = GetKey(key);
            if (WebStorage.Contains(StrKey))
            {
                value = MyUTF.GetBytes(StrKey).Deserialize<ValueType>();
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

    public class Table<ValueType, KeyType> :
        Base.Table<ValueType, KeyType>
        where KeyType : IComparable<KeyType>
    {
        public Table(string TableName,
            Func<ValueType, KeyType> GetKey, bool IsUpdatAble, Storage WebStorage) :
            base(
                 (b) =>
                 {
                     var KeyName = "K" + MyUTF.GetString(TableName.Serialize());
                     WebStorage.SetItem(KeyName, MyUTF.GetString(b));
                 },
                 () =>
                 {
                     var KeyName = "K" + MyUTF.GetString(TableName.Serialize());
                     return WebStorage.GetItem(KeyName) != null ? MyUTF.GetBytes(WebStorage.GetItem(KeyName)) : null;
                 },
                 new StorageDictionary<KeyType, (ValueType, ulong)>("V" + TableName, WebStorage), GetKey, IsUpdatAble)
        {
            this.TableName = TableName;
        }
    }
}