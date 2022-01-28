using System;
using System.IO;
using System.Linq.Expressions;
using Monsajem_Incs.Collection;
using Monsajem_Incs.Serialization;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.DynamicAssembly;
using System.Threading.Tasks;

namespace Monsajem_Incs.Database.Register.Base
{
    public abstract class Register<ValueType>:ISerializable<ValueType>
    {
        public ValueType Value;
        private BlocksCounter RunInBlocks;

        public Register()
        {
            RunInBlocks = new BlocksCounter();
            RunInBlocks.OnClosedAllBlocks += () => this.Save();
        }

        protected abstract void SaveData(ValueType Value);
        protected abstract ValueType LoadData();

        public void Save() => SaveData(Value);
        public void Save(ValueType Value)
        {
            this.Value = Value;
            SaveData(Value);
        }
        public void Load(Func<ValueType> CreateNewData)
        {
            try
            {
                Value = LoadData();
            }
            catch
            {
                Value = CreateNewData();
                Save();
            }
        }
        public void Load() => Load(() => default);

        public BlocksCounter.BlockContainer SaveAfterBlocks()
        {
            return RunInBlocks.UseBlock();
        }

        public static implicit operator ValueType (Register <ValueType> Register)=> Register.Value;

        public override string ToString()
        {
            return Value.ToString();
        }

        public ValueType GetData() => Value;

        public void SetData(ValueType Data)
        {
            Value = Data;
            RunInBlocks = new BlocksCounter();
        }
    }
}