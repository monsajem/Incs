using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monsajem_Incs.Database.Base;
using WebAssembly.Browser.DOM;
using Monsajem_Incs.Collection.Array.ArrayBased.DynamicSize;

namespace MonsajemData
{
    public abstract class TableMaker
    {
        public abstract Table<ValueType, KeyType> MakeDB<ValueType, KeyType>(string Name, Func<ValueType, KeyType> GetKey)
            where KeyType : IComparable<KeyType>;
    }
    public abstract partial class DataBase<UserType>
    {

        protected void MakeDB<ValueType, KeyType>
            (ref Table<ValueType, KeyType> TBL,string Name, Func<ValueType, KeyType> GetKey)
            where KeyType : IComparable<KeyType>
        {
            var Result = tableMaker.MakeDB(Name, GetKey);
            var TblInfo = _MakeFinder<ValueType, KeyType>(Name, Result.GetHashCode(), "");
            TblInfo.Tbl= Result;
            TBL = Result;
        }
    }
}