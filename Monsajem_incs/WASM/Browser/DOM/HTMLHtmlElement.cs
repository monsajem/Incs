using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLHTMLElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLHTMLElement : HTMLElement, IHTMLHTMLElement
    {
        internal HTMLHTMLElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLHTMLElement() { }
        [Export("version")]
        public string Version { get => GetProperty<string>("version"); set => SetProperty<string>("version", value); }
    }
}