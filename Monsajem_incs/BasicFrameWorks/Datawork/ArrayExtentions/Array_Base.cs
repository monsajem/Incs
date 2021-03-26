using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Monsajem_Incs.Array.Base
{
    internal interface IArray
    {
        System.Array ToArray();
        object MyOptions { get; set; }
        Type ElementType { get; }
        void Insert(System.Array Array);
        System.Array GetArrayFrom(int From,out int Ar_From,out int Ar_Len);
        System.Array GetArrayPos(int Ar_Pos, out int Ar_From, out int Ar_Len);
        void SetFromTo(int From, System.Array Ar, int Ar_From, int Ar_Len);
        void AddFromTo(int From, System.Array Ar, int Ar_From, int Ar_Len);
        ((int From, int Len, System.Array Ar)[] Ar, int MaxLen) GetAllArrays();
        void SetAllArrays(((int From, int Len, System.Array Ar)[] Ar, int MaxLen) Ar);
        void SetLen(int Len);
    }

    public abstract partial class IArray<ArrayType> :
        IArray,IEnumerable<ArrayType>
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IArray.SetLen(int Len) => Length = Len;
        public virtual int Length { get; internal set; }

        public abstract object MyOptions { get; set; }

        public Type ElementType => typeof(ArrayType);

        object IArray.MyOptions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        Type IArray.ElementType => throw new NotImplementedException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal abstract void AddLength(int Count);

        public abstract ArrayType this[int Pos] {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Clear()
        {
            DeleteFromTo(0, Length - 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void Copy(
            IArray<ArrayType> sourceArray,
            int sourceIndex,
            IArray<ArrayType> destinationArray,
            int destinationIndex,
            int length)
        {
            int Source_From; int Source_Len;
            var Source_Ar = sourceArray.GetArrayFrom(sourceIndex, out Source_From, out Source_Len);
            if (Source_Len >= length)
            {
                destinationArray.SetFromTo(destinationIndex, Source_Ar, Source_From, length);
                return;
            }
            else
                destinationArray.SetFromTo(destinationIndex, Source_Ar, Source_From, Source_Len);
            var Ar_Pos = 1;
            while(true)
            {
                length -= Source_Len;
                destinationIndex += Source_Len;
                Source_Ar = sourceArray.GetArrayPos(Ar_Pos++, out Source_From, out Source_Len);
                if (Source_Len >= length)
                {
                    destinationArray.SetFromTo(destinationIndex, Source_Ar, Source_From, length);
                    return;
                }
                destinationArray.SetFromTo(destinationIndex, Source_Ar, Source_From, Source_Len);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void Copy(
            IArray<ArrayType> sourceArray,
            int sourceIndex,
            ArrayType[] destinationArray,
            int destinationIndex,
            int length)
        {
            int Source_From; int Source_Len;
            var Source_Ar = sourceArray.GetArrayFrom(sourceIndex, out Source_From, out Source_Len);
            if (Source_Len >= length)
            {
                System.Array.Copy(Source_Ar, Source_From, destinationArray, destinationIndex, length);
                return;
            }
            else
                System.Array.Copy(Source_Ar, Source_From, destinationArray, destinationIndex, Source_Len);
            var Ar_Pos = 1;
            while (true)
            {
                length -= Source_Len;
                destinationIndex += Source_Len;
                Source_Ar = sourceArray.GetArrayPos(Ar_Pos++, out Source_From, out Source_Len);
                if (Source_Len >= length)
                {
                    System.Array.Copy(Source_Ar, Source_From, destinationArray, destinationIndex, length);
                    return;
                }
                System.Array.Copy(Source_Ar, Source_From, destinationArray, destinationIndex, Source_Len);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void Copy(
            ArrayType[] sourceArray,
            int sourceIndex,
            IArray<ArrayType> destinationArray,
            int destinationIndex,
            int length)
        {
            destinationArray.SetFromTo(destinationIndex, sourceArray, sourceIndex, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void CopyTo(int From, ArrayType[] destination, int destinationIndex, int Length)
        {
            Copy(this, From, destination, destinationIndex, Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void CopyTo(int From, IArray<ArrayType> destination, int destinationIndex, int Length)
        {
            Copy(this, From, destination, destinationIndex, Length);
        }

        public IEnumerator<ArrayType> GetEnumerator()
        {
            return new MyEnum() { ar = this.GetAllArrays().Ar };
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private class MyEnum : IEnumerator<ArrayType>
        {
            private (int From, int To, System.Array Ar)[] _ar;
            public (int From, int To, System.Array Ar)[] ar
            {
                set 
                { 
                    _ar = value;
                    var AR = _ar[0];
                    CurrnetAr = (ArrayType[])AR.Ar;
                    CurrnetLen = AR.To;
                    CurrentPos = AR.From - 1;
                    ArLastPos = _ar.Length - 1;
                }
            }
            private ArrayType[] CurrnetAr;
            private int CurrentPos;
            private int CurrnetLen;
            private int ArPos;
            private int ArLastPos;
            public ArrayType Current => CurrnetAr[CurrentPos];

            object IEnumerator.Current => CurrnetAr[CurrentPos];

            public void Dispose()
            {
                System.GC.SuppressFinalize(this);
            }

            public bool MoveNext()
            {
                CurrentPos++;
                var Result = CurrentPos < CurrnetLen;
                if(Result == false)
                {
                    ArPos++;
                    if (ArPos > ArLastPos)
                        return false;
                    var AR = _ar[ArPos];
                    CurrnetAr = (ArrayType[])AR.Ar;
                    CurrnetLen = AR.To;
                    CurrentPos = AR.From;
                    Result = CurrentPos < CurrnetLen;
                }
                return Result;
            }

            public void Reset()
            {
                ArPos = 0;
                CurrnetAr = (ArrayType[])_ar[0].Ar;
                CurrnetLen = CurrnetAr.Length;
                CurrentPos = -1;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal virtual ((int From, int Len, System.Array Ar)[] Ar, int MaxLen)
            GetAllArrays()
        {
            return (new (int From, int Len, System.Array Ar)[] { (0, Length, this.ToArray()) },Length);
        }

        internal virtual void SetAllArrays
            (((int From, int Len, System.Array Ar)[] Ar, int MaxLen) Ar)
        {
            Clear();
            var From = 0;
            for(int i=0;i<Ar.Ar.Length;i++)
            {
                var CurrentAr = Ar.Ar[i];
                AddFromTo(From, CurrentAr.Ar, CurrentAr.From, CurrentAr.Len);
                From += CurrentAr.Len;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArrayType[] ToArray()
        {
            var NewAr = new ArrayType[Length];
            CopyTo(0, NewAr, 0, Length);
            return NewAr;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ArrayType[](IArray<ArrayType> ar)
        {
            return ar.ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void DeleteByPosition(int Position)
        {
            if (Position == Length - 1)
            {
                DeleteFrom(Position);
                return;
            }
            //System.Array.Copy(ar, 0, ar, 0, Position);
            CopyTo(Position + 1, this, Position, (Length - Position) - 1);
            DeleteFrom(Length - 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void DeleteFrom(int from);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteFromTo(int from, int To)
        {
            CopyTo(To + 1, this, from, Length - (To + 1));
            DeleteFrom(Length - (To - from + 1));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteTo(int To)
        {
            CopyTo(To + 1, this, 0, Length - (To + 1));
            DeleteFrom(Length - (To + 1));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArrayType Pop()
        {
            var Item = this[Length - 1];
            DeleteFrom(Length - 1);
            return Item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Insert(ArrayType Value)
        {
            Insert(Value, Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(int From,IArray<ArrayType> sourceArray, int sourceIndex, int length)
        {
            int Source_From; int Source_Len;
            var Source_Ar = sourceArray.GetArrayFrom(sourceIndex, out Source_From, out Source_Len);
            if (Source_Len >= length)
            {
                AddFromTo(From,Source_Ar, Source_From,length);
                return;
            }
            else
                AddFromTo(From, Source_Ar, Source_From, Source_Len);
            var Ar_Pos = 1;
            while (true)
            {
                length -= Source_Len;
                From += Source_Len;
                Source_Ar = sourceArray.GetArrayPos(Ar_Pos++, out Source_From, out Source_Len);
                if (Source_Len >= length)
                {
                    AddFromTo(From, Source_Ar, Source_From, length);
                    return;
                }
                AddFromTo(From, Source_Ar, Source_From, Source_Len);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(params ArrayType[] Values)
        {
            var From = Length;
            AddFromTo(Length,Values,0,Values.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(IEnumerable<ArrayType> Values)
        {
            var From = Length;
            var Count = Values.Count();
            AddLength(Count);
            var i = From;
            Count = Length;
            var Reader = Values.GetEnumerator();
            Reader.MoveNext();
            while (i < Count)
            {
                this[i] = Reader.Current;
                Reader.MoveNext();
                i++;
            }
            Reader.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(int From,ArrayType[] Values,int Values_From,int ValuesLen)
        {
            var ArLen = Length;
            AddFromTo(From,Values,Values_From, ValuesLen);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(IEnumerable<ArrayType> Values, int From)
        {
            var ArLen = Length;
            var Count = Values.Count();
            AddLength(Count);
            CopyTo(From, this, ArLen + 1, ArLen - From);
            var i = From;
            Count = Length;
            var Reader = Values.GetEnumerator();
            Reader.MoveNext();
            while (i < Count)
            {
                this[i] = Reader.Current;
                Reader.MoveNext();
                i++;
            }
            Reader.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Insert(ArrayType Value, int Position)
        {
            AddLength(1);
            if (Position == Length)
                this[Position] = Value;
            else
            {
                shiftEnd(Position, Length - 1, 1);
                this[Position] = Value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        System.Array IArray.ToArray()
        {
            return ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IArray.Insert(System.Array Array)
        {
            Insert((ArrayType[])Array);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public t[] Browse<t>(Func<ArrayType, t> Selector)
        {
            var Result = new t[Length];
            for (int i = 0; i < Length; i++)
                Result[i] = Selector(this[i]);
            return Result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Browse(Action<ArrayType> Visitor)
        {
            foreach (var Value in this)
                Visitor(Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Count(Func<ArrayType, bool> Selector)
        {
            var Count = 0;
            foreach (var Value in this)
                if (Selector(Value))
                    Count++;
            return Count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (int Index, ArrayType Value) BinaryDelete(ArrayType Value)
        {
            var Place = BinarySearch(Value, 0, Length);
            var Index = Place.Index;
            if (Index >= 0)
            {
                DeleteByPosition(Index);
            }
            return Place;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BinaryDelete(IEnumerable<ArrayType> Values)
        {
            foreach (var Value in Values)
                BinaryDelete(Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int BinaryInsert(ArrayType Value)
        {
            var Place = BinarySearch(Value, 0, Length).Index;
            if (Place < 0)
                Place =~Place;
            Insert(Value, Place);
            return Place;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BinaryInsert(params ArrayType[] Values)
        {
            for (int i = 0; i < Values.Length; i++)
            {
                BinaryInsert(Values[i]);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BinaryInsert(IEnumerable<ArrayType> Values)
        {
            foreach (var Value in Values)
            {
                BinaryInsert(Value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (int Index, ArrayType Value) BinarySearch(ArrayType key)
        {
            return BinarySearch(key, 0, Length);
        }

        private static Comparer<ArrayType> DefaultComparer =
            ((Func<Comparer<ArrayType>>)(()=> {
                if(typeof(IComparable<ArrayType>).IsAssignableFrom(typeof(ArrayType)))
                {
                    return Comparer<ArrayType>.Create((c1,c2) =>
                    {
                        return ((IComparable<ArrayType>)c1).CompareTo(c2);
                    });
                }
                return null;
            }))();
        protected IComparer<ArrayType> Comparer = DefaultComparer;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (int Index, ArrayType Value) BinarySearch(ArrayType key, int minNum, int maxNum)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void shiftEnd(int Count) =>
            shiftEnd(0, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void shiftBegin(int Count) =>
            shiftBegin(0, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void shiftRollEnd(int Count) =>
            shiftRollEnd(0, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void shiftRollBegin(int Count) =>
            shiftRollBegin(0, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArrayType[] shiftExtraEnd(int Count) =>
            shiftExtraEnd(0, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArrayType[] shiftExtraBegin(int Count) =>
            shiftExtraBegin(0, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void shiftEndFrom(int From, int Count) =>
            shiftEnd(From, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void shiftBeginFrom(int From, int Count) =>
            shiftBegin(From, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void shiftRollEndFrom(int From, int Count) =>
            shiftRollEnd(From, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void shiftRollBeginFrom(int From, int Count) =>
            shiftRollBegin(From, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArrayType[] shiftExtraEndFrom(int From, int Count) =>
            shiftExtraEnd(From, Length - 1, Count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArrayType[] shiftExtraBeginFrom(int From, int Count) =>
            shiftExtraBegin(From, Length - 1, Count);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void shiftEnd(int From, int To, int Count)
        {
            var ArLen = (To - From) + 1;
            if (Count == ArLen)
                return;
            _shiftEnd(From, Count, ArLen);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void _shiftEnd(int From, int Count, int ArLen)
        {
            Copy(this, From, this, From + Count, ArLen - Count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void shiftBegin(int From, int To, int Count)
        {
            var ArLen = (To - From) + 1;
            if (Count == ArLen)
                return;
            _shiftBegin(From, Count, ArLen);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void _shiftBegin(int From, int Count, int ArLen)
        {
            Copy(this, From + Count, this, From, ArLen - Count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ArrayType[] _shiftExtraEnd(int From, int To, int Count, int ArLen)
        {
            var Roll = new ArrayType[Count];
            Copy(this, To + 1 - Count, Roll, 0, Count);
            _shiftEnd(From, Count, ArLen);
            return Roll;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ArrayType[] _shiftExtraBegin(int From, int To, int Count, int ArLen)
        {
            var Roll = new ArrayType[Count];
            Copy(this, From, Roll, 0, Count);
            _shiftBegin(From, Count, ArLen);
            return Roll;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void shiftRollEnd(int From, int To, int Count)
        {
            var ArLen = (To - From) + 1;
            if (Count == ArLen)
                return;
            var Roll = _shiftExtraEnd(From, To, Count, ArLen);
            Copy(Roll, 0, this, From, Count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void shiftRollBegin(int From, int To, int Count)
        {
            var ArLen = (To - From) + 1;
            if (Count == ArLen)
                return;
            var Roll = _shiftExtraBegin(From, To, Count, ArLen);
            To = To + 1 - Count;
            Copy(Roll, 0, this, To, Count);
        }

        internal virtual System.Array GetArrayFrom(int From, out int Ar_From, out int Ar_Len)
        {
            var Result = new ArrayType[(Length - From)+1];
            Ar_From = 0;
            Ar_Len = Result.Length;
            for (int i = 0; i < Ar_Len; i++)
                Result[i] = this[i + From];
            return Result;
        }
        internal virtual System.Array GetArrayPos(int Ar_Pos, out int Ar_From, out int Ar_Len)
        {
            if (Ar_Pos > 0)
                throw new IndexOutOfRangeException("Position of array is wrong!");
            var Result = new ArrayType[Length];
            Ar_Len = Result.Length;
            for (int i = 0; i < Ar_Len; i++)
                Result[i] = this[i];
            Ar_From = 0;
            return Result;
        }
        internal virtual void SetFromTo(int From, System.Array Ar, int Ar_From, int Ar_Len)
        {
                var _Ar = (ArrayType[])Ar;
                var I_To = Ar_From + Ar_Len; 
                for (int i = Ar_From; i < I_To; i++)
                    this[From++] = _Ar[i];
        }
        internal virtual void AddFromTo(int From, System.Array Ar, int Ar_From, int Ar_Len)
        {
            var OldCount = Length;
            AddLength(Ar_Len);
            shiftEndFrom(From, 1);
            SetFromTo(From, Ar,Ar_From,Ar_Len);
        }

        System.Array IArray.GetArrayFrom(int From, out int Ar_From, out int Ar_Len) => GetArrayFrom(From,out Ar_From,out Ar_Len);

        System.Array IArray.GetArrayPos(int Ar_Pos, out int Ar_From, out int Ar_Len) => GetArrayPos(Ar_Pos, out Ar_From, out Ar_Len);

        void IArray.SetFromTo(int From, System.Array Ar, int Ar_From, int Ar_Len) => SetFromTo(From, Ar, Ar_From, Ar_Len);

        void IArray.AddFromTo(int From, System.Array Ar, int Ar_From, int Ar_Len) => AddFromTo(From, Ar, Ar_From, Ar_Len);

        ((int From, int Len, System.Array Ar)[] Ar, int MaxLen) IArray.GetAllArrays() => GetAllArrays();

        void IArray.SetAllArrays(((int From, int Len, System.Array Ar)[] Ar, int MaxLen) Ar) => SetAllArrays(Ar);
    }

    public abstract class IArray<ArrayType, OwnerType> :
        IArray<ArrayType>
        where OwnerType : IArray<ArrayType, OwnerType>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract OwnerType MakeSameNew();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OwnerType Copy()
        {
            var NewAr = MakeSameNew();
            NewAr.Insert(this);
            return NewAr;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OwnerType From(int from)
        {
            var Result = MakeSameNew();
            Result.Insert(this,0);
            return Result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OwnerType To(int to)
        {
            var Result = MakeSameNew();
            Result.Insert(0,this,0,to+1);
            return Result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OwnerType FromTo(int from, int to)
        {
            var Result = MakeSameNew();
            Result.Insert(0,this,from,this.Length);
            return Result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OwnerType PopFrom(int From)
        {
#if DEBUG
            if (From > this.Length)
                throw new Exception("From Bigger Than Len");
#endif
            var Result = this.From(From);
            DeleteFrom(From);
            return Result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OwnerType PopTo(int to)
        {
            var Result = this.To(to);
            DeleteTo(to);
            return Result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OwnerType PopFromTo(int From, int To)
        {
            var Result = this.From(From).To(To);
            DeleteFromTo(From, To);
            return Result;
        }
    }
}