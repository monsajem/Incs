using System;
using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;
namespace WebAssembly.Browser.DOM
{
    public enum Direction
    {
        [Export(EnumValue = ConvertEnum.ToLower)]
        Inherit,
        [Export("rtl")]
        RightToLeft,
        [Export("ltr")]
        LeftToRight,
    }
}
