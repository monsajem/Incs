using Microsoft.JSInterop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WebAssembly.Browser.DOM
{

    [Export("HTMLAreasCollection", typeof(IJSInProcessObjectReference))]
    public sealed class HTMLAreasCollection : DOMObject, IEnumerable<HTMLAreaElement>, IEnumerable
    {
        internal HTMLAreasCollection(IJSInProcessObjectReference handle) : base(handle) { }

        //public HTMLAreasCollection() { }
        [Export("length")]
        public double Length => GetProperty<double>("length");
        [Export("item")]
        public HTMLAreaElement Item(double index)
        {
            return InvokeMethod<HTMLAreaElement>("item", index);
        }
        [IndexerName("TheItem")]
        public HTMLAreaElement this[double index] { get => Item(index); set => throw new NotImplementedException(); }

        IEnumerator<HTMLAreaElement> IEnumerable<HTMLAreaElement>.GetEnumerator()
        {
            return new HTMLAreasCollection.AreaEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<DOMObject>)this).GetEnumerator();
        }


        private sealed class AreaEnumerator : IEnumerator<HTMLAreaElement>, IDisposable, IEnumerator
        {
            private HTMLAreasCollection areasCollection;

            private int areaCollectionIndex;

            private double areaCollectionCount;

            public HTMLAreaElement Current
            {
                get
                {
                    return areasCollection == null
                        ? throw new ObjectDisposedException("HTMLAreasCollectionEnumerator is disposed")
                        : areasCollection[areaCollectionIndex];
                }
            }

            HTMLAreaElement IEnumerator<HTMLAreaElement>.Current
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

            public AreaEnumerator(HTMLAreasCollection collection)
            {
                areasCollection = collection;
                areaCollectionCount = areasCollection.Length;
                areaCollectionIndex = -1;
            }

            void IDisposable.Dispose()
            {
                areasCollection = null;
            }

            bool IEnumerator.MoveNext()
            {
                areaCollectionIndex++;
                return areaCollectionIndex < areaCollectionCount;
            }

            void IEnumerator.Reset()
            {
                areaCollectionIndex = -1;
            }
        }
    }
}