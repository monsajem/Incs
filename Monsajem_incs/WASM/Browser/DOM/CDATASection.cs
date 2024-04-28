using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("CDATASection", typeof(IJSInProcessObjectReference))]
    public sealed class CDATASection : Text
    {
        internal CDATASection(IJSInProcessObjectReference handle) : base(handle) { }

        //public CDATASection() { }
    }
}