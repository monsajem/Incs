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

    //public class StreamRegister<ValueType>:
    //    Register<ValueType>
    //{
    //    [Serialization.NonSerialized]
    //    private System.IO.Stream Stream;

    //    public StreamRegister(System.IO.Stream Stream)
    //    {
    //        this.Stream = Stream;
    //    }

    //    protected override ValueType LoadData()
    //    {
    //        return Stream..ReadAllBytes(FileAddress).Deserialize<ValueType>();
    //    }

    //    protected override void SaveData(ValueType Data)
    //    {
    //        System.IO.File.WriteAllBytes(FileAddress, Data.Serialize());
    //    }
    //}
}