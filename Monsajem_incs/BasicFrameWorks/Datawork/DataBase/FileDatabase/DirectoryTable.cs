using Monsajem_Incs.Collection;
using Monsajem_Incs.Database.Base;
using System;
using System.IO;
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
            base(new StreamCollection<(ValueType, ulong)>(File.Open(DirectoryAddress + "\\Data", FileMode.OpenOrCreate)),
                 new Register.FileRegister<ulong>(DirectoryAddress + "\\Register"), GetKey, IsUpdateAble)
        {
            TableName = new DirectoryInfo(DirectoryAddress).Name;
            _ = Directory.CreateDirectory(DirectoryAddress);
            foreach (var Value in BasicActions.Items)
                _ = KeysInfo.Keys.BinaryInsert(GetKey(Value.Value));
        }
    }
}