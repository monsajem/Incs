using System;
using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;using WebAssembly.Browser.MonsajemDomHelpers;
using WebAssembly.Browser.MonsajemDomHelpers;

namespace WebAssembly.Browser.DOM
{

    public static class MonsajemDataTransport
    {
        public static IJSInProcessObjectReference JsObj = ((Func<IJSInProcessObjectReference>)(() => {
            js.JsEval("self.MonsajemDT = {};");
            return js.JsGetValue("MonsajemDT");
        }))();

        public static string ObjectName { get => "MonsajemDT"; }

        public static void SetJsVar(string VarName,object Data)
        {
            JsObj.JsSetValue(VarName, Data);
        }
        public static object GetJsVar(string VarName)
        {
            return JsObj.JsGetValue<object>(VarName);
        }
    }
}