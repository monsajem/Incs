using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLDataElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLDataElement : HTMLElement, IHTMLDataElement
    {
        internal HTMLDataElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLDataElement() { }
        [Export("value")]
        public string NodeValue { get => GetProperty<string>("value"); set => SetProperty<string>("value", value); }
    }

}