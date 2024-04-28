using System;

namespace Monsajem_Incs.Collection.Array.ArrayBased.DynamicSize
{
    public class Array<ArrayType> :
        OneArrayBase.Array<ArrayType, Array<ArrayType>>
    {
        public int MinLen;
        public int MaxLen;
        public int MinCount;

        public Array() : this(500) { }

        public Array(int MinCount)
        {
            SetMyOptions(MinCount);
        }

        public Array(ArrayType[] ar, int MinCount = 500)
        {
            Length = ar.Length;
            this.ar = ar;
            SetMyOptions(MinCount);
        }

        private void SetMyOptions(int value)
        {
            MinCount = value;
            if (ar == null)
            {
                MinLen = Length - MinCount;
                MaxLen = Length + MinCount;
                ar = new ArrayType[MaxLen];
            }
            else
            {
                MinLen = Length;
                MaxLen = Length;
            }
        }
        public override object MyOptions
        {
            get => MinCount;
            set => SetMyOptions((int)value);
        }

        public override void DeleteFrom(int from)
        {
            Length = from;
            if (Length < MinLen)
            {
                MaxLen = Length + MinCount;
                MinLen = Length - MinCount;
                System.Array.Resize(ref ar, MaxLen);
            }
        }
        internal override void AddLength(int Count)
        {
            Length += Count;
            if (Length > MaxLen)
            {
                MaxLen = Length + MinCount;
                MinLen = Length - MinCount;
                System.Array.Resize(ref ar, MaxLen);
            }
        }

        public new Array<t> Browse<t>(Func<ArrayType, t> Selector)
        {
            var Result = new Array<t>(MinCount);
            Result.Insert(base.Browse(Selector));
            return Result;
        }
        public new Array<ArrayType> Browse(Func<ArrayType, bool> Selector)
        {
            var Result = new Array<ArrayType>(MinCount);
            Result.Insert(base.Browse(Selector));
            return Result;
        }

        public static implicit operator ArrayType[](Array<ArrayType> ar)
        {
            var NewAr = new ArrayType[ar.Length];
            System.Array.Copy(ar.ar, 0, NewAr, 0, ar.Length);
            return NewAr;
        }

        protected override Array<ArrayType> MakeSameNew()
        {
            return new Array<ArrayType>(MinCount);
        }
    }
}