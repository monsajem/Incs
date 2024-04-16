using System;
using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;
namespace WebAssembly.Browser.DOM
{
    public enum LineJoin
    {
        [Export(EnumValue = ConvertEnum.ToLower)]
        Bevel,
        [Export(EnumValue = ConvertEnum.ToLower)]
        Round,
        [Export(EnumValue = ConvertEnum.ToLower)]
        Miter,
    }
}
