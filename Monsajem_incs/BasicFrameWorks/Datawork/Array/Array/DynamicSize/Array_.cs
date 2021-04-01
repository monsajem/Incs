using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monsajem_Incs.Collection.Array.ArrayBased.DynamicSize
{
    public class Array<ArrayType>:
        OneArrayBase.Array<ArrayType, Array<ArrayType>>
    {
        public int MinLen;
        public int MaxLen;
        public int MinCount;

        public Array() : this(500) { }

        public Array(int MinCount)
        {
            this.SetMyOptions(MinCount);
        }

        public Array(ArrayType[] ar,int MinCount=500)
        {
            Length = ar.Length;
            this.ar=ar;
            this.SetMyOptions(MinCount);
        }

        private void SetMyOptions(int value)
        {
            this.MinCount = value;
            if (ar == null)
            {
                this.MinLen = Length - MinCount;
                this.MaxLen = Length + MinCount;
                this.ar = new ArrayType[MaxLen];
            }
            else
            {
                this.MinLen = Length;
                this.MaxLen = Length;
            }
        }
        public override object MyOptions { 
            get => MinCount;
            set=> SetMyOptions((int) value);
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
            Length = Length + Count;
            if (Length > MaxLen)
            {
                MaxLen = Length + MinCount;
                MinLen = Length - MinCount;
                System.Array.Resize(ref ar, MaxLen);
            }
        }

        public new Array<t> Browse<t>(Func<ArrayType, t> Selector)
        {
            var Result = new Array<t>(this.MinCount);
            Result.Insert(base.Browse(Selector));
            return Result;
        }
        public new Array<ArrayType> Browse(Func<ArrayType, bool> Selector)
        {
            var Result = new Array<ArrayType>(this.MinCount);
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