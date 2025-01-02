using Microsoft.JSInterop;
using System;
using WebAssembly.Browser.MonsajemDomHelpers;

namespace WebAssembly.Browser.DOM
{

    public static class MonsajemDataTransport
    {
        public static IJSInProcessObjectReference JsObj = ((Func<IJSInProcessObjectReference>)(() =>
        {
            js.JsEvalGlobal("self.MonsajemDT = {};");
            return js.JsGetValue("MonsajemDT");
        }))();

        public static string ObjectName { get => "MonsajemDT"; }

        public static void SetJsVar(string VarName, object Data)
        {
            JsObj.JsSetValue(VarName, Data);
        }
        public static object GetJsVar(string VarName)
        {
            return JsObj.JsGetValue<object>(VarName);
        }
    }
}