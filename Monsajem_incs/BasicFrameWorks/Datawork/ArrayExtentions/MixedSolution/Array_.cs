using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Monsajem_Incs.Array
{
    public class Array<ArrayType>:
        Base.IArray<ArrayType, Array<ArrayType>>
    {
        private Base.IArray<ArrayType> ar;
        private readonly static bool IsReferenced =
            ((Func<bool>)(() =>
            {
                try
                {
                    System.Runtime.InteropServices.Marshal.SizeOf(typeof(ArrayType));
                    return false;
                }
                catch
                {
                    return true;
                }
            }))();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Base.IArray<ArrayType> MakeNewAR(ArrayType[] ar = null)
        {
            if (IsReferenced)
            {
                if (ar == null)
                    return new Hyper.Array<ArrayType>();
                else
                    return new Hyper.Array<ArrayType>(ar);
            }
            else
            {
                if (ar == null)
                    return new MultiplierSize.Array<ArrayType>();
                else
                    return new MultiplierSize.Array<ArrayType>(ar);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Array(ArrayType[] ar) =>
            this.ar = MakeNewAR(ar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Array() =>
            this.ar = MakeNewAR();

        public override object MyOptions
        {
            get => null;
            set { }
        }

        public override int Length { get => ar.Length; internal set => ar.Length = value; }

        public override ArrayType this[int Pos]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ar[Pos];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => ar[Pos] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal override void AddLength(int Count)
        {
            Length = Length + Count;
            ar.AddLength(Count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Clear() => ar.Clear();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void DeleteByPosition(int Position) => ar.DeleteByPosition(Position);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Insert(ArrayType Value) => ar.Insert(Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Insert(ArrayType Value, int Position) => ar.Insert(Value,Position);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void DeleteFrom(int from) =>ar.DeleteFrom(from);

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


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ArrayType[](Array<ArrayType> ar)
        {
            var NewAr = new ArrayType[ar.Length];
            System.Array.Copy(ar.ar, 0, NewAr, 0, ar.Length);
            return NewAr;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override Array<ArrayType> MakeSameNew()
        {
            return new Array<ArrayType>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal override System.Array GetArrayFrom(int From, out int Ar_From, out int Ar_Len) =>
            ar.GetArrayFrom(From, out Ar_From, out Ar_Len);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal override System.Array GetArrayPos(int Ar_Pos, out int Ar_From, out int Ar_Len) =>
            ar.GetArrayPos(Ar_Pos, out Ar_From, out Ar_Len);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal override void SetFromTo(int From, System.Array Ar, int Ar_From, int Ar_Len) =>
            ar.SetFromTo(From, Ar, Ar_From, Ar_Len);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal override void AddFromTo(int From, System.Array Ar, int Ar_From, int Ar_Len) =>
            ar.AddFromTo(From, Ar, Ar_From, Ar_Len);
    }
}