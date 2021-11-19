using System;
using System.IO;
using System.Linq.Expressions;
using Monsajem_Incs.Collection;
using Monsajem_Incs.Serialization;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.DynamicAssembly;
using System.Threading.Tasks;

namespace Monsajem_Incs.Database.Register
{
    public class FileRegister<ValueType>:
        Base.Register<ValueType>
    {
        [Serialization.NonSerialized]
        private string FileAddress;

        public FileRegister(string FileAddress)
        {
            this.FileAddress = FileAddress;
        }

        protected override async Task<byte[]> LoadData()
        {
            return System.IO.File.ReadAllBytes(FileAddress);
        }

        protected override async Task SaveData(byte[] Data)
        {
            System.IO.File.WriteAllBytes(FileAddress, Data);
        }
    }
}