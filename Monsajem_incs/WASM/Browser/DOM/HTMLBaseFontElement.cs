﻿using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLBaseFontElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLBaseFontElement : HTMLElement, IHTMLFontElement
    {
        internal HTMLBaseFontElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLBaseFontElement() { }
        [Export("face")]
        public string Face { get => GetProperty<string>("face"); set => SetProperty<string>("face", value); }
        [Export("size")]
        public double Size { get => GetProperty<double>("size"); set => SetProperty<double>("size", value); }
        [Export("color")]
        public string Color { get => GetProperty<string>("color"); set => SetProperty<string>("color", value); }
    }
}