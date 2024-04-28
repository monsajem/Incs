using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLTitleElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLTitleElement : HTMLElement, IHTMLSpanElement
    {
        internal HTMLTitleElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLTitleElement() { }
        [Export("text")]
        public string Text { get => GetProperty<string>("text"); set => SetProperty<string>("text", value); }
    }
}