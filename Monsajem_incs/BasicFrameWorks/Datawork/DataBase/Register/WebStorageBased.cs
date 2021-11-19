using System;
using System.IO;
using System.Linq.Expressions;
using Monsajem_Incs.Collection;
using Monsajem_Incs.Serialization;
using Monsajem_Incs.Database.Base;
using Monsajem_Incs.DynamicAssembly;
using System.Threading.Tasks;
using WebAssembly.Browser.DOM;
using Monsajem_Incs.Database.KeyValue.WebStorageBased;

namespace Monsajem_Incs.Database.Register
{
    public class WebStorageRegister<ValueType> :
        Base.Register<ValueType>
    {
        [Serialization.NonSerialized]
        private string Key;
        [Serialization.NonSerialized]
        private Storage WebStorage;

        public WebStorageRegister(Storage WebStorage, string Key)
        {
            this.Key = Key;
            this.WebStorage = WebStorage;
        }

        protected override async Task<byte[]> LoadData()
        {
            return MyUTF.GetBytes(WebStorage.GetItem(Key));
        }

        protected override async Task SaveData(byte[] Data)
        {
            WebStorage.SetItem(Key,MyUTF.GetString(Data));
        }
    }
}