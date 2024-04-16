using System;
using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{



    [Export("DocumentFragment", typeof(IJSInProcessObjectReference))]
    public sealed class DocumentFragment : Node
    {
        internal DocumentFragment(IJSInProcessObjectReference handle) : base(handle) { }

        //public DocumentFragment() { }
        [Export("children")]
        public HTMLCollection Children => GetProperty<HTMLCollection>("children");
        [Export("firstElementChild")]
        public Element FirstElementChild => GetProperty<Element>("firstElementChild");
        [Export("lastElementChild")]
        public Element LastElementChild => GetProperty<Element>("lastElementChild");
        [Export("childElementCount")]
        public double ChildElementCount => GetProperty<double>("childElementCount");
        [Export("getElementById")]
        public HTMLElement GetElementById(string elementId)
        {
            return InvokeMethod<HTMLElement>("getElementById", elementId);
        }
    }
}