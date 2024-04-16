using System;

using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{
    //type InsertPosition = "beforebegin" | "afterbegin" | "beforeend" | "afterend";
    public enum InsertPosition
    {
        [Export(EnumValue = ConvertEnum.ToLower)]
        BeforeBegin,
        [Export(EnumValue = ConvertEnum.ToLower)]
        AfterBegin,
        [Export(EnumValue = ConvertEnum.ToLower)]
        BeforeEnd,
        [Export(EnumValue = ConvertEnum.ToLower)]
        AfterEnd,
    }
}
