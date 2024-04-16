using System;
using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLLIElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLLIElement : HTMLElement, IHTMLLIElement
    {
        internal HTMLLIElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLLIElement () { }
        [Export("type")]
        public string Type { get => GetProperty<string>("type"); set => SetProperty<string>("type", value); }
        [Export("value")]
        public double NodeValue { get => GetProperty<double>("value"); set => SetProperty<double>("value", value); }
    }
}