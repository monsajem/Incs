﻿using Microsoft.JSInterop;
using System;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLTextAreaElement", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLTextAreaElement : HTMLElement, IHTMLTextAreaElement
    {
        internal HTMLTextAreaElement(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLTextAreaElement() { }
        [Export("autofocus")]
        public bool Autofocus { get => GetProperty<bool>("autofocus"); set => SetProperty<bool>("autofocus", value); }
        [Export("cols")]
        public double Cols { get => GetProperty<double>("cols"); set => SetProperty<double>("cols", value); }
        [Export("defaultNodeValue")]
        public string DefaultNodeValue { get => GetProperty<string>("defaultNodeValue"); set => SetProperty<string>("defaultNodeValue", value); }
        [Export("disabled")]
        public bool Disabled { get => GetProperty<bool>("disabled"); set => SetProperty<bool>("disabled", value); }
        [Export("form")]
        public HTMLFormElement Form => GetProperty<HTMLFormElement>("form");
        [Export("maxLength")]
        public double MaxLength { get => GetProperty<double>("maxLength"); set => SetProperty<double>("maxLength", value); }
        [Export("name")]
        public string Name { get => GetProperty<string>("name"); set => SetProperty<string>("name", value); }
        [Export("placeholder")]
        public string Placeholder { get => GetProperty<string>("placeholder"); set => SetProperty<string>("placeholder", value); }
        [Export("readOnly")]
        public bool ReadOnly { get => GetProperty<bool>("readOnly"); set => SetProperty<bool>("readOnly", value); }
        [Export("required")]
        public bool Required { get => GetProperty<bool>("required"); set => SetProperty<bool>("required", value); }
        [Export("rows")]
        public double Rows { get => GetProperty<double>("rows"); set => SetProperty<double>("rows", value); }
        [Export("selectionEnd")]
        public double SelectionEnd { get => GetProperty<double>("selectionEnd"); set => SetProperty<double>("selectionEnd", value); }
        [Export("selectionStart")]
        public double SelectionStart { get => GetProperty<double>("selectionStart"); set => SetProperty<double>("selectionStart", value); }
        [Export("status")]
        public Object Status { get => GetProperty<Object>("status"); set => SetProperty<Object>("status", value); }
        [Export("type")]
        public string Type => GetProperty<string>("type");
        [Export("validationMessage")]
        public string ValidationMessage => GetProperty<string>("validationMessage");
        [Export("validity")]
        public ValidityState Validity => GetProperty<ValidityState>("validity");
        [Export("value")]
        public string NodeValue { get => GetProperty<string>("value"); set => SetProperty<string>("value", value); }
        [Export("willValidate")]
        public bool WillValidate => GetProperty<bool>("willValidate");
        [Export("wrap")]
        public string Wrap { get => GetProperty<string>("wrap"); set => SetProperty<string>("wrap", value); }
        [Export("minLength")]
        public double MinLength { get => GetProperty<double>("minLength"); set => SetProperty<double>("minLength", value); }
        [Export("checkValidity")]
        public bool CheckValidity()
        {
            return InvokeMethod<bool>("checkValidity");
        }
        [Export("select")]
        public void Select()
        {
            _ = InvokeMethod<object>("select");
        }
        [Export("setCustomValidity")]
        public void SetCustomValidity(string error)
        {
            _ = InvokeMethod<object>("setCustomValidity", error);
        }
        [Export("setSelectionRange")]
        public void SetSelectionRange(double start, double end, object direction)
        {
            _ = InvokeMethod<object>("setSelectionRange", start, end, direction);
        }
    }
}