using System;
using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLDivElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLDivElement : HTMLElement, IHTMLDivElement
    {
        //internal HTMLDivElement(IJSInProcessObjectReference handle) : base(handle) { }
        internal HTMLDivElement(IJSInProcessObjectReference jsObject) : base(jsObject) { }
        //public HTMLDivElement() { }
        [Export("align")]
        public string Align { get => GetProperty<string>("align"); set => SetProperty<string>("align", value); }
        [Export("noWrap")]
        public bool NoWrap { get => GetProperty<bool>("noWrap"); set => SetProperty<bool>("noWrap", value); }
    }
}