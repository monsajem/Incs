using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLMapElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLMapElement : HTMLElement, IHTMLMapElement
    {
        internal HTMLMapElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLMapElement () { }
        [Export("areas")]
        public HTMLAreasCollection Areas => GetProperty<HTMLAreasCollection>("areas");
        [Export("name")]
        public string Name { get => GetProperty<string>("name"); set => SetProperty<string>("name", value); }
    }
}