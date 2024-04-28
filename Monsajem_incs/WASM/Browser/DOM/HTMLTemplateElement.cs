using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLTemplateElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLTemplateElement : HTMLElement, IHTMLTemplateElement
    {
        internal HTMLTemplateElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLTemplateElement() { }
        [Export("content")]
        public DocumentFragment Content => GetProperty<DocumentFragment>("content");
    }
}