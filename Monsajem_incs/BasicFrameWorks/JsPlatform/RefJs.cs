using System;
using System.Linq;
using System.Text;
using WebAssembly.Browser.MonsajemDomHelpers;
using WebAssembly.Browser.DOM;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.JSInterop;

namespace Monsajem_Incs.JsPlatform
{
    public static class JsRumtime
    {
        public static Action<string> RunJs;
        public static Func<string, string> ResultJs;
    }
    public class RefJs
    {
        private static UInt32 IDs;
        private string ID;
        public void InvokeMethod(string Method,params RefJs[] Values)
        {
            var len = Values.Length-1;
            var js = new StringBuilder();
            js.Append($"{ID}.{Method}(");
            for (int i = 0; i < len; i++)
                js.Append($"{Values[i].ID},");
            if(len>-1)
                js.Append($"{Values[len].ID});");
            JsRumtime.RunJs(js.ToString());
        }

        public string InvokeMethodResult(string Method, params RefJs[] Values)
        {
            var len = Values.Length - 1;
            var js = new StringBuilder();
            js.Append($"{ID}.{Method}(");
            for (int i = 0; i < len; i++)
                js.Append($"{Values[i].ID},");
            if (len > -1)
                js.Append($"{Values[len].ID});");
            return JsRumtime.ResultJs(js.ToString());
        }

        public static implicit operator RefJs(int Value)
        {
            return new RefJs() { ID = Value.ToString() };
        }
        public static implicit operator RefJs(byte Value)
        {
            return new RefJs() { ID = Value.ToString() };
        }
    }
}