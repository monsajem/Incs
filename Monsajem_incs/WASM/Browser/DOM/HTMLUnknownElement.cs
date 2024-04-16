using System;
using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLUnknownElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLUnknownElement : HTMLElement
    {
        internal HTMLUnknownElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLUnknownElement () { }
    }
}