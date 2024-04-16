using System;
using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM 
{

    [Export("Text", typeof(IJSInProcessObjectReference))]
    public class Text : CharacterData, IText
    {
        internal Text(IJSInProcessObjectReference handle) : base(handle) { }

        //public Text(string data) { }
        [Export("wholeText")]
        public string WholeText => GetProperty<string>("wholeText");
        //[Export("assignedSlot")]
        //public HTMLSlotElement AssignedSlot => GetProperty<HTMLSlotElement>("assignedSlot");
        //[Export("splitText")]
        //public IText SplitText(double offset)
        //{
        //    return InvokeMethod<Text>("splitText", offset);
        //}
    }




}