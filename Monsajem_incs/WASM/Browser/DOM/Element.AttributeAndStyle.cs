using Microsoft.JSInterop;
using System;

namespace WebAssembly.Browser.DOM
{
    public class ElementAttributes : DOMObject
    {
        public ElementAttributes(IJSInProcessObjectReference handle) : base(handle) { }

        public string this[string qualifiedName]
        {
            get
            {
                return string.IsNullOrEmpty(qualifiedName)
                    ? throw new ArgumentNullException(nameof(qualifiedName))
                    : InvokeMethod<string>("getAttribute", qualifiedName);
            }
            set
            {
                if (string.IsNullOrEmpty(qualifiedName))
                    throw new ArgumentNullException(nameof(qualifiedName));

                _ = InvokeMethod<string>("setAttribute", qualifiedName, value);
            }
        }
    }

    public partial class Element
    {
        // Special Attribute and Style methods
        #region Attribute and Style methods

        public ElementAttributes Attribute { get => new(ManagedJSObject); }

        [Export("getAttribute")]
        public string GetAttribute(string qualifiedName) =>
            Attribute[qualifiedName];

        [Export("setAttribute")]
        public void SetAttribute(string qualifiedName, string value) =>
            Attribute[qualifiedName] = value;

        [Export("removeAttribute")]
        public void RemoveAttribute(string qualifiedName)
        {

            if (string.IsNullOrEmpty(qualifiedName))
                throw new ArgumentNullException(nameof(qualifiedName));

            _ = InvokeMethod<string>("removeAttribute", qualifiedName);
        }

        public void SetStyleAttribute(string qualifiedName, string value)
        {

            if (string.IsNullOrEmpty(qualifiedName))
                throw new ArgumentNullException(nameof(qualifiedName));

            SetJSStyleAttribute(qualifiedName, value);
        }


        public string GetStyleAttribute(string qualifiedName)
        {
            return string.IsNullOrEmpty(qualifiedName)
                ? throw new ArgumentNullException(nameof(qualifiedName))
                : GetJSStyleAttribute(qualifiedName).ToString();
        }


        public void RemoveStyleAttribute(string qualifiedName)
        {
            if (string.IsNullOrEmpty(qualifiedName))
                throw new ArgumentNullException(nameof(qualifiedName));

            SetJSStyleAttribute(qualifiedName, "");

        }


        #endregion
    }
}
