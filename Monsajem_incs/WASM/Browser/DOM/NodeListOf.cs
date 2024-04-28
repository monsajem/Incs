using Microsoft.JSInterop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WebAssembly.Browser.DOM
{

    [Export("NodeList", typeof(IJSInProcessObjectReference))]
    public sealed class NodeListOf<T> : DOMObject, IEnumerable<T>, IEnumerable
    {
        public NodeListOf(IJSInProcessObjectReference handle) : base(handle) { }

        //public NodeListOf() { }
        [Export("length")]
        public double Length => GetProperty<double>("length");
        [Export("item")]
        public T Item(double index)
        {
            return InvokeMethod<T>("item", index);
        }
        [IndexerName("TheItem")]
        public T this[double index] { get => Item(index); set => throw new NotImplementedException(); }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new NodeListOf<T>.NodeEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }


        private sealed class NodeEnumerator<U> : IEnumerator<U>, IDisposable, IEnumerator
        {
            private NodeListOf<U> nodeListCollection;

            private int nodeListIndex;

            private double nodeListCount;

            public U Current
            {
                get
                {
                    return nodeListCollection == null
                        ? throw new ObjectDisposedException("NodeListEnumerator is disposed")
                        : nodeListCollection[nodeListIndex];
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

            public NodeEnumerator(NodeListOf<U> collection)
            {
                nodeListCollection = collection;
                nodeListCount = nodeListCollection.Length;
                nodeListIndex = -1;
            }

            void IDisposable.Dispose()
            {
                nodeListCollection = null;
            }

            bool IEnumerator.MoveNext()
            {
                nodeListIndex++;
                return nodeListIndex < nodeListCount;
            }

            void IEnumerator.Reset()
            {
                nodeListIndex = -1;
            }
        }

    }

}