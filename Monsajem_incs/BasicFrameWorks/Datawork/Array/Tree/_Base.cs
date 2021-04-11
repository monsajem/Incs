using Monsajem_Incs.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monsajem_Incs.Collection.Array.TreeBased
{
    public partial class Array<ValueType> : 
        Base.IArray<ValueType, Array<ValueType>>,
#if DEBUG
        Serialization.ISerializable<(Array<ValueType>.INode Root,int Len,bool CacheSerialize, 
            ArrayBased.DynamicSize.Array<ValueType> ItemsForDebug)>
#else
        Serialization.ISerializable<(Array<ValueType>.INode Root,int Len,bool CacheSerialize)>
#endif
    {

        public Array()
        {
            CreateNode = _CreateNode;
        }
        public Array(ValueType[] Values)
        {
            CreateNode = _CreateNode;
            this.Insert(Values);
        }
        public Array(bool CacheSerialize)
        {
            if (CacheSerialize)
                CreateNode = _CreateCacheSerializeNode;
            else
                CreateNode = _CreateNode;
        }
        public Array(ValueType[] Values,bool CacheSerialize)
        {
            if (CacheSerialize)
                CreateNode = _CreateCacheSerializeNode;
            else
                CreateNode = _CreateNode;
            this.Insert(Values);
        }

        private static Func<ValueType,INode> _CreateCacheSerializeNode = (Value) => new CacheSerializeNode(Value);
        private static Func<ValueType, INode> _CreateNode = (Value) => new Node(Value);

#if DEBUG
        private Array.ArrayBased.DynamicSize.Array<ValueType> ItemsForDebug =
            new Array.ArrayBased.DynamicSize.Array<ValueType>(100);

        private void CheckBugs()
        {

            (int Len, int Deep) CheckInners(INode Current)
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

        public INode Root;
        public interface INode
        {
            int Balance { get=> NextDeep - BeforeDeep; }
            int NextDeep{get;set;}
            int BeforeDeep{get;set;}
            int NextLen{get;set; }
            int BeforeLen{get;set;}
            ValueType Value{ get; set; }
            INode Holder{get;set;}
            bool IsNext{get;set;}
            INode Next{get;set;}
            public INode Before{get;set;}
            int Hash { get; }
        }
        public Func<ValueType,INode> CreateNode;

        public class Node:INode
        {
            public Node(ValueType Value)
            {
                this.Hash = this.GetHashCode();
                this.Value = Value;
            }

            public int NextDeep { get; set; }
            public int BeforeDeep { get ; set; }
            public int NextLen { get; set; }
            public int BeforeLen { get; set; }
            public ValueType Value { get; set; }
            public INode Holder { get; set; }
            public bool IsNext { get; set; }
            public INode Next { get; set; }
            public INode Before { get; set; }

            public int Hash { get; }

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
        public class CacheSerializeNode
            : Serialization.StreamCacheSerialize, INode
        {
            public CacheSerializeNode(ValueType Value)
            {
                this._Hash = this.GetHashCode();
                this.Value = Value;
            }

            private int _NextDeep;
            public int NextDeep
            {
                get => _NextDeep;
                set { Cache = null; _NextDeep = value; }
            }

            private int _BeforeDeep;
            public int BeforeDeep
            {
                get => _BeforeDeep;
                set { Cache = null; _BeforeDeep = value; }
            }

            private int _NextLen;
            public int NextLen
            {
                get => _NextLen;
                set { Cache = null; _NextLen = value; }
            }

            private int _BeforeLen;
            public int BeforeLen
            {
                get => _BeforeLen;
                set { Cache = null; _BeforeLen = value; }
            }

            private ValueType _Value;
            public ValueType Value
            {
                get => _Value;
                set { Cache = null; _Value = value; }
            }

            private INode _Holder;
            public INode Holder
            {
                get => _Holder;
                set { Cache = null; _Holder = value; }
            }

            public bool _IsNext;
            public bool IsNext
            {
                get => _IsNext;
                set { Cache = null; _IsNext = value; }
            }

            private INode _Next;
            public INode Next
            {
                get => _Next;
                set { Cache = null; _Next = value; }
            }

            private INode _Before;
            public INode Before
            {
                get => _Before;
                set { Cache = null; _Before = value; }
            }

            private int _Hash;
            public int Hash { get => _Hash; }

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

        private INode GetItem(
            int position,
            out INode Before,
            out INode Next)
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
            INode Before; INode Next;
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
            INode Before; INode Next; int Pos;
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
            INode Before; INode Next; int Pos;
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

        private void Insert(ValueType Value, INode Current, INode Before, INode Next)
        {
            if (Root == null)
            {
                Current = CreateNode(Value);
                Root = Current;
                Length++;
                return;
            }

            if (Current != null)
            {
                Next = Current;
                Before = Next.Before;
                if(Before!=null)
                {
                        while (Before.Next != null)
                            Before = Before.Next;
                        Next = null;
                }
            }
            else if(Before!=null&&Next!=null)
            {
                if (Before.NextDeep > Next.BeforeDeep)
                    Before = null;
            }
            Current = CreateNode(Value);

            if (Before != null)
            {
                Current.IsNext = true;
                Current.Holder = Before;
                Next = Before.Next;
                Before.Next = Current;
                if (Next != null)
                {
                    Current.Next = Next;
                    Current.NextLen = Next.NextLen + Next.BeforeLen+1;
                    Next.Holder = Current;
                    Current.NextDeep = Math.Max(Next.BeforeDeep, Next.NextDeep)+1;
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
                    Current.BeforeLen = Before.NextLen + Before.BeforeLen+1;
                    Before.Holder = Current;
                    Current.BeforeDeep =Math.Max(Before.BeforeDeep,Before.NextDeep)+1;
                    Next.BeforeLen = Current.BeforeLen;
                    Next.BeforeDeep = Current.BeforeDeep;
                    Current = Before;
                }
            }
            FixBalance(Current);
            Length++;
        }

        public override void DeleteByPosition(int Position)
        {
            INode Before; INode Next;
            var Item = GetItem(Position, out Before, out Next);
            Drop(Item);
            Length--;

#if DEBUG
            ItemsForDebug.DeleteByPosition(Position);
            CheckBugs();
#endif
        }

        public override (int Index, ValueType Value) BinaryDelete(ValueType Value)
        {
            INode Before; INode Next; int Pos;
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

        private void MoveToHolder(INode Node)
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
                if(Node.IsNext)
                    Root.NextDeep = Math.Max(Node.NextDeep, Node.BeforeDeep) + 1;
                else
                    Root.BeforeDeep = Math.Max(Node.NextDeep, Node.BeforeDeep) + 1;
            }
            else
                Node.Holder = null;
        }

        private void Drop(INode Node)
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
                Node.BeforeDeep = Math.Max(Before.BeforeDeep,Before.NextDeep) + 1;
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

        private void DropFromHolder(INode Node, INode Replace)
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

        private void FixWay(INode Holder)
        {
            while (Holder != null)
            {
                var Next = Holder.Next;
                var Before = Holder.Before;
                if(Next!=null)
                {
                    Holder.NextDeep = Math.Max(Next.NextDeep, Next.BeforeDeep) + 1;
                    Holder.NextLen = Next.NextLen + Next.BeforeLen + 1;
                }
                else
                {
                    Holder.NextDeep =0;
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

        public INode Find(ValueType Value)
        {
            return Find(Value,out var Before,out var Next,out var Pos);
        }

        public override object MyOptions { get => null; set { } }

        public INode Find(
            ValueType Value,
            out INode Before,
            out INode Next, 
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

        private void FixBalance(INode node)
        {
            while (node!=null && node.Holder != null)
            {
                var Holder = node.Holder;
                var HolderDeep = Math.Max(node.NextDeep, node.BeforeDeep) + 1;
                var HolderLen = node.NextLen+ node.BeforeLen+1;
                if (node.IsNext == true)
                {
                    Holder.NextLen= HolderLen;
                    Holder.NextDeep = HolderDeep;
                }
                else
                {
                    Holder.BeforeLen= HolderLen;
                    Holder.BeforeDeep = HolderDeep;
                }

                var HolderBalance = Holder.Balance;
                if (HolderBalance > 1)
                {
                    if(node.Balance>=0)
                        MoveToHolder(node);
                    else
                    {
                        var Before = node.Before;
                        MoveToHolder(Before);
                        MoveToHolder(Before);
                        node = Before;
                    }
                }else if (HolderBalance < -1)
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

#if DEBUG
        (INode Root, int Len, bool CacheSerialize, ArrayBased.DynamicSize.Array<ValueType> ItemsForDebug) 
            ISerializable<(INode Root, int Len, bool CacheSerialize, ArrayBased.DynamicSize.Array<ValueType> ItemsForDebug)>.
            GetData()
        {
            return (Root, Length,CreateNode==_CreateCacheSerializeNode,ItemsForDebug);
        }

        void ISerializable<(INode Root, int Len, bool CacheSerialize, ArrayBased.DynamicSize.Array<ValueType> ItemsForDebug)>.
            SetData((INode Root, int Len, bool CacheSerialize, ArrayBased.DynamicSize.Array<ValueType> ItemsForDebug) Data)
        {
            Comparer = Comparer<ValueType>.Default;
            this.Root = Data.Root;
            this.Length = Data.Len;
            this.ItemsForDebug = Data.ItemsForDebug;
            if (Data.CacheSerialize)
                CreateNode = _CreateCacheSerializeNode;
            else
                CreateNode = _CreateNode;
            return;
        }
#else
        (INode Root, int Len, bool CacheSerialize) ISerializable<(INode Root, int Len, bool CacheSerialize)>.GetData()
        {
            return (this.Root, Length,CreateNode==_CreateCacheSerializeNode);
        }

        void ISerializable<(INode Root, int Len, bool CacheSerialize)>.SetData((INode Root, int Len, bool CacheSerialize) Data)
        {
            Comparer = Comparer<ValueType>.Default;
            this.Root = Data.Root;
            this.Length = Data.Len;
            if (Data.CacheSerialize)
                CreateNode = _CreateCacheSerializeNode;
            else
                CreateNode = _CreateNode;
        }
#endif

    }
}