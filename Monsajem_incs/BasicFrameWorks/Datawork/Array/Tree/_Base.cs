using Monsajem_Incs.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Monsajem_Incs.Collection.Array.TreeBased
{
    public partial class Array<ValueType> :
        Base.IArray<ValueType, Array<ValueType>>,
#if DEBUG
        Serialization.ISerializable<(Array<ValueType>.Node Root,int Len, 
            ArrayBased.DynamicSize.Array<ValueType> ItemsForDebug)>
#else
        Serialization.ISerializable<(Array<ValueType>.Node Root, int Len)>
#endif
    {

        public Array()
        {}
        public Array(ValueType[] Values)
        {
            this.Insert(Values);
        }

        public Action<(Node Before,Node Next)> ChangedNextSequence;

#if DEBUG
        private Array.ArrayBased.DynamicSize.Array<ValueType> ItemsForDebug =
            new Array.ArrayBased.DynamicSize.Array<ValueType>(100);

        private void CheckBugs()
        {

            (int Len, int Deep) CheckInners(Node Current)
            {
                var Next = Current.Next;
                var Before = Current.Before;
                var NextLen = 0;
                var BeforeLen = 0;
                var NextDeep = 0;
                var BeforeDeep = 0;

                if (Next != null)
                {
                    var Result = CheckInners(Next);
                    NextLen = Result.Len;
                    NextDeep = Result.Deep;
                }

                if (Before != null)
                {
                    var Result = CheckInners(Before);
                    BeforeLen = Result.Len;
                    BeforeDeep = Result.Deep;
                }

                if (Current.Balance > 1 || Current.Balance < -1)
                    throw new Exception("Balance Faild!");
                if (Current.Holder == null && Current != Root)
                    throw new Exception("Some Data lost!");

                if (Current.NextDeep !=NextDeep)
                    throw new Exception("Data not correct!");
                if (Current.NextLen != NextLen )
                    throw new Exception("Data not correct!");

                if (Next != null)
                {
                    if (Current.Next.Holder != Current)
                        throw new Exception("Some Data lost!");
                }

                if (Current.BeforeDeep !=BeforeDeep)
                    throw new Exception("Data not correct!");
                if (Current.BeforeLen !=BeforeLen)
                    throw new Exception("Data not correct!");

                if (Current.Before != null)
                {
                    if (Current.Before.Holder != Current)
                        throw new Exception("Some Data lost!");
                }

                if (Current.Holder != null)
                {
                    if (Current.Holder == Current)
                        throw new Exception("infinity loop!");

                    if (Current.IsNext && Current != Current.Holder.Next)
                        throw new Exception("Some Data lost!");
                    else if (Current.IsNext == false && Current != Current.Holder.Before)
                        throw new Exception("Some Data lost!");
                    if (ItemsForDebug.Count((c) => Comparer.Compare(c, Current.Holder.Value) == 0) == 0)
                        throw new Exception("Some Data lost!");
                }

                return (NextLen + BeforeLen + 1, Math.Max(NextDeep, BeforeDeep) + 1);
            }

            if (Root != null)
            {
                if (CheckInners(Root).Len != Length)
                    throw new Exception("Length is wrong or bad data exist!");
            }

            for (int i = 0; i < ItemsForDebug.Length; i++)
            {
                var Current = GetItem(i,out var Before,out var Next);
                var ItemValue = ItemsForDebug[i];
                if (Current == null || Comparer.Compare(Current.Value, ItemValue) != 0)
                    throw new Exception("Some Item lost!");

            }
        }
#endif

        private Node _Root;
        public Node Root { get=>_Root; set { _Root = value; } }

        public class Node
        {
            public Node(ValueType Value,Array<ValueType> Collector)
            {
                this.Value = Value;
                this.Collector = Collector;
            }

            public Array<ValueType> Collector;

            public int Balance { get => NextDeep - BeforeDeep; }
            public int NextDeep { get; set; }
            public int BeforeDeep { get; set; }
            public int NextLen { get; set; }
            public int BeforeLen { get; set; }
            public ValueType Value { get; set; }


            public Node _Holder;
            public Node Holder { get=>_Holder; set { _Holder = value;} }

            public bool IsNext { get; set; }

            private Node _Next;
            public Node Next { get=>_Next; set { _Next = value;} }

            private Node _Before;
            public Node Before { get=>_Before; set { _Before = value;} }

            [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
            public Node FindNextSequnce()
            {
                Node holder = default;
                var next = Next;
                if (next != null)
                {
                    while (next != null)
                    {
                        holder = next;
                        next = next.Before;
                    }
                }
                else
                {
                    holder = Holder;
                    if (IsNext == true)
                    {
                        while (holder != null && holder.IsNext == true)
                            holder = holder.Holder;
                        if (holder != null)
                            holder = holder.Holder;
                    }
                }
                return holder;
            }

            public Node FindBeforeSequnce()
            {
                Node holder = default;
                var before = Before;
                if (before != null)
                {
                    while (before != null)
                    {
                        holder = before;
                        before = before.Next;
                    }
                }
                else
                {
                    holder = Holder;
                    if (IsNext == false)
                    {
                        while (holder != null && holder.IsNext == false)
                            holder = holder.Holder;
                        if (holder != null)
                            holder = holder.Holder;
                    }
                }
                return holder;
            }

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
                var cmp = position - (Current.BeforeLen + 1);
                if (cmp == 0)
                    return Current;
                else if (cmp > 0)
                {
                    Before = Current;
                    position -= Current.BeforeLen + 1;
                    Current = Current.Next;
                }
                else
                {
                    Next = Current;
                    Current = Current.Before;
                }
            }

            return null;
        }

        public override ValueType this[int Position]
        {
            get => GetItem(Position, out var x, out var y).Value;
            set
            {
                GetItem(Position, out var x, out var y).Value = value;
#if DEBUG
                ItemsForDebug[Position] = value;
                CheckBugs();
#endif
            }
        }

        public override void Insert(ValueType Value, int Position)
        {
            Node Before; Node Next;
            var Current = GetItem(Position, out Before, out Next);
            Insert(Value, Current, Before, Next);
#if DEBUG
            ItemsForDebug.Insert(Value, Position);
            CheckBugs();
#endif
        }

        public override int BinaryInsert(ValueType Value)
        {
#if DEBUG
            if (
            BinarySearch(Value)
                .Index > -1)
                throw new Exception("Dup")
                ;
#endif
            Node Before; Node Next; int Pos;
            var Current = Find(Value, out Before, out Next, out Pos);
            Insert(Value, Current, Before, Next);

#if DEBUG
            ItemsForDebug.BinaryInsert(Value);
            var ItemResult = ItemsForDebug.BinarySearch(Value);
            if (Pos != ItemResult.Index)
                throw new Exception("Binary insert result is wrong!");
            CheckBugs();
#endif
            return Pos;
        }

        public override (int Index, ValueType Value) BinarySearch(ValueType key, int minNum, int maxNum)
        {
            (int Index, ValueType Value) Result = default;
            Node Before; Node Next; int Pos;
            var Current = Find(key, out Before, out Next, out Pos);
            if (Current != null)
                Result = (Pos, Current.Value);
            else
                Result = (~Pos, default);
#if DEBUG
            CheckBugs();
            var ItemResult = ItemsForDebug.BinarySearch(key);
            if (Result.Index != ItemResult.Index)
                throw new Exception("Binary search result is wrong!");
#endif
            return Result;
        }

        private void Insert(ValueType Value, Node Current, Node Before, Node Next)
        {
            if (Root == null)
            {
                Current = new Node(Value,this);
                Root = Current;
                goto End;
            }

            if (Current != null)
            {
                Next = Current;
                Before = Next.Before;
                if (Before != null)
                {
                    while (Before.Next != null)
                        Before = Before.Next;
                    Next = null;
                }
            }
            else if (Before != null && Next != null)
            {
                if (Before.NextDeep > Next.BeforeDeep)
                    Before = null;
            }
            Current = new Node(Value,this);

            if (Before != null)
            {
                Current.IsNext = true;
                Current.Holder = Before;
                Next = Before.Next;
                Before.Next = Current;
                if (Next != null)
                {
                    Current.Next = Next;
                    Current.NextLen = Next.NextLen + Next.BeforeLen + 1;
                    Next.Holder = Current;
                    Current.NextDeep = Math.Max(Next.BeforeDeep, Next.NextDeep) + 1;
                    Before.NextLen = Current.NextLen;
                    Before.NextDeep = Current.NextDeep;
                    Current = Next;
                }
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
                    Current.BeforeLen = Before.NextLen + Before.BeforeLen + 1;
                    Before.Holder = Current;
                    Current.BeforeDeep = Math.Max(Before.BeforeDeep, Before.NextDeep) + 1;
                    Next.BeforeLen = Current.BeforeLen;
                    Next.BeforeDeep = Current.BeforeDeep;
                    Current = Before;
                }
            }
            FixBalance(Current);

            End:
            Length++;

            if(ChangedNextSequence!=null)
            {
                ChangedNextSequence.Invoke((Current.FindBeforeSequnce(),Current));
            }
        }

        public override void DeleteByPosition(int Position)
        {
            Node Before; Node Next;
            var Item = GetItem(Position, out Before, out Next);

            Node BeforeSequnce=default;
            var ChangedNextSequence = this.ChangedNextSequence;
            if (ChangedNextSequence != null)
                BeforeSequnce = Item.FindBeforeSequnce();

            Drop(Item);
            Length--;

            if (ChangedNextSequence != null)
                ChangedNextSequence.Invoke((BeforeSequnce, BeforeSequnce?.FindNextSequnce()));

#if DEBUG
            ItemsForDebug.DeleteByPosition(Position);
            CheckBugs();
#endif
        }

        public override (int Index, ValueType Value) BinaryDelete(ValueType Value)
        {
            Node Before; Node Next; int Pos;
            var Item = Find(Value, out Before, out Next, out Pos);
            if (Item == null)
                throw new Exception($"{Value} not found!");
            var Result = (Pos, Item.Value);
            Drop(Item);
            Length--;
#if DEBUG
           var ItemResult = ItemsForDebug.BinaryDelete(Value);
            CheckBugs();
            if (Pos != ItemResult.Index)
                throw new Exception("Binary delete result is wrong!");
#endif
            return Result;
        }

        private void MoveToHolder(Node Node)
        {
            var Holder = Node.Holder;
            var Root = Holder.Holder;
            var HolderIsNext = Holder.IsNext;
            var NodeIsNext = Node.IsNext;
            if (Root != null)
            {
                if (HolderIsNext)
                {
                    Root.Next = Node;
                    Node.IsNext = true;
                }
                else
                {
                    Root.Before = Node;
                    Node.IsNext = false;
                }
            }
            else
                this.Root = Node;

            if (NodeIsNext)
            {
                Holder.NextLen = Node.BeforeLen;
                Node.BeforeLen = (Holder.NextLen + Holder.BeforeLen) + 1;
                var Before = Node.Before;
                Holder.Next = Before;
                if (Before != null)
                {
                    Before.Holder = Holder;
                    Before.IsNext = true;
                    Holder.NextDeep = Math.Max(Before.NextDeep, Before.BeforeDeep) + 1;
                }
                else
                    Holder.NextDeep = 0;
                Node.Before = Holder;
                Holder.IsNext = false;
                Node.BeforeDeep = Math.Max(Holder.NextDeep, Holder.BeforeDeep) + 1;
            }
            else
            {
                Holder.BeforeLen = Node.NextLen;
                Node.NextLen = (Holder.NextLen + Holder.BeforeLen) + 1;
                var Next = Node.Next;
                Holder.Before = Next;
                if (Next != null)
                {
                    Next.Holder = Holder;
                    Next.IsNext = false;
                    Holder.BeforeDeep = Math.Max(Next.NextDeep, Next.BeforeDeep) + 1;
                }
                else
                    Holder.BeforeDeep = 0;
                Node.Next = Holder;
                Holder.IsNext = true;
                Node.NextDeep = Math.Max(Holder.NextDeep, Holder.BeforeDeep) + 1;
            }

            Holder.Holder = Node;
            if (Root != null)
            {
                Node.Holder = Root;
                if (Node.IsNext)
                    Root.NextDeep = Math.Max(Node.NextDeep, Node.BeforeDeep) + 1;
                else
                    Root.BeforeDeep = Math.Max(Node.NextDeep, Node.BeforeDeep) + 1;
            }
            else
                Node.Holder = null;
        }

        private void Drop(Node Node)
        {
            var Root = Node.Holder;
            var BeforeLen = Node.BeforeLen;
            var NextLen = Node.NextLen;
            if (BeforeLen == 0 && NextLen == 0)
            {
                DropFromHolder(Node, null);
                FixWay(Root);
                return;
            }
            else if (BeforeLen == 0)
            {
                DropFromHolder(Node, Node.Next);
                FixWay(Root);
                return;
            }
            else if (NextLen == 0)
            {
                DropFromHolder(Node, Node.Before);
                FixWay(Root);
                return;
            }

            var Holder = Node;
            if (Node.BeforeLen > Node.NextLen)
            {
                Node = Node.Before;
                while (Node.NextLen > 0)
                {
                    Node.NextLen--;
                    Node = Node.Next;
                }
                var Replace = Node.Before;
                DropFromHolder(Node, Replace);
                if (Replace == null)
                    Replace = Node;
                FixWay(Replace);
            }
            else
            {
                Node = Node.Next;
                while (Node.BeforeLen > 0)
                {
                    Node.BeforeLen--;
                    Node = Node.Before;
                }
                var Replace = Node.Next;
                DropFromHolder(Node, Replace);
                if (Replace == null)
                    Replace = Node;
                FixWay(Replace);
            }
            var Before = Holder.Before;
            var Next = Holder.Next;
            Node.Before = Before;
            Node.Next = Next;
            if (Before != null)
            {
                Node.BeforeLen = Before.BeforeLen + Before.NextLen + 1;
                Node.BeforeDeep = Math.Max(Before.BeforeDeep, Before.NextDeep) + 1;
                Before.Holder = Node;
            }
            else
            {
                Node.BeforeLen = 0;
                Node.BeforeDeep = 0;
            }
            if (Next != null)
            {
                Node.NextLen = Next.BeforeLen + Next.NextLen + 1;
                Node.NextDeep = Math.Max(Next.BeforeDeep, Next.NextDeep) + 1;
                Next.Holder = Node;
            }
            else
            {
                Node.NextLen = 0;
                Node.NextDeep = 0;
            }
            DropFromHolder(Holder, Node);
            FixWay(Root);
        }

        private void DropFromHolder(Node Node, Node Replace)
        {
            var Holder = Node.Holder;
            if (Holder == null)
            {
                Root = Replace;
                if (Replace != null)
                    Replace.Holder = null;
            }
            else
            {
                var len = 0;
                if (Replace != null)
                    len = Replace.NextLen + Replace.BeforeLen + 1;

                if (Node.IsNext)
                {
                    Holder.Next = Replace;
                    if (Replace != null)
                    {
                        Replace.Holder = Holder;
                        Replace.IsNext = true;
                    }
                    Holder.NextLen = len;
                }
                else
                {
                    Holder.Before = Replace;
                    if (Replace != null)
                    {
                        Replace.Holder = Holder;
                        Replace.IsNext = false;
                    }
                    Holder.BeforeLen = len;
                }
            }
            return;
        }

        private void FixWay(Node Holder)
        {
            while (Holder != null)
            {
                var Next = Holder.Next;
                var Before = Holder.Before;
                if (Next != null)
                {
                    Holder.NextDeep = Math.Max(Next.NextDeep, Next.BeforeDeep) + 1;
                    Holder.NextLen = Next.NextLen + Next.BeforeLen + 1;
                }
                else
                {
                    Holder.NextDeep = 0;
                    Holder.NextLen = 0;
                }
                if (Before != null)
                {
                    Holder.BeforeDeep = Math.Max(Before.NextDeep, Before.BeforeDeep) + 1;
                    Holder.BeforeLen = Before.NextLen + Before.BeforeLen + 1;
                }
                else
                {
                    Holder.BeforeDeep = 0;
                    Holder.BeforeLen = 0;
                }

                if (Holder.Balance > 1)
                {
                    var node = Next;
                    if (node.Balance >= 0)
                    {
                        MoveToHolder(node);
                        Holder = node;
                    }
                    else
                    {
                        Before = node.Before;
                        MoveToHolder(Before);
                        MoveToHolder(Before);
                        Holder = Before;
                    }
                }
                else if (Holder.Balance < -1)
                {
                    var node = Before;
                    if (node.Balance <= 0)
                    {
                        MoveToHolder(node);
                        Holder = node;
                    }
                    else
                    {
                        Next = node.Next;
                        MoveToHolder(Next);
                        MoveToHolder(Next);
                        Holder = Next;
                    }
                }
                else
                    Holder = Holder.Holder;
            }
        }

        public Node Find(ValueType Value)
        {
            return Find(Value, out var Before, out var Next, out var Pos);
        }

        public override object MyOptions { get => null; set { } }

        public Node Find(
            ValueType Value,
            out Node Before,
            out Node Next,
            out int Position)
        {
            Before = default;
            Next = default;
            Position = 0;
            var Current = Root;
            while (Current != null)
            {
                var cmp = Comparer.Compare(Current.Value, Value);
                if (cmp == 0)
                {
                    Position += Current.BeforeLen;
                    return Current;
                }
                else if (cmp < 0)
                {
                    Position += Current.BeforeLen + 1;
                    Before = Current;
                    Current = Current.Next;
                }
                else
                {
                    Next = Current;
                    Current = Current.Before;
                }
            }
            //if (Next != null)
            //    Position += Next.BeforeLen + Next.NextLen + 1;
            return null;
        }

        private void FixBalance(Node node)
        {
            while (node != null && node.Holder != null)
            {
                var Holder = node.Holder;
                var HolderDeep = Math.Max(node.NextDeep, node.BeforeDeep) + 1;
                var HolderLen = node.NextLen + node.BeforeLen + 1;
                if (node.IsNext == true)
                {
                    Holder.NextLen = HolderLen;
                    Holder.NextDeep = HolderDeep;
                }
                else
                {
                    Holder.BeforeLen = HolderLen;
                    Holder.BeforeDeep = HolderDeep;
                }

                var HolderBalance = Holder.Balance;
                if (HolderBalance > 1)
                {
                    if (node.Balance >= 0)
                        MoveToHolder(node);
                    else
                    {
                        var Before = node.Before;
                        MoveToHolder(Before);
                        MoveToHolder(Before);
                        node = Before;
                    }
                }
                else if (HolderBalance < -1)
                {
                    if (node.Balance <= 0)
                        MoveToHolder(node);
                    else
                    {
                        var Next = node.Next;
                        MoveToHolder(Next);
                        MoveToHolder(Next);
                        node = Next;
                    }
                }
                else
                    node = node.Holder;
            }
        }

        protected override Array<ValueType> MakeSameNew()
        {
            return new Array<ValueType>();
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization|MethodImplOptions.AggressiveInlining)]
        public override IEnumerator<ValueType> GetEnumerator()
        {
            Node Node = this.GetItem(0, out var b, out var n);
            while (Node != null)
            {
                yield return Node.Value;
                Node = Node.FindNextSequnce();
            }
        }

#if DEBUG
        (Node Root, int Len, ArrayBased.DynamicSize.Array<ValueType> ItemsForDebug) 
            ISerializable<(Node Root, int Len, ArrayBased.DynamicSize.Array<ValueType> ItemsForDebug)>.
            GetData()
        {
            return (Root, Length,ItemsForDebug);
        }

        void ISerializable<(Node Root, int Len, ArrayBased.DynamicSize.Array<ValueType> ItemsForDebug)>.
            SetData((Node Root, int Len, ArrayBased.DynamicSize.Array<ValueType> ItemsForDebug) Data)
        {
            Comparer = Comparer<ValueType>.Default;
            this.Root = Data.Root;
            this.Length = Data.Len;
            this.ItemsForDebug = Data.ItemsForDebug;
            return;
        }
#else
        (Node Root, int Len) ISerializable<(Node Root, int Len)>.GetData()
        {
            return (this.Root, Length);
        }

        void ISerializable<(Node Root, int Len)>.SetData((Node Root, int Len) Data)
        {
            Comparer = Comparer<ValueType>.Default;
            this.Root = Data.Root;
            this.Length = Data.Len;
        }
#endif

    }
}