using System;
using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("TimeRanges", typeof(IJSInProcessObjectReference))]
    public sealed class TimeRanges : DOMObject
    {
        internal TimeRanges(IJSInProcessObjectReference handle) : base(handle) { }

        //public TimeRanges() { }
        [Export("length")]
        public double Length => GetProperty<double>("length");
        [Export("end")]
        public double End(double index)
        {
            return InvokeMethod<double>("end", index);
        }
        [Export("start")]
        public double Start(double index)
        {
            return InvokeMethod<double>("start", index);
        }
    }
}