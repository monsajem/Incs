using Monsajem_Incs.Database.Base;
using System;

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
            (ref Table<ValueType, KeyType> TBL, string Name, Func<ValueType, KeyType> GetKey)
            where KeyType : IComparable<KeyType>
        {
            var Result = tableMaker.MakeDB(Name, GetKey);
            TBL = Result;
        }
    }
}