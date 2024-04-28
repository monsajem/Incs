using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLDirectoryElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLDirectoryElement : HTMLElement, IHTMLDirectoryElement
    {
        internal HTMLDirectoryElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLDirectoryElement () { }
        [Export("compact")]
        public bool Compact { get => GetProperty<bool>("compact"); set => SetProperty<bool>("compact", value); }
    }

}