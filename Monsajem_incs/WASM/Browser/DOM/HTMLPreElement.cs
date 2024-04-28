using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLPreElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLPreElement : HTMLElement, IHTMLPreElement
    {
        internal HTMLPreElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLPreElement () { }
        [Export("width")]
        public double Width { get => GetProperty<double>("width"); set => SetProperty<double>("width", value); }
    }
}