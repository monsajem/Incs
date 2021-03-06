﻿using System;
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
        object Comparer { get; set; }
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

        object IArray.MyOptions { get => MyOptions; set => MyOptions=value; }

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
        
        object IArray.Comparer { get => Comparer; set => Comparer = (IComparer<ArrayType>)value; }

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