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
                var ItemValue = Items[i];
                if (Current == null||Comparer.Compare(Current.Value,ItemValue)!=0)
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
                if (Current.Holder != null)
                {
                    if (Current.IsNext && Current != Current.Holder.Next)
                        throw new Exception("Some Data lost!");
                    else if (Current.IsNext == false && Current != Current.Holder.Before)
                        throw new Exception("Some Data lost!");
                    if(Items.Count((c)=>Comparer.Compare(c,Current.Holder.Value)==0)==0)
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

            public ValueType Value;
            public Node Holder;
            public bool IsNext;
            public Node Next;
            public Node Before;
            public int Hash;


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
#if DEBUG
            Items.Insert(Value, Position);
            Check();
#endif
        }

        public override int BinaryInsert(ValueType Value)
        {
            Node Before; Node Next;
            var Current = Find(Value, out Before, out Next);
            Insert(Value,ref Current, Before, Next);
#if DEBUG
            Items.BinaryInsert(Value);
            Check();
#endif
            return Current.BeforeDeep;
        }

        private void Insert(ValueType Value,ref Node Current, Node Before, Node Next)
        {
            if (Root == null)
            {
                Current = new Node(Value);
                Root = Current;
                Length++;
                return;
            }

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
            Length++;
        }

        public override void DeleteByPosition(int Position)
        {
            Node Before; Node Next;
            var Item = GetItem(Position, out Before, out Next);
            Item.WayIsEnd = true;
            Drop(Item);
            Length--;

#if DEBUG
            Items.DeleteByPosition(Position);
            Check();
#endif
        }

        public override (int Index, ValueType Value) BinaryDelete(ValueType Value)
        {
            Node Before; Node Next;
            var Item = Find(Value, out Before, out Next);
            if (Item == null)
                throw new Exception($"{Value} not found!");
            Item.WayIsEnd = true;
            var Result = (Item.BeforeDeep, Item.Value);
            Drop(Item);
            Length--;
            return Result;
#if DEBUG
            Items.BinaryDelete(Value);
            Check();
#endif
        }

        private void MoveToHolder(Node Node)
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

        private void Drop(Node Node)
        {
            DropWayLen();
            var BeforeLen = Node.BeforeDeep;
            var NextLen = Node.NextDeep;
            if (BeforeLen == 0 && NextLen == 0)
            {
                DropFromHolder(Node, null);
                return;
            }
            else if (BeforeLen == 0)
            {
                DropFromHolder(Node, Node.Next);
                return;
            }
            else if (NextLen == 0)
            {
                DropFromHolder(Node, Node.Before);
                return;
            }

            var Holder = Node;
            if (Node.BeforeDeep > Node.NextDeep)
            {
                Node = Node.Before; 
                while (Node.NextDeep > 0)
                {
                    Node.NextDeep--;
                    Node = Node.Next;
                }
                DropFromHolder(Node, Node.Before); 
            }
            else
            {
                Node = Node.Next;
                while (Node.BeforeDeep > 0)
                {
                    Node.BeforeDeep--;
                    Node = Node.Before;
                }
                DropFromHolder(Node, Node.Next);
            }
            var Before = Holder.Before;
            var Next = Holder.Next;
            Node.Before = Before;
            Node.Next = Next;
            Node.BeforeDeep = Before.BeforeDeep + Before.NextDeep + 1;
            Node.NextDeep = Next.BeforeDeep + Next.NextDeep + 1;
            DropFromHolder(Holder, Node);
        }

        private void DropFromHolder(Node Node,Node Replace)
        {
            var Holder = Node.Holder;
            if (Holder == null)
            {
                Root = Replace;
                if(Replace!=null)
                    Replace.Holder = null;
            }
            else
            {
                var len = 0;
                if (Replace != null)
                    len = Replace.NextDeep + Replace.BeforeDeep + 1;

                if (Node.IsNext)
                {
                    Holder.Next = Replace;
                    if(Replace!=null)
                    {
                        Replace.Holder = Holder;
                        Replace.IsNext = true;
                    }
                    Holder.NextDeep = len;
                }
                else
                {
                    Holder.Before = Replace;
                    if(Replace!=null)
                    {
                        Replace.Holder = Holder;
                        Replace.IsNext = false;
                    }
                    Holder.BeforeDeep = len;
                }
            }
            return;
        }

        private void DropWayLen()
        {
            var Holder = Root;
            while (Holder.WayIsEnd != true)
            {
                if (Holder.WayIsNext)
                {
                    Holder.NextDeep--;
                    Holder = Holder.Next;
                }
                else
                {
                    Holder.BeforeDeep--;
                    Holder = Holder.Before;
                }
            }
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