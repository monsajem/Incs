using System;
using System.Reflection;
using Monsajem_Incs.Collection.Array.ArrayBased.DynamicSize;
using Microsoft.JSInterop.Implementation;
using Microsoft.JSInterop;
using static Monsajem_Incs.Collection.Array.Extentions;
using WebAssembly.Browser.MonsajemDomHelpers;
using Microsoft.JSInterop;
using System.Collections.Generic;

namespace WebAssembly.Browser.DOM
{
    public abstract class DOMObject : IDisposable
    {
        internal static IJSInProcessObjectReference StaticObject<type>()
        {
            return ExportClassAttribute.GetExportOf<type>().jSObjectStatic;
        }

        internal static System.Collections.Generic.HashSet<DOMObject> objects =
            new System.Collections.Generic.HashSet<DOMObject>();

        bool disposed = false;

        public IJSInProcessObjectReference ManagedJSObject { get; private set; }

        private Action onRemoved;

        internal void ReadyForManageObject()
        {
            objects.Add(this);
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
            if (args != null && args.Length > 0)
            {
                Type argType = null;
                // All DOMObjects will need to pass the IJSInProcessObjectReference that they are associated with
                for (int a = 0; a < args.Length; a++)
                {
                    argType = args[a].GetType();
                    if (argType.IsSubclassOf(typeof(DOMObject)) || argType == typeof(DOMObject))
                    {
                        args[a] = ((DOMObject)args[a]).ManagedJSObject;
                    }
                }
            }
            if (ManagedJSObject == null)
                throw new Exception("JSObject Is null");
            var res = ManagedJSObject.Invoke<object>(methodName, args);
            return UnWrapObject(type, res);
        }

        protected T InvokeMethod<T>(string methodName, params object[] args)
        {
            return (T)InvokeMethod(typeof(T), methodName, args);
        }

        protected T GetProperty<T>(string expr)
        {

            var type = typeof(T);
            object propertyNodeValue = ManagedJSObject.JsGetValue<object>(expr);
            if (type.IsSubclassOf(typeof(DOMObject)) || type == typeof(DOMObject))
                propertyNodeValue = ManagedJSObject.JsGetValue<IJSInProcessObjectReference>(expr);
            else
                propertyNodeValue = ManagedJSObject.JsGetValue<T>(expr);

            if (propertyNodeValue == null)
                return default;
            if (typeof(T) == typeof(object))
                return (T)propertyNodeValue;
            if(typeof(T) == propertyNodeValue.GetType())
                return (T)propertyNodeValue;
            return UnWrapObject<T>(propertyNodeValue);
        }

        List<object> DGS = new List<object>();
        protected void SetProperty<T>(string expr, T Value)
        {
            object value = Value;
            if (Value is Delegate)
            {
                value = DotNetObjectReference.Create((Delegate)value);
                DGS.Add(value);
            }
            if (value == null)
                ManagedJSObject.JsSetValue(expr, value);
            else
            {
                var valueType = value.GetType();

                if (valueType.IsSubclassOf(typeof(DOMObject)) || valueType == typeof(DOMObject))
                {
                    ManagedJSObject.JsSetValue(expr, ((DOMObject)(object)value).ManagedJSObject);
                }
                else
                    ManagedJSObject.JsSetValue(expr, value);
            }
        }

        object UnWrapObject(Type type, object obj)
        {
            if (type.IsSubclassOf(typeof(IJSInProcessObjectReference)) || type == typeof(IJSInProcessObjectReference))
            {


                var jsobjectconstructor = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                                null, new Type[] { typeof(Int32) }, null);

                var jsobjectnew = jsobjectconstructor.Invoke(new object[] { (obj == null) ? -1 : obj });
                return jsobjectnew;

            }
            else if (type.IsSubclassOf(typeof(DOMObject)) || type == typeof(DOMObject))
            {


                var jsobjectconstructor = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                                null, new Type[] { typeof(IJSInProcessObjectReference) }, null);

                //var jsobjectnew = jsobjectconstructor.Invoke<object>(new object[] { obj });
                return jsobjectconstructor.Invoke(new object[] { obj }); ;

            }
            else if (type.IsPrimitive || typeof(Decimal) == type)
            {

                // Make sure we handle null and undefined
                // have found this only on FireFox for now
                if (obj == null)
                {
                    return Activator.CreateInstance(type);
                }

                return Convert.ChangeType(obj, type);
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {

                var conv = System.ComponentModel.TypeDescriptor.GetConverter(type);

                if (!conv.CanConvertFrom(obj.GetType()))
                {
                    throw new NotSupportedException();
                }

                if (conv.IsValid(obj))
                {
                    return conv.ConvertFrom(obj);
                }

                throw new InvalidCastException();
            }
            else if (type.IsEnum)
            {
                return obj;
                //return Runtime.EnumFromExportContract(type, obj);
            }
            else if (type == typeof(string))
            {
                return obj;
            }
            else if (type is object)
            {
                // called via invoke
                if (obj == null)
                    return (object)null;
                else
                    throw new NotSupportedException($"Type {type} not supported yet.");

            }
            else
            {
                throw new NotSupportedException($"Type {type} not supported yet.");
            }


        }

        T UnWrapObject<T>(object obj)
        {

            return (T)UnWrapObject(typeof(T), obj);
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
