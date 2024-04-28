using Microsoft.JSInterop;
using System;

namespace WebAssembly.Browser.DOM
{

    [Export("History", typeof(IJSInProcessObjectReference))]
    public sealed class History : DOMObject, IHistory
    {
        public History(IJSInProcessObjectReference handle) : base(handle) { }

        //public History() { }
        [Export("length")]
        public double Length => GetProperty<double>("length");
        [Export("state")]
        public Object State => GetProperty<Object>("state");
        [Export("scrollRestoration")]
        public ScrollRestoration ScrollRestoration { get => GetProperty<ScrollRestoration>("scrollRestoration"); set => SetProperty<ScrollRestoration>("scrollRestoration", value); }
        [Export("back")]
        public void Back()
        {
            _ = InvokeMethod<object>("back");
        }
        [Export("forward")]
        public void Forward()
        {
            _ = InvokeMethod<object>("forward");
        }
        [Export("go")]
        public void Go(double delta)
        {
            _ = InvokeMethod<object>("go", delta);
        }
        [Export("pushState")]
        public void PushState(Object data, string title, string url)
        {
            _ = InvokeMethod<object>("pushState", data, title, url);
        }
        [Export("replaceState")]
        public void ReplaceState(Object data, string title, string url)
        {
            _ = InvokeMethod<object>("replaceState", data, title, url);
        }
    }

}