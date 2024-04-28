using Microsoft.JSInterop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLFormControlsCollection", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLFormControlsCollection : DOMObject, IEnumerable<Element>, IEnumerable
    {
        internal HTMLFormControlsCollection(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLFormControlsCollection() { }
        [Export("length")]
        public double Length => GetProperty<double>("length");
        [Export("namedItem")]
        public Element NamedItem(string name)
        {
            return InvokeMethod<Element>("namedItem", name);
        }
        [Export("item")]
        public Element Item(double index)
        {
            return InvokeMethod<Element>("item", index);
        }
        [IndexerName("TheItem")]
        public Element this[double index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        IEnumerator<Element> IEnumerable<Element>.GetEnumerator()
        {
            return new HTMLFormControlsCollection.ElementEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<DOMObject>)this).GetEnumerator();
        }


        private sealed class ElementEnumerator : IEnumerator<Element>, IDisposable, IEnumerator
        {
            private HTMLFormControlsCollection htmlCollectionCollection;

            private int htmlCollectionIndex;

            private double htmlCollectionCount;

            public Element Current
            {
                get
                {
                    return htmlCollectionCollection == null
                        ? throw new ObjectDisposedException("HTMLCollectionEnumerator is disposed")
                        : htmlCollectionCollection[htmlCollectionIndex];
                }
            }

            Element IEnumerator<Element>.Current
            {
                get
                {
                    return Current;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            public ElementEnumerator(HTMLFormControlsCollection collection)
            {
                htmlCollectionCollection = collection;
                htmlCollectionCount = htmlCollectionCollection.Length;
                htmlCollectionIndex = -1;
            }

            void IDisposable.Dispose()
            {
                htmlCollectionCollection = null;
            }

            bool IEnumerator.MoveNext()
            {
                htmlCollectionIndex++;
                return htmlCollectionIndex < htmlCollectionCount;
            }

            void IEnumerator.Reset()
            {
                htmlCollectionIndex = -1;
            }
        }
    }


}