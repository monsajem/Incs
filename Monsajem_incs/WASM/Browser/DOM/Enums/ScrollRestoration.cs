using System;

using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{
    public enum ScrollRestoration
    {
        [Export(EnumValue = ConvertEnum.ToLower)]
        Auto,
        [Export(EnumValue = ConvertEnum.ToLower)]
        Manual
    }
}
