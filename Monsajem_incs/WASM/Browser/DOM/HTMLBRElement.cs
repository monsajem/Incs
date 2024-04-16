using System;
using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLBRElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLBRElement : HTMLElement, IHTMLBRElement
    {
        internal HTMLBRElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLBRElement() { }
        [Export("clear")]
        public string Clear { get => GetProperty<string>("clear"); set => SetProperty<string>("clear", value); }
    }
}