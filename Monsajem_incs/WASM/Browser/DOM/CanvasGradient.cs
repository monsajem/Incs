using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("CanvasGradient", typeof(IJSInProcessObjectReference))]
    public sealed class CanvasGradient : DOMObject, ICanvasGradient
    {
        internal CanvasGradient(IJSInProcessObjectReference handle) : base(handle) { }

        //public CanvasGradient() { }
        [Export("addColorStop")]
        public void AddColorStop(double offset, string color)
        {
            _ = InvokeMethod<object>("addColorStop", offset, color);
        }
    }

}