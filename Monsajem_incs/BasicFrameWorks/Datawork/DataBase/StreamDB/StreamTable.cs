using System;
using System.IO;
using System.Linq.Expressions;
using Monsajem_Incs.Collection;
using Monsajem_Incs.Serialization;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.DynamicAssembly;
namespace Monsajem_Incs.Database.Stream
{
    public class StreamTable<ValueType, KeyType> :
        Table<ValueType, KeyType>
        where KeyType : IComparable<KeyType>
    {
        public StreamTable(
            string TableName,
            System.IO.Stream Stream,
            Func<ValueType, KeyType> GetKey,
            bool IsUpdateAble) :
            base(new StreamCollection<(ValueType,ulong)>(Stream),
                 new Register.MemoryRegister<ulong>(), GetKey, IsUpdateAble)
        {
            this.TableName = TableName;
            foreach (var Value in this.BasicActions.Items)
                KeysInfo.Keys.BinaryInsert(GetKey(Value.Value));
        }
    }
}