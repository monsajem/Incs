using System;
using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLHeadElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLHeadElement : HTMLElement
    {
        internal HTMLHeadElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLHeadElement() { }
        [Export("profile")]
        public string Profile { get => GetProperty<string>("profile"); set => SetProperty<string>("profile", value); }
    }
}