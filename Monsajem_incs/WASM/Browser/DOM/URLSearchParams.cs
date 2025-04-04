﻿using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("URLSearchParams", typeof(IJSInProcessObjectReference))]
    public sealed class URLSearchParams : DOMObject
    {
        internal URLSearchParams(IJSInProcessObjectReference handle) : base(handle) { }

        //public URLSearchParams (object init) { }
        [Export("append")]
        public void Append(string name, string value)
        {
            _ = InvokeMethod<object>("append", name, value);
        }
        [Export("delete")]
        public void Delete(string name)
        {
            _ = InvokeMethod<object>("delete", name);
        }
        [Export("get")]
        public string Get(string name)
        {
            return InvokeMethod<
        string>("get", name);
        }
        [Export("getAll")]
        public string[] GetAll(string name)
        {
            return InvokeMethod<string[]>("getAll", name);
        }
        [Export("has")]
        public bool Has(string name)
        {
            return InvokeMethod<bool>("has", name);
        }
        [Export("set")]
        public void Set(string name, string value)
        {
            _ = InvokeMethod<object>("set", name, value);
        }
    }

}