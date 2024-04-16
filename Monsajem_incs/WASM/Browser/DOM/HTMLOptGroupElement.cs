using System;
using System.Runtime.InteropServices.JavaScript;using Microsoft.JSInterop.Implementation;using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM 
{

[Export("HTMLOptGroupElement", typeof(IJSInProcessObjectReference))]
public sealed class HTMLOptGroupElement : HTMLElement, IHTMLOptGroupElement {
    internal HTMLOptGroupElement  (IJSInProcessObjectReference handle) : base (handle) {}

    //public HTMLOptGroupElement () { }
    [Export("defaultSelected")]
    public bool DefaultSelected { get => GetProperty<bool>("defaultSelected"); set => SetProperty<bool>("defaultSelected", value); }
    [Export("disabled")]
    public bool Disabled { get => GetProperty<bool>("disabled"); set => SetProperty<bool>("disabled", value); }
    [Export("form")]
    public HTMLFormElement Form => GetProperty<HTMLFormElement>("form");
    [Export("index")]
    public double Index => GetProperty<double>("index");
    [Export("label")]
    public string Label { get => GetProperty<string>("label"); set => SetProperty<string>("label", value); }
    [Export("selected")]
    public bool Selected { get => GetProperty<bool>("selected"); set => SetProperty<bool>("selected", value); }
    [Export("text")]
    public string Text => GetProperty<string>("text");
    [Export("value")]
    public string NodeValue { get => GetProperty<string>("value"); set => SetProperty<string>("value", value); }
}
}