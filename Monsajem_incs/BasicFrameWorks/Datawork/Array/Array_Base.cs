using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Monsajem_Incs.Collection.Array.Base
{
    internal interface IArray
    {
        System.Array ToArray();
        object MyOptions { get; set; }
        Type ElementType { get; }
        void Insert(System.Array Array);
    }

    public abstract partial class IArray<ArrayType> :
        IArray,IEnumerable<ArrayType>
    {

        public virtual int Length {
            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            protected set; }

        public abstract object MyOptions { get; set; }

        public Type ElementType => typeof(ArrayType);

        object IArray.MyOptions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        Type IArray.ElementType => typeof(ArrayType);


        public abstract ArrayType this[int Pos] 
        {
            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            set; 
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual void Clear()
        {
            DeleteFromTo(0, Length - 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        protected static void Copy(
            IArray<ArrayType> sourceArray,
            int sourceIndex,
            IArray<ArrayType> destinationArray,
            int destinationIndex,
            int length)
        {
            sourceArray.CopyTo(sourceIndex, destinationArray, destinationIndex, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        protected static void Copy(
            IArray<ArrayType> sourceArray,
            int sourceIndex,
            ArrayType[] destinationArray,
            int destinationIndex,
            int length)
        {
            sourceArray.CopyTo(sourceIndex, destinationArray, destinationIndex, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        protected static void Copy(
            ArrayType[] sourceArray,
            int sourceIndex,
            IArray<ArrayType> destinationArray,
            int destinationIndex,
            int length)
        {
            destinationArray.CopyFrom(sourceIndex, sourceArray, destinationIndex, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual void CopyTo(int sourceIndex, ArrayType[] destination, int destinationIndex, int Length)
        {
            for (int i = 0; i < Length; i++)
                destination[i + destinationIndex] = this[i + sourceIndex];
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual void CopyTo(int sourceIndex, IArray<ArrayType> destination, int destinationIndex, int Length)
        {
            var Ar = new ArrayType[Length];
            CopyTo(sourceIndex, Ar, 0, Length);
            CopyFrom(0, Ar, destinationIndex, Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual void CopyFrom(int sourceIndex, ArrayType[] sourceArray, int destinationIndex, int Length)
        {
            for (int i = 0; i < Length; i++)
                this[i + destinationIndex] = sourceArray[i + sourceIndex];
        }

        public virtual IEnumerator<ArrayType> GetEnumerator()
        {
            for (int i = 0; i < Length; i++)
                yield return this[i];
        }
        IEnumerator IEnumerable.GetEnumerator()=>this.GetEnumerator();


        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public ArrayType[] ToArray()
        {
            var NewAr = new ArrayType[Length];
            CopyTo(0, NewAr, 0, Length);
            return NewAr;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static implicit operator ArrayType[](IArray<ArrayType> ar)
        {
            return ar.ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public abstract void DeleteByPosition(int Position);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual void DeleteFrom(int from)
        {
            while (Length > from)
                DeleteByPosition(Length - 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void DeleteFromTo(int from, int To)
        {
            CopyTo(To + 1, this, from, Length - (To + 1));
            DeleteFrom(Length - (To - from + 1));
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void DeleteTo(int To)
        {
            CopyTo(To + 1, this, 0, Length - (To + 1));
            DeleteFrom(Length - (To + 1));
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public ArrayType Pop()
        {
            var Item = this[Length - 1];
            DeleteFrom(Length - 1);
            return Item;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public ArrayType Pop(int Position)
        {
            if (Position == Length - 1)
                return Pop();
            var Item = this[Position];
            DeleteByPosition(Position);
            return Item;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public ArrayType BinaryPop(ArrayType Key)
        {
            var Result = BinarySearch(Key);
            DeleteByPosition(Result.Index);
            return Result.Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual void Insert(ArrayType Value)
        {
            Insert(Value, Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual void Insert(int From,IArray<ArrayType> sourceArray, int sourceIndex, int length)
        {
            var Len = sourceArray.Length;
            for (int i = 0; i < Len; i++)
                Insert(sourceArray[i]);
        }        


        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void Insert(int From, IEnumerable<ArrayType> Values, int sourceIndex, int length) =>
            Insert(From, Values.Skip(sourceIndex - 1).Take(length));

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void Insert(IEnumerable<ArrayType> Values)=> Insert(Length,Values);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual void Insert(int From, IEnumerable<ArrayType> Values)
        {
            foreach (var Value in Values)
            {
                this.Insert(Value, From++);
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void Insert(params ArrayType[] Values) =>
            Insert(Length,Values,0,Values.Length);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual void Insert(int From, ArrayType[] Values, int Values_From, int ValuesLen) =>
            Insert(From, (IEnumerable<ArrayType>)Values, Values_From, ValuesLen);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public abstract void Insert(ArrayType Value, int Position);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void DropFromInsertTo(int From, int To, ArrayType Value)
        {
            if (From < To)
            {
                CopyTo(From + 1, this, From, (To - From));
            }
            else if (From > To)
            {
                CopyTo(To, this, To + 1, (From - To));
            }
            this[To] = Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void DropFromInsertTo(int From, int To)
        {
            var Value = this[From];
            if (From < To)
            {
                CopyTo(From + 1, this, From, (To - From));
            }
            else if (From > To)
            {
                CopyTo(To, this, To + 1, (From - To));
            }
            this[To] = Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        System.Array IArray.ToArray()
        {
            return ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        void IArray.Insert(System.Array Array)
        {
            Insert((ArrayType[])Array);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public t[] Browse<t>(Func<ArrayType, t> Selector)
        {
            var Result = new t[Length];
            for (int i = 0; i < Length; i++)
                Result[i] = Selector(this[i]);
            return Result;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public ArrayType[] Browse(Func<ArrayType, bool> Selector)
        {
            var Result = new ArrayType[Length];
            var Count = 0;
            foreach (var Value in this)
            {
                if (Selector(Value))
                {
                    Result[Count] = Value;
                    Count++;
                }
            }
            System.Array.Resize(ref Result, Count);
            return Result;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void Browse(Action<ArrayType> Visitor)
        {
            foreach (var Value in this)
                Visitor(Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int Count(Func<ArrayType, bool> Selector)
        {
            var Count = 0;
            foreach (var Value in this)
                if (Selector(Value))
                    Count++;
            return Count;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual (int Index, ArrayType Value) BinaryDelete(ArrayType Value)
        {
            var Place = BinarySearch(Value, 0, Length);
            var Index = Place.Index;
            if (Index >= 0)
            {
                DeleteByPosition(Index);
            }
            return Place;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void BinaryDelete(IEnumerable<ArrayType> Values)
        {
            foreach (var Value in Values)
                BinaryDelete(Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual int BinaryInsert(ArrayType Value)
        {
            var Place = BinarySearch(Value, 0, Length).Index;
            if (Place < 0)
                Place =~Place;
            Insert(Value, Place);
            return Place;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int BinaryInsert(ref ArrayType Value)
        {
            var Place = BinarySearch(Value, 0, Length);
            var Index = Place.Index;
            if (Index < 0)
            {
                Insert(Value, ~Index);
                return Index;
            }
            Value = Place.Value;
            return Index;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void BinaryInsert(params ArrayType[] Values)
        {
            for (int i = 0; i < Values.Length; i++)
            {
                BinaryInsert(Values[i]);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void BinaryInsert(IEnumerable<ArrayType> Values)
        {
            foreach (var Value in Values)
            {
                BinaryInsert(Value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public (int Index, ArrayType Value) BinarySearch(ArrayType key)
        {
            return BinarySearch(key, 0, Length);
        }

        public IComparer<ArrayType> Comparer = Comparer<ArrayType>.Default;

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public virtual (int Index, ArrayType Value) BinarySearch(ArrayType key, int minNum, int maxNum)
        {
            var Comparer = this.Comparer;
            maxNum = minNum + maxNum - 1;
            int mid = 0;
            ArrayType Value;
            while (minNum <= maxNum)
            {
                mid = (minNum + maxNum) / 2;
                Value = this[mid];
                var cmp =Comparer.Compare(Value,key);
                if (cmp == 0)
                {
                    return (mid, Value);
                }
                else if (cmp > 0)
                {
                    maxNum = mid - 1;
                }
                else
                {
                    minNum = mid + 1;
                }
            }
            return ((minNum + 1) * -1, default);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void shiftEnd(int Count) =>
            shiftEnd(0, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void shiftBegin(int Count) =>
            shiftBegin(0, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void shiftRollEnd(int Count) =>
            shiftRollEnd(0, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void shiftRollBegin(int Count) =>
            shiftRollBegin(0, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public ArrayType[] shiftExtraEnd(int Count) =>
            shiftExtraEnd(0, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public ArrayType[] shiftExtraBegin(int Count) =>
            shiftExtraBegin(0, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void shiftEndFrom(int From, int Count) =>
            shiftEnd(From, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void shiftBeginFrom(int From, int Count) =>
            shiftBegin(From, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void shiftRollEndFrom(int From, int Count) =>
            shiftRollEnd(From, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void shiftRollBeginFrom(int From, int Count) =>
            shiftRollBegin(From, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public ArrayType[] shiftExtraEndFrom(int From, int Count) =>
            shiftExtraEnd(From, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public ArrayType[] shiftExtraBeginFrom(int From, int Count) =>
            shiftExtraBegin(From, Length - 1, Count);


        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void shiftEnd(int From, int To, int Count)
        {
            var ArLen = (To - From) + 1;
            if (Count == ArLen)
                return;
            _shiftEnd(From, Count, ArLen);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private void _shiftEnd(int From, int Count, int ArLen)
        {
            Copy(this, From, this, From + Count, ArLen - Count);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void shiftBegin(int From, int To, int Count)
        {
            var ArLen = (To - From) + 1;
            if (Count == ArLen)
                return;
            _shiftBegin(From, Count, ArLen);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private void _shiftBegin(int From, int Count, int ArLen)
        {
            Copy(this, From + Count, this, From, ArLen - Count);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public ArrayType[] shiftExtraEnd(int From, int To, int Count)
        {
            var ArLen = (To - From) + 1;
            if (Count == ArLen)
            {
                var Roll = new ArrayType[Count];
                Copy(this, From, Roll, 0, Count);
                return Roll;
            }
            return _shiftExtraEnd(From, To, Count, ArLen);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private ArrayType[] _shiftExtraEnd(int From, int To, int Count, int ArLen)
        {
            var Roll = new ArrayType[Count];
            Copy(this, To + 1 - Count, Roll, 0, Count);
            _shiftEnd(From, Count, ArLen);
            return Roll;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public ArrayType[] shiftExtraBegin(int From, int To, int Count)
        {
            var ArLen = (To - From) + 1;
            if (Count == ArLen)
            {
                var Roll = new ArrayType[Count];
                Copy(this, From, Roll, 0, Count);
                return Roll;
            }
            return _shiftExtraBegin(From, To, Count, ArLen);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private ArrayType[] _shiftExtraBegin(int From, int To, int Count, int ArLen)
        {
            var Roll = new ArrayType[Count];
            Copy(this, From, Roll, 0, Count);
            _shiftBegin(From, Count, ArLen);
            return Roll;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void shiftRollEnd(int From, int To, int Count)
        {
            var ArLen = (To - From) + 1;
            if (Count == ArLen)
                return;
            var Roll = _shiftExtraEnd(From, To, Count, ArLen);
            Copy(Roll, 0, this, From, Count);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void shiftRollBegin(int From, int To, int Count)
        {
            var ArLen = (To - From) + 1;
            if (Count == ArLen)
                return;
            var Roll = _shiftExtraBegin(From, To, Count, ArLen);
            To = To + 1 - Count;
            Copy(Roll, 0, this, To, Count);
        }

        public Func<IArray<ArrayType>> MakeSameNew=()=>
            throw new NotImplementedException($"MakeSameNew in array of {typeof(ArrayType)} not implementd!");

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        protected virtual IArray<ArrayType> _MakeSameNew() => MakeSameNew();

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public IArray<ArrayType> Copy()
        {
            var NewAr = _MakeSameNew();
            NewAr.Insert(this);
            return NewAr;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public IArray<ArrayType> From(int from)
        {
            var Result = _MakeSameNew();
            Result.Insert(0, this, from, Length - from);
            return Result;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public IArray<ArrayType> To(int to)
        {
            var Result = _MakeSameNew();
            Result.Insert(0, this, 0, to + 1);
            return Result;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public IArray<ArrayType> FromTo(int from, int to)
        {
            var Result = _MakeSameNew();
            Result.Insert(0, this, from, this.Length);
            return Result;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public IArray<ArrayType> PopFrom(int From)
        {
#if DEBUG
            if (From > this.Length)
                throw new Exception("From Bigger Than Len");
#endif
            var Result = this.From(From);
            DeleteFrom(From);
            return Result;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public IArray<ArrayType> PopTo(int to)
        {
            var Result = this.To(to);
            DeleteTo(to);
            return Result;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public IArray<ArrayType> PopFromTo(int From, int To)
        {
            var Result = this.From(From).To(To);
            DeleteFromTo(From, To);
            return Result;
        }
    }

    public abstract class IArray<ArrayType, OwnerType> :
        IArray<ArrayType>
        where OwnerType : IArray<ArrayType, OwnerType>
    {
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        protected abstract new OwnerType MakeSameNew();

        protected override IArray<ArrayType> _MakeSameNew()=> MakeSameNew();

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public new OwnerType Copy() => (OwnerType)base.Copy();

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public new OwnerType From(int from) => (OwnerType)base.From(from);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public new OwnerType To(int to)=>(OwnerType) base.To(to);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public new OwnerType FromTo(int from, int to) => (OwnerType)base.FromTo(from, to);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public new OwnerType PopFrom(int From) => (OwnerType)base.PopFrom(From);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public new OwnerType PopTo(int to) => (OwnerType)base.PopTo(to);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public new OwnerType PopFromTo(int From, int To) => (OwnerType)base.PopFromTo(From, To);
    }
}