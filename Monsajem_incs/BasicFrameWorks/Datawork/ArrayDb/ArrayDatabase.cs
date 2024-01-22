using System;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.Serialization;
using System.Collections.Generic;

namespace Monsajem_Incs.Database
{
    public class ArrayTable<ValueType,KeyType>:
        Table<ValueType,KeyType>
        where KeyType:IComparable<KeyType>
        
    {
        public ArrayTable(
            Func<ValueType,KeyType> GetKey,
            bool IsUpdateAble,string Name=null):
            base(new Collection.Array.TreeBased.Array<(ValueType,ulong)>(),
                 new Register.MemoryRegister<ulong>(),                 
                 GetKey,
                 IsUpdateAble)
        {
            if(Name!=null)
                this.TableName = Name;
        }
    }
}