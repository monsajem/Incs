using System;
using System.IO;
using System.Linq.Expressions;
using Monsajem_Incs.Collection;
using Monsajem_Incs.Serialization;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.DynamicAssembly;
using System.Threading.Tasks;
using Monsajem_Incs.Database.Register.Base;

namespace Monsajem_Incs.Database.Register
{

    public class FileRegister<ValueType>:
        Register<ValueType>
    {
        [Serialization.NonSerialized]
        private string FileAddress;

        public FileRegister(string FileAddress)
        {
            this.FileAddress = FileAddress;
        }

        protected override ValueType LoadData()
        {
            return System.IO.File.ReadAllBytes(FileAddress).Deserialize<ValueType>();
        }

        protected override void SaveData(ValueType Data)
        {
            System.IO.File.WriteAllBytes(FileAddress, Data.Serialize());
        }
    }
}