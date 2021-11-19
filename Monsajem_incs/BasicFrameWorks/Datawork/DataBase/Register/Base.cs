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
    public abstract class Register<ValueType>
    {
        public ValueType Data;
        protected abstract Task SaveData(byte[] Data);
        protected abstract Task<byte[]> LoadData();
        public async Task Save() => await SaveData(this.Serialize());
        public async Task Load(Func<ValueType> CreateNewData)
        {
            try
            {
                Data = (await LoadData()).Deserialize<ValueType>();
            }
            catch
            {
                Data = CreateNewData();
                await Save();
            }
        }

        public async Task Action(Action<ValueType> Ac)
        {
            Ac?.Invoke(Data);
            await Save();
        }
        public async Task Action(Func<ValueType,Task> Ac)
        {
            await Ac?.Invoke(Data);
            await Save();
        }
    }
}