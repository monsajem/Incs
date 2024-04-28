using Monsajem_Incs.Database.Register.Base;

namespace Monsajem_Incs.Database.Register
{

    public class MemoryRegister<ValueType> :
        Register<ValueType>
    {
        protected override ValueType LoadData()
        {
            return Value;
        }

        protected override void SaveData(ValueType Value)
        { }
    }
}