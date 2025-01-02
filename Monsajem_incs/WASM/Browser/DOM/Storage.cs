using WebAssembly.Browser.MonsajemDomHelpers;

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
            MyType = type == Type.LocalStorage
                ? "localStorage"
                : type == Type.SessionStorage ? "sessionStorage" : throw new System.Exception("Storage type is not valid!");
        }

        public int Length { get => js.JsEval<int>($"{MyType}.length;"); }
        public string Key(int Position) => js.JsEval<string>($"{MyType}.key({Position});");
        public string GetItem(string Key)
        {
            MonsajemDataTransport.SetJsVar("K", Key);
            var Result = js.JsEval<string>(
                $"{MyType}.getItem({MonsajemDataTransport.ObjectName}.K);");
            MonsajemDataTransport.SetJsVar("K", "");
            return Result;
        }
        public void SetItem(string Key, string Value)
        {
            MonsajemDataTransport.SetJsVar("K", Key);
            MonsajemDataTransport.SetJsVar("V", Value);
            _ = js.JsEval<string>(
                $"{MyType}.setItem({MonsajemDataTransport.ObjectName}.K,{MonsajemDataTransport.ObjectName}.V);");
            MonsajemDataTransport.SetJsVar("K", "");
            MonsajemDataTransport.SetJsVar("V", "");
        }
        public void RemoveItem(string Key)
        {
            MonsajemDataTransport.SetJsVar("K", Key);
            _ = js.JsEval<string>(
                $"{MyType}.removeItem({MonsajemDataTransport.ObjectName}.K);");
            MonsajemDataTransport.SetJsVar("K", "");
        }
        public bool Contains(string Key)
        {
            MonsajemDataTransport.SetJsVar("K", Key);
            var Result = js.JsEval<bool>(
                $"{MyType}.hasOwnProperty({MonsajemDataTransport.ObjectName}.K);") == true;
            MonsajemDataTransport.SetJsVar("K", "");
            return Result;
        }
        public void Clear() => js.JsEval($"{MyType}.clear();");
    }
}
