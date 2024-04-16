using WebAssembly.Browser.MonsajemDomHelpers;
using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;

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

        public int Length { get => int.Parse(js.JsEval($"{MyType}.length;")); }
        public string Key(int Position) => js.JsEval($"{MyType}.key({Position});");
        public string GetItem(string Key)
        {
            MonsajemDataTransport.SetJsVar("K", Key);
            var Result = js.JsEval(
                $"{MyType}.getItem({MonsajemDataTransport.ObjectName}.K);");
            MonsajemDataTransport.SetJsVar("K", "");
            return Result;
        }
        public void SetItem(string Key, string Value)
        {
            MonsajemDataTransport.SetJsVar("K", Key);
            MonsajemDataTransport.SetJsVar("V", Value);
            js.JsEval(
                $"{MyType}.setItem({MonsajemDataTransport.ObjectName}.K,{MonsajemDataTransport.ObjectName}.V);");
            MonsajemDataTransport.SetJsVar("K", "");
            MonsajemDataTransport.SetJsVar("V", "");
        }
        public void RemoveItem(string Key)
        {
            MonsajemDataTransport.SetJsVar("K", Key);
            js.JsEval(
                $"{MyType}.removeItem({MonsajemDataTransport.ObjectName}.K);");
            MonsajemDataTransport.SetJsVar("K", "");
        }
        public bool Contains(string Key)
        {
            MonsajemDataTransport.SetJsVar("K", Key);
            var Result = js.JsEval(
                $"{MyType}.hasOwnProperty({MonsajemDataTransport.ObjectName}.K);") == "true";
            MonsajemDataTransport.SetJsVar("K", "");
            return Result;
        }
        public void Clear() => js.JsEval($"{MyType}.clear();");
    }
}
