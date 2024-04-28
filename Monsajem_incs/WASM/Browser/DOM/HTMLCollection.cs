using Microsoft.JSInterop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLCollection", typeof(IJSInProcessObjectReference))]
    public class HTMLCollection : DOMObject, IEnumerable<Element>, IEnumerable
    {

        internal HTMLCollection(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLCollection() { }
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
        public Element this[double index] { get => Item(index); set => throw new NotImplementedException(); }

        IEnumerator<Element> IEnumerable<Element>.GetEnumerator()
        {
            return new HTMLCollection.ElementEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<DOMObject>)this).GetEnumerator();
        }

        private sealed class ElementEnumerator : IEnumerator<Element>, IDisposable, IEnumerator
        {
            private HTMLCollection htmlCollectionCollection;

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

            public ElementEnumerator(HTMLCollection collection)
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