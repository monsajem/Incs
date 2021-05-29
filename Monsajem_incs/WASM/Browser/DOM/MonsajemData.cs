using System;
using System.Runtime.InteropServices.JavaScript;

namespace WebAssembly.Browser.DOM
{

    public static class MonsajemDataTransport
    {
        public static JSObject JsObj = ((Func<JSObject>)(() => {
            Runtime.InvokeJS("self.MonsajemDT = {};");
            return (JSObject) Runtime.GetGlobalObject("MonsajemDT");
        }))();

        public static string ObjectName { get => "MonsajemDT"; }

        public static void SetJsVar(string VarName,object Data)
        {
            JsObj.SetObjectProperty("VarName",VarName);
        }
        public static object GetJsVar(string VarName)
        {
            return JsObj.GetObjectProperty(VarName);
        }
    }
}