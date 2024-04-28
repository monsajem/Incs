using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("TextMetrics", typeof(IJSInProcessObjectReference))]
    public sealed class TextMetrics : DOMObject, ITextMetrics
    {
        internal TextMetrics(IJSInProcessObjectReference handle) : base(handle) { }

        //public TextMetrics() { }
        [Export("width")]
        public double Width => GetProperty<double>("width");
    }
}