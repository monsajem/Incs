using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;
using System;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;
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
            InvokeMethod<object>("addColorStop", offset, color);
        }
    }

}