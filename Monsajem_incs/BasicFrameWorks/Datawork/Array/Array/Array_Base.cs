using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Monsajem_Incs.Collection.Array.ArrayBased.Base
{
    public abstract partial class IArray<ArrayType> :
        Array.Base.IArray<ArrayType>
    {
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        internal abstract void AddLength(int Count);


        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public override void DeleteByPosition(int Position)
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

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static void Copy(
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

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static void Copy(
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


        public void CopyTo(int sourceIndex, IArray<ArrayType> destination, int destinationIndex, int Length)
        {
            Copy(this,sourceIndex, destination, destinationIndex, Length);
        }

        public override void CopyTo(int sourceIndex, Array.Base.IArray<ArrayType> destination, int destinationIndex, int Length)
        {
            if (destination.GetType().IsAssignableTo(typeof(IArray<ArrayType>)))
                CopyTo(sourceIndex,(IArray<ArrayType>) destination, destinationIndex, Length);
            else
                base.CopyTo(sourceIndex, destination, destinationIndex, Length);
        }

        public override void CopyTo(int sourceIndex, ArrayType[] destination, int destinationIndex, int Length)
        {
            Copy(this, sourceIndex, destination, destinationIndex, Length);
        }

        protected virtual IEnumerator<ArrayType> _GetEnumerator()
        {
            return new MyEnum() { ar = this.GetAllArrays().Ar };
        }

        public override IEnumerator<ArrayType> GetEnumerator() => _GetEnumerator();

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

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
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

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public override void Insert(int From, Array.Base.IArray<ArrayType> sourceArray, int sourceIndex, int length)
        { 
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
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

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public override void Insert(int From,ArrayType[] Values,int Values_From,int ValuesLen)
        {
            var ArLen = Length;
            AddFromTo(From,Values,Values_From, ValuesLen);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public override void Insert(int From ,IEnumerable<ArrayType> Values)
        {
            var ArLen = Length;
            var Count = Values.Count();
            AddLength(Count);
            shiftEndFrom(From,Count);
            var i = From;
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

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public override void Insert(ArrayType Value, int Position)
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

        internal virtual System.Array GetArrayFrom(int From, out int Ar_From, out int Ar_Len)
        {
            var Result = new ArrayType[Length - From];
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
    }

    public abstract class IArray<ArrayType, OwnerType> :
    IArray<ArrayType>
    where OwnerType : IArray<ArrayType, OwnerType>
    {
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        protected abstract new OwnerType MakeSameNew();

        protected override IArray<ArrayType> _MakeSameNew() => MakeSameNew();

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public new OwnerType Copy() => (OwnerType)base.Copy();

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public new OwnerType From(int from) => (OwnerType)base.From(from);

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public new OwnerType To(int to) => (OwnerType)base.To(to);

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