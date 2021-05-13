using System;
using System.IO;
using System.Linq.Expressions;
using Monsajem_Incs.Serialization;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Collection.Array.Base;
using static System.Text.Encoding;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using WebAssembly.Browser.DOM;

namespace Monsajem_Incs.Database.KeyValue.WebStorageBased
{
    internal class MyUTF
    {
        public static byte[] GetBytes(string str)
        {
            return System.Convert.FromBase64String(ASCII.GetString(Unicode.GetBytes(str)));
            //byte[] bytes = new byte[str.Length * 2];
            //System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            //return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            return Unicode.GetString(ASCII.GetBytes(System.Convert.ToBase64String(bytes)));
            //char[] chars = new char[(bytes.Length / 2) + bytes.Length % 2];
            //System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            //return new string(chars);
        }
    }

    public class StorageDictionary<KeyType, ValueType> : 
        Collection.IDictionary<KeyType, ValueType>
    {
        public Storage WebStorage;
        private string StorageKey;
        public StorageDictionary(string Key,Storage WebStorage)
        {
            this.StorageKey = Key;
            this.WebStorage = WebStorage;
        }

        public ValueType this[KeyType Key] { 
            get
            {
                var Str_Key = WebStorage.GetItem(StorageKey + MyUTF.GetString(Key.Serialize()));
                return MyUTF.GetBytes(Str_Key).Deserialize<ValueType>();
            }
            set
            {
                WebStorage.SetItem(
                      StorageKey + MyUTF.GetString(Key.Serialize()),
                      MyUTF.GetString(value.Serialize()));
            }
        }
        public ValueType this[int Position] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IArray<KeyType> Keys { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Count => Keys.Length;

        public void Add(KeyType key, ValueType value)
        {
            if (Keys.BinarySearch(key).Index > -1)
                throw new Exception("Value Is Exist");
            Keys.BinaryInsert(key);
            this[key] = value;
        }

        public bool ContainKey(KeyType Key)
        {
            return Keys.BinarySearch(Key).Index>-1;
        }

        public IEnumerator<KeyValuePair<KeyType, ValueType>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyType key)
        {
            if(Keys.BinaryDelete(key).Index>-1)
            {
                WebStorage.RemoveItem(StorageKey + MyUTF.GetString(key.Serialize()));
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
            Func<ValueType, KeyType> GetKey, bool IsUpdatAble,Storage WebStorage) :
            base(
                 (b) =>
                 {
                     var KeyName = "K" + MyUTF.GetString(TableName.Serialize());
                     WebStorage.SetItem(KeyName, MyUTF.GetString(b));
                 },
                 () =>
                 {
                     var KeyName = "K" + MyUTF.GetString(TableName.Serialize());
                     if (WebStorage.GetItem(KeyName)!=null)
                         return MyUTF.GetBytes(WebStorage.GetItem(KeyName));
                     return null;
                 },
                 new StorageDictionary<KeyType, ValueType>("V" + TableName, WebStorage), GetKey, IsUpdatAble)
        {
            this.TableName = TableName;
        }        
    }
}
