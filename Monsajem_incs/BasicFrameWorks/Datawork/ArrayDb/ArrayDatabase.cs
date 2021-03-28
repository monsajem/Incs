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
            bool IsUpdateAble,string Name=""):
            base(new SortedDictionary<KeyType,ValueType>(), GetKey,
                 IsUpdateAble)
        {
            this.TableName = Name;
        }
    }
}