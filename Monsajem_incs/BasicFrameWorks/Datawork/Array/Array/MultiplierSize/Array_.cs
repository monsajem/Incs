using System;
using System.Runtime.CompilerServices;

namespace Monsajem_Incs.Collection.Array.ArrayBased.MultiplierSize
{
    internal class Array<ArrayType> :
        OneArrayBase.Array<ArrayType, Array<ArrayType>>
    {
        public int MinLen;
        public int MaxLen;

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public Array()
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public Array(ArrayType[] ar)
        {
            Length = ar.Length;
            this.ar = ar;
        }

        public override object MyOptions
        {
            get => null;
            set { }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public override void DeleteFrom(int from)
        {
            Length = from;
            from = Length + 1000;
            if (Length < MinLen)
            {
                MaxLen = from * 2;
                MinLen = from / 2;
                System.Array.Resize(ref ar, MaxLen);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        internal override void AddLength(int Count)
        {
            Length += Count;
            Count = Length + 1000;
            if (Length > MaxLen)
            {
                MaxLen = Count * 2;
                MinLen = Count / 2;
                System.Array.Resize(ref ar, MaxLen);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public new Array<t> Browse<t>(Func<ArrayType, t> Selector)
        {
            var Result = new Array<t>();
            Result.Insert(base.Browse(Selector));
            return Result;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public new Array<ArrayType> Browse(Func<ArrayType, bool> Selector)
        {
            var Result = new Array<ArrayType>();
            Result.Insert(base.Browse(Selector));
            return Result;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static implicit operator ArrayType[](Array<ArrayType> ar)
        {
            var NewAr = new ArrayType[ar.Length];
            System.Array.Copy(ar.ar, 0, NewAr, 0, ar.Length);
            return NewAr;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        protected override Array<ArrayType> MakeSameNew()
        {
            return new Array<ArrayType>();
        }
    }
}