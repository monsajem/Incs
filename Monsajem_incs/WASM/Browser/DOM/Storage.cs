using WebAssembly.Browser.MonsajemDomHelpers;
using System.Runtime.InteropServices.JavaScript;

namespace WebAssembly.Browser.DOM
{
    
    public class Storage
    {
        public enum Type:byte
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
        public string GetItem(string Key) => Runtime.InvokeJS($"{MyType}.getItem({js.Js(Key)});");
        public void SetItem(string Key, string Value) => Runtime.InvokeJS($"{MyType}.setItem({js.Js(Key)},{js.Js(Value)});");
        public void RemoveItem(string Key) => Runtime.InvokeJS($"{MyType}.removeItem({js.Js(Key)});");
        public bool Contains(string Key)=> Runtime.InvokeJS($"{MyType}.hasOwnProperty({js.Js(Key)});")== "true";
        public void Clear() => Runtime.InvokeJS($"{MyType}.clear();");
    }
}
