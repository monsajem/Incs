using System;
using System.Collections.Generic;
using WebAssembly.Browser.DOM.Events;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.JSInterop.Implementation;
using Microsoft.JSInterop;
using WebAssembly.Browser.MonsajemDomHelpers;

namespace WebAssembly.Browser.DOM
{
    public class DOMEventArgs : EventArgs
    {

        public int ClientX { get => Me.JsGetValue<int>("clientX"); }
        public int ClientY { get => Me.JsGetValue<int>("clientY"); }
        public int OffsetX { get => Me.JsGetValue<int>("offsetX"); }
        public int OffsetY { get => Me.JsGetValue<int>("offsety"); }
        public int ScreenX { get => Me.JsGetValue<int>("screenX"); }
        public int ScreenY { get => Me.JsGetValue<int>("Screeny"); }
        public bool AltKey { get => throw new NotImplementedException("Please Declare this!"); }
        public bool CtrlKey { get => throw new NotImplementedException("Please Declare this!"); }
        public bool ShiftKey { get => throw new NotImplementedException("Please Declare this!"); }
        public int KeyCode { get => throw new NotImplementedException("Please Declare this!"); }
        public string EventType { get; internal set; }
        public DOMObject Source { get; internal set; }
        public Event EventObject { get; internal set; }
        private IJSInProcessObjectReference Me;

        public DOMEventArgs(DOMObject source, string typeOfEvent, IJSInProcessObjectReference eventObject)
        {

            Me = eventObject;
            Source = source;
            EventType = typeOfEvent;

            switch (typeOfEvent)
            {
                case "MouseEvent":
                    EventObject = new MouseEvent(eventObject);
                    break;
                case "DragEvent":
                    EventObject = new DragEvent(eventObject);
                    break;
                case "FocusEvent":
                    EventObject = new FocusEvent(eventObject);
                    break;
                case "WheelEvent":
                    EventObject = new WheelEvent(eventObject);
                    break;
                case "KeyboardEvent":
                    EventObject = new KeyboardEvent(eventObject);
                    break;
                case "ClipboardEvent":
                    EventObject = new ClipboardEvent(eventObject);
                    break;
                default:
                    EventObject = new Event(eventObject);
                    break;
            }
        }

        public void PreventDefault()
        {
            if (EventObject != null)
                EventObject.PreventDefault();
        }

        public void StopPropagation()
        {
            if (EventObject != null)
                EventObject.StopPropagation();
        }

    }
}
