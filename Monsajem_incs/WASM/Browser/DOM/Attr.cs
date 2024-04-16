using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;
using System;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;
using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("Attr", typeof(IJSInProcessObjectReference))]
    public sealed class Attr : DOMObject
    {
        internal Attr(IJSInProcessObjectReference handle) : base(handle) { }

        //internal Attr() { }

        [Export("name")]
        public string Name => GetProperty<string>("name");
        [Export("ownerElement")]
        public Element OwnerElement => GetProperty<Element>("ownerElement");
        [Export("prefix")]
        public string Prefix => GetProperty<string>("prefix");
        [Export("specified")]
        public bool Specified => GetProperty<bool>("specified");
        [Export("value")]
        public string Value { get => GetProperty<string>("value"); set => SetProperty<string>("value", value); }
    }

}