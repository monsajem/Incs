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
        [Serialization.NonSerialized]
        [CopyOrginalObject]
        public System.IO.Stream Stream;
        
        public StreamDictionary(int MinCount = 5000)
        {
            Info.minCount = MinCount;
            Info.MinLen = -1 * Info.minCount;
            Info.MaxLen = Info.minCount;
            Info.Keys = new SortedDictionary<KeyType, Data>();
            Info.GapsByFrom = new SortedSet<DataByForm>();
            Info.GapsByLen = new SortedSet<DataByLen>();
            Info.GapsByTo = new SortedSet<DataByTo>();
        }

        public ICollection<KeyType> Keys => Info.Keys.Keys;

        public ICollection<ValueType> Values => throw new NotImplementedException();

        public int Count => Info.Keys.Count;

        public bool IsReadOnly => false;

        public ValueType this[KeyType key]
        {
            get => GetItem(key);
            set => SetItem(key, value);
        }

        private void DeleteLen(int Count)
        {
            Info.StreamLen = Info.StreamLen - Count;
            if (Info.StreamLen < Info.MinLen)
            {
                Info.MaxLen = Info.StreamLen + Info.minCount;
                Info.MinLen = Info.StreamLen - Info.minCount;
                Stream.SetLength(Info.MaxLen);
            }
        }
        private void AddLen(long Count)
        {
            Info.StreamLen = Info.StreamLen + Count;
            if (Info.StreamLen > Info.MaxLen)
            {
                Info.MaxLen = Info.StreamLen + Info.minCount;
                Info.MinLen = Info.StreamLen - Info.minCount;
                Stream.SetLength(Info.MaxLen);
            }
        }

        public void Add(KeyType key, ValueType value)
        {
            InnerInser(value.Serialize(), key);
        }

        public bool ContainsKey(KeyType key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyType key)
        {
            if (Info.Keys.ContainsKey(key) == false)
                return false;
#if DEBUG
            Info.Browse(this);
#endif
            var DataLoc = Info.PopData(key);
            var NGap = DataLoc;
            Data BGap;
            if (Info.PopNextGap(ref NGap))
            {
                DataLoc.To = NGap.To;
                DataLoc.Len += NGap.Len;
            }
            if (DataLoc.To == Info.StreamLen - 1)//// is last data;
            {
                BGap = DataLoc;
                if (Info.PopBeforeGap(ref BGap))
                    DataLoc.Len += BGap.Len;
                DeleteLen(DataLoc.Len);
                Stream.Flush();
            }
            else
            {
                BGap = DataLoc;
                if (Info.PopBeforeGap(ref BGap))
                {
                    DataLoc.From = BGap.From;
                    DataLoc.Len += BGap.Len;
                }
                Info.InsertGap(DataLoc);
            }
#if DEBUG
            Info.Browse(this);
#endif
            return true;
        }

        public bool TryGetValue(KeyType key, [MaybeNullWhen(false)] out ValueType value)
        {
            throw new NotImplementedException();
        }

        public void Add(KeyValuePair<KeyType, ValueType> item)
        {
            Add(item.Key,item.Value);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<KeyType, ValueType> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<KeyType, ValueType>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<KeyType, ValueType> item)
        {
            throw new NotImplementedException();
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
