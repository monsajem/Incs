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

    public class MemoryRegister<ValueType>:
        Register<ValueType>
    {
        protected override ValueType LoadData()
        {
            return Value;
        }

        protected override void SaveData(ValueType Value)
        {}
    }
}