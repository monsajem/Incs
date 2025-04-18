﻿using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM.Events
{

    [Export("UIEvent", typeof(IJSInProcessObjectReference))]
    public class UIEvent : Event
    {
        internal UIEvent(IJSInProcessObjectReference handle) : base(handle) { }

        //        public UIEvent(string typeArg, IUIEventInit eventInitDict) { }
        [Export("detail")]
        public double Detail { get; internal set; }
        [Export("view")]
        public Window View => GetProperty<Window>("view");
        //[Export("initUIEvent")]
        //public void InitUiEvent(string typeArg, bool canBubbleArg, bool cancelableArg, Window viewArg, double detailArg)
        //{
        //    InvokeMethod<object>("initUIEvent", typeArg, canBubbleArg, cancelableArg, viewArg, detailArg);
        //}

    }

}
