using Monsajem_Incs.Database.KeyValue.WebStorageBased;
using Monsajem_Incs.Serialization;
using WebAssembly.Browser.DOM;

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

        protected override ValueType LoadData()
        {
            return MyUTF.GetBytes(WebStorage.GetItem(Key)).Deserialize<ValueType>();
        }

        protected override void SaveData(ValueType Data)
        {
            WebStorage.SetItem(Key, MyUTF.GetString(Data.Serialize()));
        }
    }
}