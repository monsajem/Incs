using Microsoft.JSInterop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLCollection", typeof(IJSInProcessObjectReference))]
    public class HTMLCollectionOf<T> : DOMObject, IEnumerable<T>, IEnumerable
    {

        public HTMLCollectionOf(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLCollectionOf() { }

        [Export("length")]
        public double Length => GetProperty<double>("length");
        [Export("namedItem")]
        public T NamedItem(string name)
        {
            return InvokeMethod<T>("namedItem", name);
        }
        [Export("item")]
        public T Item(double index)
        {
            return InvokeMethod<T>("item", index);
        }
        [IndexerName("TheItem")]
        public T this[double index] { get => Item(index); set => throw new NotImplementedException(); }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new HTMLCollectionOf<T>.ElementEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }


        private sealed class ElementEnumerator<U> : IEnumerator<U>, IDisposable, IEnumerator
        {
            private HTMLCollectionOf<U> htmlCollectionCollection;

            private int htmlCollectionIndex;

            private double htmlCollectionCount;

            public U Current
            {
                get
                {
                    return htmlCollectionCollection == null
                        ? throw new ObjectDisposedException("HTMLCollectionEnumerator is disposed")
                        : htmlCollectionCollection[htmlCollectionIndex];
                }
            }

            U IEnumerator<U>.Current
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

            public ElementEnumerator(HTMLCollectionOf<U> collection)
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