using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Monsajem_Incs.Array.OneArrayBase
{
    internal abstract class Array<ArrayType,OwnerType> :
        Base.IArray<ArrayType, OwnerType>
        where OwnerType: Array<ArrayType, OwnerType>
    {
        public ArrayType[] ar;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Array()
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Array(ArrayType[] ar)
        {
            this.ar = ar;
        }

        public override ArrayType this[int Pos]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ar[Pos];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => ar[Pos] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public new Array<t> Browse<t>(Func<ArrayType, t> Selector)
        {
            var Result = new Array<t>();
            Result.Insert(base.Browse(Selector));
            return Result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public new Array<ArrayType> Browse(Func<ArrayType, bool> Selector)
        {
            var Result = new Array<ArrayType>();
            Result.Insert(base.Browse(Selector));
            return Result;
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
            System.Array.Copy(Ar, Ar_From, this.ar, From, Ar_Len);
        }
    }
}