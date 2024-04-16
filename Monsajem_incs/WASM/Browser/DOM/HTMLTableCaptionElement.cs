using System;
using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLTableCaptionElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLTableCaptionElement : HTMLElement, IHTMLTableCaptionElement
    {
        internal HTMLTableCaptionElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLTableCaptionElement () { }
        [Export("align")]
        public string Align { get => GetProperty<string>("align"); set => SetProperty<string>("align", value); }
        [Export("vAlign")]
        public string VAlign { get => GetProperty<string>("vAlign"); set => SetProperty<string>("vAlign", value); }
    }
}