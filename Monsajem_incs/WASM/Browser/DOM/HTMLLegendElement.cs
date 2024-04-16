using System;
using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLLegendElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLLegendElement : HTMLElement, IHTMLLegendElement
    {
        internal HTMLLegendElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLLegendElement () { }
        [Export("align")]
        public string Align { get => GetProperty<string>("align"); set => SetProperty<string>("align", value); }
        [Export("form")]
        public HTMLFormElement Form => GetProperty<HTMLFormElement>("form");
    }
}