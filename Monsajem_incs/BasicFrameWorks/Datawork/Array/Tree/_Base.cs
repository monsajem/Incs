using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monsajem_Incs.Collection.Array.TreeBased
{
    public partial class Array<ValueType>:Base.IArray<ValueType,Array<ValueType>>
    {

        public Array()
        { }
        public Array(ValueType[] Values)
        {
            this.Insert(Values);
        }

#if DEBUG
        private Array.ArrayBased.DynamicSize.Array<ValueType> Items =
            new Array.ArrayBased.DynamicSize.Array<ValueType>(100);

        private void Check()
        {
            for (int i=0; i<Items.Length;i++)
            {
                Node Before; Node Next;
                var Current = GetItem(i, out Before, out Next);
                if (Current == null)
                    throw new Exception("Some Item lost!");
                //if (Current.Equality > 2 || Current.Equality < -2)
                //    throw new Exception("Equality Faild!");
                if (Current.Holder == null && Current != Root)
                    throw new Exception("Some Data lost!");
                if (Current.Next != null)
                {
                    //if (Comparer.Compare(Current.Next.Value, Current.Value) < 0)
                    //    throw new Exception("Data not correct!");

                    if (Current.NextDeep > 0)
                    {
                        if (Current.NextDeep != Current.Next.NextDeep + Current.Next.BeforeDeep + 1)
                            throw new Exception("Data not correct!");
                    }
                    else if (Current.Next != null)
                        throw new Exception("Data not correct!");

                    if (Current.BeforeDeep > 0)
                    {
                        if (Current.BeforeDeep != Current.Before.NextDeep + Current.Before.BeforeDeep + 1)
                            throw new Exception("Data not correct!");
                    }
                    else if (Current.Before != null)
                        throw new Exception("Data not correct!");

                    if (Current.Next.Holder != Current)
                        throw new Exception("Some Data lost!");
                }
                if (Current.Before != null)
                {
                    //if (Comparer.Compare(Current.Before.Value, Current.Value) > 0)
                    //    throw new Exception("Data not correct!");
                    if (Current.Before.Holder != Current)
                        throw new Exception("Some Data lost!");
                }
                if (Current != Root)
                {
                    if (Current.IsNext && Current != Current.Holder.Next)
                        throw new Exception("Some Data lost!");
                    else if (Current.IsNext == false && Current != Current.Holder.Before)
                        throw new Exception("Some Data lost!");
                }
            }
        }
#endif

        public Node Root;
        public class Node
        {
            public Node(ValueType Value)
            {
                this.Hash = this.GetHashCode();
                this.Value = Value;
            }

            public int Equality { get => NextDeep - BeforeDeep; }
            public int NextDeep;
            public int BeforeDeep;

            public bool WayIsNext;
            public bool WayIsEnd=true;

#if DEBUG
            private void Set<t>(t From, ref t To)
            {
                //if (From != null && To != null)
                //    if (From.GetHashCode() == To.GetHashCode())
                //        throw new Exception("Extra Action");
                To = From;
            }

            public ValueType Value;
            private Node _Holder;
            public Node Holder { get => _Holder; set => Set(value, ref _Holder); }
            private Node _Next;
            public Node Next { get => _Next; set => Set(value, ref _Next); }
            private Node _Before;
            public Node Before { get => _Before; set => Set(value, ref _Before); }
            private bool _IsNext;
            public bool IsNext { get => _IsNext; set => _IsNext = value; }
            public int Hash;
#else
            public ValueType Value;
            public Node Holder;
            public bool IsNext;
            public Node Next;
            public Node Before;
            public int Hash;
#endif

            public override string ToString()
            {
                if (Holder == null)
                    return "Root";
                else
                {
                    if (IsNext)
                        return $"Body next: {Value}";
                    else
                        return $"Body before: {Value}";
                }
            }
        }

        private Node GetItem(
            int position,
            out Node Before,
            out Node Next)
        {
            Before = default;
            Next = default;
            if (position < 0)
                throw new Exception("Position must start at zero.");
            position++;
            var Current = Root;
            while (Current != null)
            {
                Current.WayIsEnd = false;
                var cmp = position - (Current.BeforeDeep + 1);
                if (cmp == 0)
                    return Current;
                else if (cmp > 0)
                {
                    Before = Current;
                    position -= Current.BeforeDeep + 1;
                    Current.WayIsNext = true;
                    Current = Current.Next;
                }
                else
                {
                    Next = Current;
                    Current.WayIsNext = false;
                    Current = Current.Before;
                }
            }

            return null;
        }

        public override ValueType this[int Position]
        {
            get => GetItem(Position,out var x,out var y).Value;
            set => GetItem(Position, out var x, out var y).Value = value;
        }

        public override void Insert(ValueType Value, int Position)
        {
            Node Before; Node Next;
            var Current = GetItem(Position, out Before, out Next);
            Insert(Value,ref Current, Before, Next);
        }

        public override int BinaryInsert(ValueType Value)
        {
            Node Before; Node Next;
            var Current = Find(Value, out Before, out Next);
            Insert(Value,ref Current, Before, Next);
            return Current.BeforeDeep;
        }

        private void Insert(ValueType Value,ref Node FindedSame, Node Before, Node Next)
        {
            if (Root == null)
            {
                Root = new Node(Value);
#if DEBUG
                Items.BinaryInsert(Value);
                Check();
#endif
                return;
            }

            var Current = FindedSame;
            if (Current != null)
            {
                Next = Current;
                Next.WayIsNext = false;
            }
            Current = new Node(Value);

            if (Before != null)
            {
                Current.IsNext = true;
                Current.Holder = Before;
                Next = Before.Next;
                Before.Next = Current;
                if (Next != null)
                {
                    Current.Next = Next;
                    Current.NextDeep = Next.NextDeep + Next.BeforeDeep + 1;
                    Next.Holder = Current;
                }
                Before.NextDeep = Current.NextDeep;
            }
            else
            {
                Current.IsNext = false;
                Current.Holder = Next;
                Before = Next.Before;
                Next.Before = Current;
                if (Before != null)
                {
                    Current.Before = Before;
                    Current.BeforeDeep = Before.NextDeep + Before.BeforeDeep + 1;
                    Before.Holder = Current;
                }
                Next.BeforeDeep = Current.BeforeDeep;
            }

            FixEquality();
#if DEBUG
            Items.BinaryInsert(Value);
            Check();
#endif
        }

        public void MoveToHolder(Node Node)
        {
            var Holder = Node.Holder;
            var Root = Holder.Holder;
            if (Root != null)
            {
                if (Holder.IsNext)
                    Root.Next = Node;
                else
                    Root.Before = Node;
            }
            else
                this.Root = Node;

            if (Node.IsNext)
            {
                Holder.NextDeep = Node.BeforeDeep;
                Node.BeforeDeep = (Holder.NextDeep + Holder.BeforeDeep) + 1;
                var Before = Node.Before;
                Holder.Next = Before;
                if (Before != null)
                {
                    Before.Holder = Holder;
                    Before.IsNext = true;
                }
                Node.Before = Holder;
                Holder.IsNext = false;
            }
            else
            {
                Holder.BeforeDeep = Node.NextDeep;
                Node.NextDeep = (Holder.NextDeep + Holder.BeforeDeep) + 1;
                var Next = Node.Next;
                Holder.Before = Next;
                if (Next != null)
                {
                    Next.Holder = Holder;
                    Next.IsNext = false;
                }
                Node.Next = Holder;
                Holder.IsNext = true;
            }

            Holder.Holder = Node;
            if (Root != null)
                Node.Holder = Root;
            else
                Node.Holder = null;
        }

        public Node Find(ValueType Value)
        {
            Node Before; Node Next;
            return Find(Value, out Before, out Next);
        }

        public override object MyOptions { get =>null; set{ }}

        public Node Find(
            ValueType Value,
            out Node Before,
            out Node Next)
        {
            Before = default;
            Next = default;
            var Current = Root;
            while (Current != null)
            {
                Current.WayIsEnd = false;
                var cmp = Comparer.Compare(Current.Value, Value);
                if (cmp == 0)
                    return Current;
                else if (cmp < 0)
                {
                    if (Current.Hash == Before?.Hash)
                    {
                        return null;
                    }
                    Before = Current;
                    Current.WayIsNext = true;
                    Current = Current.Next;
                }
                else
                {
                    if (Current.Hash == Next?.Hash)
                    {
                        return null;
                    }
                    Next = Current;
                    Current.WayIsNext = false;
                    Current = Current.Before;
                }
            }
            return null;
        }

        private void FixEquality()
        {
            var node = Root;
            while (node != null)
            {
                if(node.WayIsEnd==false)
                {
                    if (node.WayIsNext == true && node.Next != null)
                    {
                        node.NextDeep++;
                        if (node.Equality > 1)
                        {
                            MoveToHolder(node.Next);
                            node = node.Holder;
                        }
                        else
                            node = node.Next;
                    }
                    else if (node.WayIsNext == false && node.Before != null)
                    {
                        node.BeforeDeep++;
                        if (node.Equality < -1)
                        {
                            MoveToHolder(node.Before);
                            node = node.Holder;
                        }
                        else
                            node = node.Before;
                    }
                    else
                        node = null;
                }
                else
                    node = null;
            }
        }

        protected override Array<ValueType> MakeSameNew()
        {
            return new Array<ValueType>();
        }
    }
}