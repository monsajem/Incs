using System;
using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLDataListElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLDataListElement : HTMLElement, IHTMLDataListElement
    {
        internal HTMLDataListElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLDataListElement () { }
        [Export("options")]
        public HTMLCollectionOf<HTMLOptionElement> Options { get => GetProperty<HTMLCollectionOf<HTMLOptionElement>>("options"); set => SetProperty<HTMLCollectionOf<HTMLOptionElement>>("options", value); }
    }

}