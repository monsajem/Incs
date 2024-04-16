using System;
using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLSpanElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLSpanElement : HTMLElement, IHTMLSpanElement
    {
        internal HTMLSpanElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLSpanElement () { }
    }
}