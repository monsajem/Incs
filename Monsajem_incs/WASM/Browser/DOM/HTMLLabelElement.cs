using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLLabelElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLLabelElement : HTMLElement, IHTMLLabelElement
    {
        internal HTMLLabelElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLLabelElement() { }
        [Export("form")]
        public HTMLFormElement Form => GetProperty<HTMLFormElement>("form");
        [Export("htmlFor")]
        public string HtmlFor { get => GetProperty<string>("htmlFor"); set => SetProperty<string>("htmlFor", value); }
        [Export("control")]
        public HTMLInputElement Control => GetProperty<HTMLInputElement>("control");
    }

}