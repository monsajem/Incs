using System;
using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLTableHeaderCellElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLTableHeaderCellElement : HTMLTableCellElement
    {
        internal HTMLTableHeaderCellElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLTableHeaderCellElement () { }
        //[Export("scope")]
        //public string Scope { get => GetProperty<string>("scope"); set => SetProperty<string>("scope", value); }
    }
}