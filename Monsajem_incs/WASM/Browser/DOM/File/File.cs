using Microsoft.JSInterop;
namespace WebAssembly.Browser.DOM
{
    [Export("File", typeof(IJSInProcessObjectReference))]
    public class File : Blob
    {
        internal File(IJSInProcessObjectReference jsObject) : base(jsObject) { }
        [Export("name")]
        public string Name { get => GetProperty<string>("name"); }
    }
}