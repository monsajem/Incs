﻿using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM.Events
{
    [Export("MouseEvent", typeof(IJSInProcessObjectReference))]
    public class MouseEvent : UIEvent
    {
        internal MouseEvent(IJSInProcessObjectReference handle) : base(handle) { }

        //public MouseEvent(string typeArg, IMouseEventInit eventInitDict) { }
        [Export("altKey")]
        public bool AltKey { get; internal set; }
        [Export("button")]
        public double Button { get; internal set; }
        [Export("buttons")]
        public double Buttons { get; internal set; }
        [Export("clientX")]
        public double ClientX { get; internal set; }
        [Export("clientY")]
        public double ClientY { get; internal set; }
        [Export("ctrlKey")]
        public bool CtrlKey { get; internal set; }
        [Export("fromElement")]
        public Element FromElement => GetProperty<Element>("fromElement");
        [Export("layerX")]
        public double LayerX { get; internal set; }
        [Export("layerY")]
        public double LayerY { get; internal set; }
        [Export("metaKey")]
        public bool MetaKey { get; internal set; }
        [Export("movementX")]
        public double MovementX { get; internal set; }
        [Export("movementY")]
        public double MovementY { get; internal set; }
        [Export("offsetX")]
        public double OffsetX { get; internal set; }
        [Export("offsetY")]
        public double OffsetY { get; internal set; }
        [Export("pageX")]
        public double PageX { get; internal set; }
        [Export("pageY")]
        public double PageY { get; internal set; }
        [Export("relatedTarget")]
        public EventTarget RelatedTarget => GetProperty<EventTarget>("relatedTarget");
        [Export("screenX")]
        public double ScreenX { get; internal set; }
        [Export("screenY")]
        public double ScreenY { get; internal set; }
        [Export("shiftKey")]
        public bool ShiftKey { get; internal set; }
        [Export("toElement")]
        public Element ToElement => GetProperty<Element>("toElement");
        [Export("which")]
        public double Which { get; internal set; }
        [Export("x")]
        public double X { get; internal set; }
        [Export("y")]
        public double Y { get; internal set; }
        [Export("getModifierState")]
        public bool GetModifierState(string keyArg)
        {
            return InvokeMethod<bool>("getModifierState", keyArg);
        }
        //[Export("initMouseEvent")]
        //public void InitMouseEvent(string typeArg, bool canBubbleArg, bool cancelableArg, Window viewArg, double detailArg, double screenXArg, double screenYArg, double clientXArg, double clientYArg, bool ctrlKeyArg, bool altKeyArg, bool shiftKeyArg, bool metaKeyArg, double buttonArg, EventTarget relatedTargetArg)
        //{
        //    InvokeMethod<object>("initMouseEvent", typeArg, canBubbleArg, cancelableArg, viewArg, detailArg, screenXArg, screenYArg, clientXArg, clientYArg, ctrlKeyArg, altKeyArg, shiftKeyArg, metaKeyArg, buttonArg, relatedTargetArg);
        //}
    }

}
