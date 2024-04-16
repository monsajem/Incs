﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;
using WebAssembly.Browser.DOM.Events;
using Monsajem_Incs.Serialization;
using System.Linq;


namespace WebAssembly.Browser.DOM
{


    [Export("EventTarget", typeof(IJSInProcessObjectReference))]
    public class EventTarget : DOMObject, IEventTarget
    {

        static int nextEventId = 0;
        static int NextEventId => nextEventId++;

        internal EventTarget(IJSInProcessObjectReference handle) : base(handle) { }

        protected EventTarget(string globalName) : base(globalName)
        {

        }

        internal Dictionary<string, DOMEventHandler> eventHandlers = new Dictionary<string, DOMEventHandler>();


        [Export("addEventListener")]
        public void AddEventListener(string type, DOMEventHandler listener, object options)
        {
            bool addNativeEventListener = false;
            lock (eventHandlers)
            {
                if (!eventHandlers.ContainsKey(type))
                {
                    eventHandlers.Add(type, null);
                    addNativeEventListener = true;
                }
                eventHandlers[type] += listener;
            }

            if (addNativeEventListener)
            {

                var UID = NextEventId;
                AddJSEventListener(type, dispather(type), UID);

            }


        }

        public delegate int DOMEventDelegate(IJSInProcessObjectReference eventTarget);


        DOMEventDelegate dispather(string type)
        {
            return (eventTarget) => DispatchDOMEvent(type, eventTarget);
        }

        [Export("dispatchEvent")]
        public bool DispatchEvent(Event evt)
        {
            return InvokeMethod<bool>("dispatchEvent", evt);
        }

        [Export("removeEventListener")]
        public void RemoveEventListener(string type, DOMEventHandler listener, object options)
        {

        }

        public int DispatchDOMEvent(string typeOfEvent, IJSInProcessObjectReference eventTarget)
        {

            var eventArgs = new DOMEventArgs(this, typeOfEvent, eventTarget);


            lock (eventHandlers)
            {
                if (eventHandlers.TryGetValue(typeOfEvent, out DOMEventHandler eventHandler))
                {
                    eventHandler?.Invoke(this, eventArgs);
                }
            }

            eventArgs.EventObject?.Dispose();
            eventArgs.EventObject = null;
            eventArgs.Source = null;
            eventArgs = null;
            return 0;
        }

        //protected internal override object ConvertTo(Type targetType)
        //{

        //    if (targetType.IsAssignableFrom(base.GetType()))
        //    {
        //        return this;
        //    }
        //    else if (targetType.IsSubclassOf(this.GetType()))
        //    {
        //        return CreateJSObjectFrom(targetType, this);
        //    }

        //    return base.ConvertTo(targetType);
        //}
    }

}