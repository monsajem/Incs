using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("WebKitPoint", typeof(IJSInProcessObjectReference))]
    public sealed class WebKitPoint : DOMObject
    {
        public WebKitPoint(IJSInProcessObjectReference handle) : base(handle) { }

        //public WebKitPoint(double x, double y) { }
        [Export("x")]
        public double X { get => GetProperty<double>("x"); set => SetProperty<double>("x", value); }
        [Export("y")]
        public double Y { get => GetProperty<double>("y"); set => SetProperty<double>("y", value); }
    }
}