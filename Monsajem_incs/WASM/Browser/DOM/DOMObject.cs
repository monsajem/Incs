using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Reflection;
using WebAssembly.Browser.MonsajemDomHelpers;
using static Monsajem_Incs.Collection.Array.Extentions;

namespace WebAssembly.Browser.DOM
{
    public abstract class DOMObject : IDisposable
    {
        internal static IJSInProcessObjectReference StaticObject<type>()
        {
            return ExportClassAttribute.GetExportOf<type>().jSObjectStatic;
        }

        internal static System.Collections.Generic.HashSet<DOMObject> objects =
            [];

        bool disposed = false;

        public IJSInProcessObjectReference ManagedJSObject { get; internal set; }

        private Action onRemoved;

        internal void ReadyForManageObject()
        {
            _ = objects.Add(this);
            //onRemoved = () => Console.WriteLine(JSHandle);
            //int last = GetProperty<int>("MNH");
            //if(last!=1)
            //    SetProperty("onRemoved", onRemoved);
            //ManagedJSObject.Invoke<object>("onRemoved");
        }
        public DOMObject(IJSInProcessObjectReference jsObject)
        {
            ManagedJSObject = jsObject;
            ReadyForManageObject();
        }

        public DOMObject(string globalName)
        {
            ManagedJSObject = (IJSInProcessObjectReference)js.JsGetGlobalObject(globalName);
            ReadyForManageObject();
        }

        protected object InvokeMethod(Type type, string methodName, params object[] args)
        {
            if (ManagedJSObject == null)
                throw new Exception("JSObject Is null");
            return ManagedJSObject.InvokeJs(type, methodName, args);
        }

        protected T InvokeMethod<T>(string methodName, params object[] args)
        {
            return (T)InvokeMethod(typeof(T), methodName, args);
        }

        protected T GetProperty<T>(string expr)=>ManagedJSObject.JsGetValue<T>(expr);

        List<object> DGS = [];
        protected void SetProperty<T>(string expr, T Value)
        {
            object value = Value;
            if (value == null)
                ManagedJSObject.JsSetValue(expr, value);
            else
            {
                var valueType = value.GetType();

                if (valueType.IsSubclassOf(typeof(DOMObject)) || valueType == typeof(DOMObject))
                {
                    ManagedJSObject.JsSetValue(expr, ((DOMObject)value).ManagedJSObject);
                }
                else
                    ManagedJSObject.JsSetValue(expr, value);
            }
        }

        private object[] Events = new object[0];
        protected void AddJSEventListener(string eventName, object eventDelegate, int uid)
        {
            ManagedJSObject.InvokeVoid("addEventListener", eventName, eventDelegate, uid);
            Insert(ref Events, eventDelegate);
        }

        protected void SetJSStyleAttribute(string qualifiedName, string value)
        {

            ManagedJSObject.JsGetValue("style").JsSetValue(qualifiedName, value);

        }

        protected string GetJSStyleAttribute(string qualifiedName)
        {
            return ManagedJSObject.JsGetValue("style").JsGetValue<string>(qualifiedName);
        }


        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {

                    // Free any other managed objects here.
                    //
                }

                ManagedJSObject?.Dispose();
                ManagedJSObject = null;

                disposed = true;
            }
        }

        // We are hanging onto JavaScript objects and pointers.
        // Make sure the object sticks around long enough or those
        // same objects may get disposed out from under you.
        ~DOMObject()
        {
            Dispose(false);
        }
    }

}
