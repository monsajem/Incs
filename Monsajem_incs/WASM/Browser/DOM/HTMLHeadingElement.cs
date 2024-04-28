using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLHeadingElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLHeadingElement : HTMLElement, IHTMLHeadingElement
    {
        internal HTMLHeadingElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLHeadingElement() { }
        [Export("align")]
        public string Align { get => GetProperty<string>("align"); set => SetProperty<string>("align", value); }
    }

}