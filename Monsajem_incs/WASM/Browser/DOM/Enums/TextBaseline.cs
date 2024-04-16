using System;
using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;
namespace WebAssembly.Browser.DOM
{
    public enum TextBaseline
    {
        [Export(EnumValue = ConvertEnum.ToLower)]
        Alphabetic,
        [Export(EnumValue = ConvertEnum.ToLower)]
        Top,
        [Export(EnumValue = ConvertEnum.ToLower)]
        Hanging,
        [Export(EnumValue = ConvertEnum.ToLower)]
        Middle,
        [Export(EnumValue = ConvertEnum.ToLower)]
        Ideographic,
        [Export(EnumValue = ConvertEnum.ToLower)]
        Bottom,
    }
}
