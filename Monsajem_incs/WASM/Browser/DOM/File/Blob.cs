
using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;
using WebAssembly.Browser.MonsajemDomHelpers;

namespace WebAssembly.Browser.DOM
{
    
    [Export("Blob", typeof(IJSInProcessObjectReference))]
    public class Blob:DOMObject
    {
        public Blob(byte[] Data) :
            this(((System.Func<IJSInProcessObjectReference>)(()=>
            {
                return js.JsNewObject("Blob", Data.JsConvert(), new { type = "application/octet-stream" });
            }))())
        { }
        internal Blob(IJSInProcessObjectReference jSObject) : base(jSObject) { }
    }
}