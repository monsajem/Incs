using System;
using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("Comment", typeof(IJSInProcessObjectReference))]
    public sealed class Comment : CharacterData, IComment
    {
        internal Comment(IJSInProcessObjectReference handle) : base(handle) { }

        //public Comment () { }
        [Export("text")]
        public string Text { get => GetProperty<string>("text"); set => SetProperty<string>("text", value); }
    }
}