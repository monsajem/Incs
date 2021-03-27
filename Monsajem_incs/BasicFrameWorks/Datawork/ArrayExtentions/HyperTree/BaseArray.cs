using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Monsajem_Incs.Array.Research.HyperTree
{
    public class Array<ArrayType> :
        Base.IArray<ArrayType, Array<ArrayType>>
    {
        protected override Array<ArrayType> MakeSameNew()
        {
            return new Array<ArrayType>(MaxLen);
        }

        public class ArrayInstance 
        {
            public int FromPos;
            public int MaxLen;
            public Array<ArrayType> ar;
            public ArrayType Value;
            public int BeforeLen;
            public int AfterLen;
            public int Length
            {
                get
                {
                    if (ar == null)
                        return 1;
                    else
                        return ar.Length;
                }
            }

            public void Insert(ArrayType Value, int Pos)
            {
                if (ar == null)
                {
                    ar = new Array<ArrayType>(MaxLen);
                    ar.Insert(this.Value);
                    this.Value = default;
                }
                ar.Insert(Value, Pos);
            }
            public void Insert(ArrayType Value)
            {
                if (ar == null)
                {
                    ar = new Array<ArrayType>(MaxLen);
                    ar.Insert(this.Value);
                    this.Value = default;
                }
                ar.Insert(Value);
            }
            public void Delete(int Pos)
            {
#if DEBUG
                if (ar == null)
                    throw new Exception("Not Allowd!");
#endif
                if (ar.Length == 2)
                {
                    Value = ar[Pos];
                    ar = null;
                }
                else
                    ar.DeleteByPosition(Pos);
            }

            public ArrayType this[int Pos]
            {
                [MethodImpl(MethodImplOptions.AggressiveOptimization)]
                get
                {
                    if (ar == null)
                        return Value;
                    else
                        return ar[Pos];
                }
                [MethodImpl(MethodImplOptions.AggressiveOptimization)]
                set
                {
                    if (ar == null)
                        Value = value;
                    else
                        ar[Pos] = value;
                }
            }

            public struct ICompareByFrom : IComparer<ArrayInstance>
            {
                public int Compare(ArrayInstance x, ArrayInstance y)
                {
                    return x.FromPos-y.FromPos;
                }
            }
            public struct ICompareByBeforeLen : IComparer<ArrayInstance>
            {
                public int Compare(ArrayInstance x, ArrayInstance y)
                { 
                    var Res = x.BeforeLen - y.BeforeLen;
                    if (Res == 0)
                        Res = x.FromPos - y.FromPos;
                    return Res;
                }
            }

            public struct ICompareByAfterLen : IComparer<ArrayInstance>
            {
                public int Compare(ArrayInstance x, ArrayInstance y)
                {
                    var Res = x.AfterLen - y.AfterLen;
                    if (Res == 0)
                        Res = x.FromPos - y.FromPos;
                    return Res;
                }
            }
        }
        public FixedSize.Array<ArrayInstance> ar;
        public FixedSize.Array<ArrayInstance> ar_BeforeLens;
        public FixedSize.Array<ArrayInstance> ar_AfterLens;
        private int MaxLen;

        public Array() : this(100) { }

        public Array(int MaxLen)
        {
            this.MyOptions = MaxLen;
        }

        public override object MyOptions
        {
            get => MaxLen;
            set
            {
                this.MaxLen = (int)value;
                if (this.ar == null)
                {
                    this.ar = new FixedSize.Array<ArrayInstance>(MaxLen);
                    this.ar.Comparer = new ArrayInstance.ICompareByFrom();
                    this.ar_BeforeLens = new FixedSize.Array<ArrayInstance>(MaxLen-1);
                    this.ar_BeforeLens.Comparer =new  ArrayInstance.ICompareByBeforeLen();
                    this.ar_AfterLens = new FixedSize.Array<ArrayInstance>(MaxLen-1);
                    this.ar_AfterLens.Comparer = new ArrayInstance.ICompareByAfterLen();
                }
            }
        }

        public override ArrayType this[int Pos]
        {
            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            get
            {
                ArrayInstance Myar;
                var arInfo = ar.BinarySearch(new ArrayInstance() { FromPos = Pos });
                var Index = arInfo.Index;
                if (Index < 0)
                {
                    Index = (Index * -1) - 2;
                    Myar = ar[Index];
                    return Myar[Pos - Myar.FromPos];
                }

                Myar = arInfo.Value;
                return Myar[Pos - Myar.FromPos];
            }
            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            set
            {
                ArrayInstance Myar;
                var arInfo = ar.BinarySearch(new ArrayInstance() { FromPos = Pos });
                var Index = arInfo.Index;
                if (Index < 0)
                {
                    Index = (Index * -1) - 2;
                    Myar = ar[Index];
                    Myar[Pos - Myar.FromPos] = value;
                    return;
                }

                Myar = arInfo.Value;
                Myar[Pos - Myar.FromPos] = value;
                return;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public override void Insert(ArrayType Value, int Position)
        {
#if DEBUG
            if (ar.Length == 0 && Position > 0)
                throw new Exception("Out of length!");
#endif

            if (ar.Length == 0)
            {
                ar.Insert(new ArrayInstance()
                {
                    MaxLen = MaxLen,
                    Value = Value
                });
            }
            else
            {

                var arInfo = ar.BinarySearch(new ArrayInstance() { FromPos = Position });
                var arPos = arInfo.Index;
                if (arPos < 0)
                {
                    arPos = (arPos * -1) - 2;

                    var MyAr = ar[arPos];
                    Position = Position - MyAr.FromPos;
                    if (Position == MyAr.Length) // between two arrays
                    {
                        if (ar.Length == MaxLen) // we can't add more arrays
                        {
                                MyAr.Insert(Value);
                        }
                        else // we able to add more arrays
                        {
                            arPos++;
                            ar.Insert(new ArrayInstance()
                            {
                                MaxLen = MaxLen,
                                Value = Value,
                                FromPos = MyAr.FromPos + MyAr.Length
                            }, arPos);
                        }
                    }
                    else // in middle of an array
                        MyAr.Insert(Value, Position);
                }
                else
                {
                    var MyAr = ar[arPos];
                    if (ar.Length == MaxLen) // we can't add more arrays
                    {
                        MyAr.Insert(Value, 0);
                        arPos++;
                    }
                    else // we able to add more arrays
                    {
                        ar.Insert(new ArrayInstance()
                        {
                            MaxLen = MaxLen,
                            Value = Value,
                            FromPos = MyAr.FromPos
                        }, arPos);
                    }
                }
                for (int i = arPos + 1; i < ar.Length; i++)
                    ar[i].FromPos += 1;
            }
            Length++;
        }

        private int MargeArray()
        {
            var LowArrayBefore = ar_BeforeLens[0];
            var LowArrayAfter = ar_AfterLens[0];
            int Pos;
            if(LowArrayBefore.BeforeLen<LowArrayAfter.AfterLen)
            {
                Pos = ar.BinarySearch(LowArrayBefore).Index+1;
                LowArrayAfter = ar.Pop(Pos);
            }
            else
            {
                Pos = ar.BinarySearch(LowArrayAfter).Index;
                LowArrayBefore = ar[Pos-1];
                ar.DeleteByPosition(Pos);
            }

            var AfterLen = LowArrayAfter.Length;
            for (int i = 0; i < AfterLen; i++)
                LowArrayBefore.Insert(LowArrayAfter[i]);

            return Pos;
        }

        public override void DeleteByPosition(int Position)
        {
            var arPos = ar.BinarySearch(new ArrayInstance() { FromPos = Position }).Index;
            if (arPos < 0)
            {
                arPos = (arPos * -1) - 2;
            }
            var MyAr = ar[arPos];
            if (MyAr.Length > 1)
                MyAr.Delete(Position - MyAr.FromPos);
            else
                ar.DeleteByPosition(arPos);
            Length--;
        }

        internal override void AddLength(int Count)
        {
            throw new NotImplementedException();
        }
        internal override System.Array GetArrayFrom(int From, out int Ar_From, out int Ar_Len)
        {
            var arPos = ar.BinarySearch(new ArrayInstance() { FromPos = From }).Index;
            if (arPos < 0)
            {
                arPos = (arPos * -1) - 2;
            }
            if (ar.Length > 0)
                From = ar[0].FromPos - From;

            return ar[arPos].ar.GetArrayFrom(From, out Ar_From, out Ar_Len);
        }

        internal override System.Array GetArrayPos(int Ar_Pos, out int Ar_From, out int Ar_Len)
        {
            return ar[Ar_Pos].ar.GetArrayFrom(0, out Ar_From, out Ar_Len);
        }

        protected override IEnumerator<ArrayType> _GetEnumerator()
        {
            return new MyEnum() { ar = this };
        }

        private class MyEnum : IEnumerator<ArrayType>
        {
            public Array<ArrayType> ar;
            private int Pos=-1;
            public ArrayType Current => ar[Pos];

            object IEnumerator.Current => ar[Pos];

            public void Dispose()
            {
                System.GC.SuppressFinalize(this);
            }

            public bool MoveNext()
            {
                Pos++;
                return Pos < ar.Length;
            }

            public void Reset()
            {
                Pos = -1;
            }
        }
    }
}