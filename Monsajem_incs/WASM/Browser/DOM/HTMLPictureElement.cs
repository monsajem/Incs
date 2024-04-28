using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLPictureElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLPictureElement : HTMLElement
    {
        internal HTMLPictureElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLPictureElement() { }
    }
}