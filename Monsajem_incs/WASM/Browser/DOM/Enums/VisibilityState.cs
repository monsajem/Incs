using System;

using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{
    public enum VisibilityState
    {
        [Export(EnumValue = ConvertEnum.ToLower)]
        Visible,
        [Export(EnumValue = ConvertEnum.ToLower)]
        Hidden,
        [Export(EnumValue = ConvertEnum.ToLower)]
        PreRender,
        [Export(EnumValue = ConvertEnum.ToLower)]
        UnLoaded,

    }
}
