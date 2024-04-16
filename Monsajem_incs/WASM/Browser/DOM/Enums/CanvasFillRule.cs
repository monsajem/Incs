using System;
using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;
namespace WebAssembly.Browser.DOM
{
    // "nonzero" | "evenodd";
    public enum CanvasFillRule
    {
        [Export(EnumValue = ConvertEnum.ToLower)]
        NonZero,
        [Export(EnumValue = ConvertEnum.ToLower)]
        EventOdd,
    }
}
