using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLQuoteElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLQuoteElement : HTMLElement, IHTMLQuoteElement
    {
        internal HTMLQuoteElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLQuoteElement() { }
        [Export("cite")]
        public string Cite { get => GetProperty<string>("cite"); set => SetProperty<string>("cite", value); }
    }
}