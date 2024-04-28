using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLUListElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLUListElement : HTMLElement, IHTMLUListElement
    {
        internal HTMLUListElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLUListElement() { }
        [Export("compact")]
        public bool Compact { get => GetProperty<bool>("compact"); set => SetProperty<bool>("compact", value); }
        [Export("type")]
        public string Type { get => GetProperty<string>("type"); set => SetProperty<string>("type", value); }
    }

}