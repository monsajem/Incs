﻿using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLProgressElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLProgressElement : HTMLElement, IHTMLProgressElement
    {
        internal HTMLProgressElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLProgressElement () { }
        [Export("form")]
        public HTMLFormElement Form => GetProperty<HTMLFormElement>("form");
        [Export("max")]
        public double Max { get => GetProperty<double>("max"); set => SetProperty<double>("max", value); }
        [Export("position")]
        public double Position => GetProperty<double>("position");
        [Export("value")]
        public double NodeValue { get => GetProperty<double>("value"); set => SetProperty<double>("value", value); }
    }

}