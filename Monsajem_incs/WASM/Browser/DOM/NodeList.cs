using Microsoft.JSInterop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WebAssembly.Browser.DOM
{

    [Export("NodeList", typeof(IJSInProcessObjectReference))]
    public sealed class NodeList : DOMObject, IEnumerable<Node>, IEnumerable
    {
        public NodeList(IJSInProcessObjectReference handle) : base(handle) { }

        //public NodeList() { }
        [Export("length")]
        public double Length => GetProperty<double>("length");
        [Export("item")]
        public Node Item(double index)
        {
            return InvokeMethod<Node>("item", index);
        }
        [IndexerName("TheItem")]
        public Node this[double index] { get => Item(index); set => throw new NotImplementedException(); }

        IEnumerator<Node> IEnumerable<Node>.GetEnumerator()
        {
            return new NodeList.NodeEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<DOMObject>)this).GetEnumerator();
        }


        private sealed class NodeEnumerator : IEnumerator<Node>, IDisposable, IEnumerator
        {
            private NodeList nodeListCollection;

            private int nodeListIndex;

            private double nodeListCount;

            public Node Current
            {
                get
                {
                    return nodeListCollection == null
                        ? throw new ObjectDisposedException("NodeListEnumerator is disposed")
                        : nodeListCollection[nodeListIndex];
                }
            }

            Node IEnumerator<Node>.Current
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

            public NodeEnumerator(NodeList collection)
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