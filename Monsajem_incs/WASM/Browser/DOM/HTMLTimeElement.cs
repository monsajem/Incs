using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLTimeElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLTimeElement : HTMLElement, IHTMLTimeElement
    {
        internal HTMLTimeElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLTimeElement () { }
        [Export("dateTime")]
        public string DateTime { get => GetProperty<string>("dateTime"); set => SetProperty<string>("dateTime", value); }
    }

}