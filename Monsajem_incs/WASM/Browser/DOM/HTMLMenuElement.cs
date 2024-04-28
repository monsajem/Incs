using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLMenuElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLMenuElement : HTMLElement, IHTMLMenuElement
    {
        internal HTMLMenuElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLMenuElement () { }
        [Export("compact")]
        public bool Compact { get => GetProperty<bool>("compact"); set => SetProperty<bool>("compact", value); }
        [Export("type")]
        public string Type { get => GetProperty<string>("type"); set => SetProperty<string>("type", value); }
    }
}