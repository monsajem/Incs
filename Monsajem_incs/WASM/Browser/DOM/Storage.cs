using WebAssembly.Browser.MonsajemDomHelpers;
using System.Runtime.InteropServices.JavaScript;

namespace WebAssembly.Browser.DOM
{
    public class Storage
    {
        public enum Type : byte
        {
            LocalStorage,
            SessionStorage
        }

        private string MyType;

        public Storage(Type type)
        {
            if (type == Type.LocalStorage)
                MyType = "localStorage";
            else if (type == Type.SessionStorage)
                MyType = "sessionStorage";
            else
                throw new System.Exception("Storage type is not valid!");
        }

        public int Length { get => int.Parse(Runtime.InvokeJS($"{MyType}.length;")); }
        public string Key(int Position) => Runtime.InvokeJS($"{MyType}.key({Position});");
        public string GetItem(string Key)
        {
            MonsajemDataTransport.SetJsVar("K", Key);
            var Result = Runtime.InvokeJS(
                $"{MyType}.getItem({MonsajemDataTransport.ObjectName}.K);");
            MonsajemDataTransport.SetJsVar("K", "");
            return Result;
        }
        public void SetItem(string Key, string Value)
        {
            MonsajemDataTransport.SetJsVar("K", Key);
            MonsajemDataTransport.SetJsVar("V", Value);
            Runtime.InvokeJS(
                $"{MyType}.setItem({MonsajemDataTransport.ObjectName}.K,{MonsajemDataTransport.ObjectName}.V);");
            MonsajemDataTransport.SetJsVar("K", "");
            MonsajemDataTransport.SetJsVar("V", "");
        }
        public void RemoveItem(string Key)
        {
            MonsajemDataTransport.SetJsVar("K", Key);
            Runtime.InvokeJS(
                $"{MyType}.removeItem({MonsajemDataTransport.ObjectName}.K);");
            MonsajemDataTransport.SetJsVar("K", "");
        }
        public bool Contains(string Key)
        {
            MonsajemDataTransport.SetJsVar("K", Key);
            var Result = Runtime.InvokeJS(
                $"{MyType}.hasOwnProperty({MonsajemDataTransport.ObjectName}.K);") == "true";
            MonsajemDataTransport.SetJsVar("K", "");
            return Result;
        }
        public void Clear() => Runtime.InvokeJS($"{MyType}.clear();");
    }
}
