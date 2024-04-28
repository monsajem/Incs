using Microsoft.JSInterop;
using System;
using System.Runtime.CompilerServices;

namespace WebAssembly.Browser.DOM
{
    [Export("DOMTokenList", typeof(IJSInProcessObjectReference))]
    public class DOMTokenList : DOMObject
    {
        public DOMTokenList(IJSInProcessObjectReference handle) : base(handle) { }

        //public DOMTokenList() { }
        [Export("length")]
        public double Length => GetProperty<double>("length");
        [Export("add")]
        public void Add(params string[] token)
        {
            _ = InvokeMethod<object>("add", token);
        }
        [Export("contains")]
        public bool Contains(string token)
        {
            return InvokeMethod<bool>("contains", token);
        }
        [Export("item")]
        public string Item(double index)
        {
            return InvokeMethod<string>("item", index);
        }
        [Export("remove")]
        public void Remove(params string[] token)
        {
            _ = InvokeMethod<object>("remove", token);
        }
        [Export("toggle")]
        public bool Toggle(string token, bool force)
        {
            return InvokeMethod<bool>("toggle", token, force);
        }
        [Export("toggle")]
        public bool Toggle(string token)
        {
            return InvokeMethod<bool>("toggle", token);
        }
        [Export("toString")]
        public override string ToString()
        {
            return InvokeMethod<string>("toString");
        }
        [IndexerName("TheItem")]
        public string this[double index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }

}