using System;
using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("ImageData", typeof(IJSInProcessObjectReference))]
    public sealed class ImageData : DOMObject, IImageData
    {
        internal ImageData(IJSInProcessObjectReference handle) : base(handle) { }

        //public ImageData(double width, double height) { }
        //public ImageData(byte[] array, double width, double height) { }
        [Export("data")]
        public byte[] Data => GetProperty<byte[]>("data");
        [Export("height")]
        public double Height => GetProperty<double>("height");
        [Export("width")]
        public double Width => GetProperty<double>("width");
    }
}