using System;
using System.IO;
using System.Linq.Expressions;
using Monsajem_Incs.Collection;
using Monsajem_Incs.Serialization;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.DynamicAssembly;
namespace Monsajem_Incs.Database.DirectoryTable
{
    public class DirectoryTable<ValueType, KeyType> :
        Table<ValueType, KeyType>
        where KeyType : IComparable<KeyType>
    {
        public DirectoryTable(
            string DirectoryAddress,
            Func<ValueType, KeyType> GetKey,
            bool IsUpdateAble) :
            base(new StreamCollection<(ValueType,ulong)>(File.Open(DirectoryAddress + "\\Data", FileMode.OpenOrCreate)),
                 new Register.FileRegister<ulong>(DirectoryAddress + "\\Register"), GetKey, IsUpdateAble)
        {
            this.TableName = new DirectoryInfo(DirectoryAddress).Name;
            Directory.CreateDirectory(DirectoryAddress);
            foreach (var Value in this.BasicActions.Items)
                KeysInfo.Keys.BinaryInsert(GetKey(Value.Value));
        }
    }
}