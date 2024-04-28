using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLModElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLModElement : HTMLElement, IHTMLModElement
    {
        internal HTMLModElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLModElement () { }
        [Export("cite")]
        public string Cite { get => GetProperty<string>("cite"); set => SetProperty<string>("cite", value); }
        [Export("dateTime")]
        public string DateTime { get => GetProperty<string>("dateTime"); set => SetProperty<string>("dateTime", value); }
    }
}