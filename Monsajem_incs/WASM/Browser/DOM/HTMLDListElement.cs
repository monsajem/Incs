using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLDListElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLDListElement : HTMLElement, IHTMLDListElement
    {
        internal HTMLDListElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLDListElement () { }
        [Export("compact")]
        public bool Compact { get => GetProperty<bool>("compact"); set => SetProperty<bool>("compact", value); }
    }

}