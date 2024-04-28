using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLBaseElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLBaseElement : HTMLElement, IHTMLBaseElement
    {
        internal HTMLBaseElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLBaseElement() { }
        [Export("href")]
        public string Href { get => GetProperty<string>("href"); set => SetProperty<string>("href", value); }
        [Export("target")]
        public string Target { get => GetProperty<string>("target"); set => SetProperty<string>("target", value); }
    }
}