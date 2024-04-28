using System;
using System.Runtime.CompilerServices;

namespace Monsajem_Incs.Collection.Array.ArrayBased.OneArrayBase
{
    public abstract class Array<ArrayType, OwnerType> :
        Base.IArray<ArrayType, OwnerType>
        where OwnerType : Array<ArrayType, OwnerType>
    {
        public ArrayType[] ar;

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public Array()
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public Array(ArrayType[] ar)
        {
            this.ar = ar;
        }

        public override ArrayType this[int Pos]
        {
            [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
            get => ar[Pos];
            [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
            set => ar[Pos] = value;
        }

        internal override System.Array GetArrayFrom(int From, out int Ar_From, out int Ar_Len)
        {
            Ar_From = From;
            Ar_Len = Length;
            return ar;
        }

        internal override System.Array GetArrayPos(int Ar_Pos, out int Ar_From, out int Ar_Len)
        {
            if (Ar_Pos > 0)
                throw new IndexOutOfRangeException("Position of array is wrong!");
            Ar_From = 0;
            Ar_Len = Length;
            return ar;
        }

        internal override void SetFromTo(int From, System.Array Ar, int Ar_From, int Ar_Len)
        {
            System.Array.Copy(Ar, Ar_From, ar, From, Ar_Len);
        }
    }
}