using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("BarProp", typeof(IJSInProcessObjectReference))]
    public sealed class BarProp : DOMObject
    {
        internal BarProp(IJSInProcessObjectReference handle) : base(handle) { }

        //public BarProp() { }
        [Export("visible")]
        public bool Visible => GetProperty<bool>("visible");
    }
}