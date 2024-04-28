using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("CanvasPattern", typeof(IJSInProcessObjectReference))]
    public sealed class CanvasPattern : DOMObject, ICanvasPattern
    {
        internal CanvasPattern(IJSInProcessObjectReference handle) : base(handle) { }

        //public CanvasPattern() { }
        //[Export("setTransform")]
        //public void SetTransform(SVGMatrix matrix)
        //{
        //    InvokeMethod<object>("setTransform", matrix);
        //}
    }
}