using System;
using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("MSGraphicsTrust", typeof(IJSInProcessObjectReference))]
    public sealed class MSGraphicsTrust : DOMObject
    {
        internal MSGraphicsTrust(IJSInProcessObjectReference handle) : base(handle) { }

        //public MSGraphicsTrust() { }
        [Export("constrictionActive")]
        public bool ConstrictionActive => GetProperty<bool>("constrictionActive");
        [Export("status")]
        public string Status => GetProperty<string>("status");
    }
}