﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Monsajem_Incs.Collection.Array.ArrayBased.Hyper
{

    public class ArrayHuge<ArrayType> :Array<ArrayType>
    {
        public ArrayHuge() : base(50000, 100, 500) { }
    }
    public class Array<ArrayType> :
        Base.IArray<ArrayType, Array<ArrayType>>
    {

        private Func<Base.IArray<ArrayType>> MakeInner;
        protected override Array<ArrayType> MakeSameNew()
        {
            return new Array<ArrayType>(MinCount, Sub,MinSub);
        }

        public class ArrayInstance :
            IComparable<ArrayInstance>
        {
            public int FromPos;
            public Base.IArray<ArrayType> ar;
            public int CompareTo(ArrayInstance other)
            {
                return this.FromPos - other.FromPos;
            }
        }
        public DynamicSize.Array<ArrayInstance> ar;
        internal int MinCount;
        private int Sub;
        private int MinSub;
        private int MaxLen;
        private int MaxLen_Div2;

        public Array() : this(500,0,0) { }

        public Array(int MinCount,int Sub,int MinSub)
        {
            if (Sub == 0)
                Sub = MinCount + 1;
            this.MyOptions =(MinCount, Sub,MinSub);
        }
        public Array(ArrayType[] ar, int MinCount = 500, int Sub=0,int MinSub=0) : 
            this(MinCount, Sub,MinSub)
        {
            Insert(ar);
        }

        public override object MyOptions
        {
            get => (MinCount, Sub, MinSub);
            set
            {
                var Option = ((int MinCount, int Sub,int MinSub))value;
                this.Sub = Option.Sub;
                this.MinCount = Option.MinCount;
                this.MinSub = Option.MinSub;
                var SubMinCount = this.MinCount / this.Sub;
                if (MinSub > SubMinCount)
                    SubMinCount = 0;

                if (SubMinCount == 0)
                    this.MakeInner = () => new FixedSize.Array<ArrayType>(this.MinCount);
                else
                    this.MakeInner = () => new Array<ArrayType>(SubMinCount,Sub,MinSub);
                this.MaxLen = MinCount - 1;
                this.MaxLen_Div2 = MaxLen / 2;

                if (this.ar == null)
                {
                    this.ar = new DynamicSize.Array<ArrayInstance>();
                    ar.Insert(new ArrayInstance()
                    {
                        ar = this.MakeInner(),
                        FromPos = 0
                    }, 0);
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
                    return Myar.ar[Pos - Myar.FromPos];
                }

                Myar = arInfo.Value;
                return Myar.ar[Pos - Myar.FromPos];
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
                    Myar.ar[Pos - Myar.FromPos] = value;
                    return;
                }

                Myar = arInfo.Value;
                Myar.ar[Pos - Myar.FromPos] = value;
                return;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private void Optimization(DynamicSize.Array<ArrayInstance> ar)
        {
            for (int i = 0; i < ar.Length; i++)
            {
                i += Optimization(ar, i);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private int Optimization(DynamicSize.Array<ArrayInstance> ar, int Pos)
        {
            var OldAr = ar[Pos];
            if (OldAr.ar.Length > MaxLen)
            {
                var NewAr =(Base.IArray<ArrayType>) OldAr.ar.PopFrom(MaxLen_Div2);
                ar.Insert(new ArrayInstance()
                {
                    FromPos = OldAr.FromPos + OldAr.ar.Length,
                    ar = NewAr
                },
                Pos + 1);
                return 1;
            }
            else if (OldAr.ar.Length == 0)
            {
                if (ar.Length > 1)
                {

                    ar.DeleteByPosition(Pos);
                    return -1;
                }
            }
            else if (OldAr.ar.Length < MaxLen_Div2)
            {
                if (Pos == 0)
                {
                    if (ar.Length > 1)
                    {
                        var NextAr = ar[1];
                        if ((NextAr.ar.Length + OldAr.ar.Length) < (MaxLen_Div2))
                        {
                            NextAr.ar.Insert(0,OldAr.ar.ToArray());
                            NextAr.FromPos = 0;
                            ar.DeleteByPosition(0);
                            return -1;
                        }
                    }
                }
                else if (Pos == ar.Length - 1)
                {
                    var BeforeAr = ar[Pos - 1];
                    if ((BeforeAr.ar.Length + OldAr.ar.Length) < (MaxLen_Div2))
                    {
                        BeforeAr.ar.Insert(OldAr.ar.ToArray());
                        ar.DeleteByPosition(Pos);
                        return -1;
                    }
                }
                else
                {
                    var NextAr = ar[Pos + 1];
                    if ((NextAr.ar.Length + OldAr.ar.Length) < (MaxLen_Div2))
                    {
                        NextAr.ar.Insert(0,OldAr.ar.ToArray());
                        NextAr.FromPos = OldAr.FromPos;
                        ar.DeleteByPosition(Pos);
                        return -1;
                    }
                    else
                    {
                        var BeforeAr = ar[Pos - 1];
                        if ((BeforeAr.ar.Length + OldAr.ar.Length) < (MaxLen_Div2))
                        {
                            BeforeAr.ar.Insert(OldAr.ar.ToArray());
                            ar.DeleteByPosition(Pos);
                            return -1;
                        }
                    }
                }
            }
            return 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public override void Insert(ArrayType Value, int Position)
        {

            var arInfo = ar.BinarySearch(new ArrayInstance() { FromPos = Position });
            var arPos = arInfo.Index;
            ArrayInstance MyAr;
            if (arPos < 0)
            {
                arPos = (arPos * -1) - 2;
                MyAr = ar[arPos];

                if (MyAr.ar.Length==MinCount)
                {
                    Position = Position - MyAr.FromPos;
                    if (Position == MyAr.ar.Length)
                    {
                        var InnerAr = MakeInner();
                        InnerAr.Insert(Value);
                        var NewAr = new ArrayInstance()
                        {
                            FromPos = MyAr.FromPos + MyAr.ar.Length,
                            ar = InnerAr
                        };
                        arPos += 1;
                        ar.Insert(NewAr, arPos);
                    }
                    else
                    {
                        var InnerAr = MyAr.ar.PopFrom(Position);
                        MyAr.ar.Insert(Value);
                        var NewAr = new ArrayInstance()
                        {
                            FromPos = MyAr.FromPos + MyAr.ar.Length,
                            ar = (Base.IArray<ArrayType>)InnerAr
                        };
                        arPos += 1;
                        ar.Insert(NewAr, arPos);
                    }
                }
                else
                    MyAr.ar.Insert(Value, Position - MyAr.FromPos);
            }
            else
            {
                MyAr = arInfo.Value;
                if (MyAr.ar.Length == MinCount)
                {
                    var InnerAr = MakeInner();
                    InnerAr.Insert(Value);
                    var NewAr = new ArrayInstance()
                    {
                        FromPos = MyAr.FromPos,
                        ar = InnerAr
                    };
                    ar.Insert(NewAr, arPos);
                }
                else
                    MyAr.ar.Insert(Value, 0);
            }
            for (int i = arPos + 1; i < ar.Length; i++)
                ar[i].FromPos += 1;
            Length++;
        }

        public override void DeleteByPosition(int Position)
        {
            var arPos = ar.BinarySearch(new ArrayInstance() { FromPos = Position }).Index;
            if (arPos < 0)
            {
                arPos = (arPos * -1) - 2;
            }
            var MyAr = ar[arPos];
            MyAr.ar.DeleteByPosition(Position - MyAr.FromPos);
            for (int i = arPos + 1; i < ar.Length; i++)
                ar[i].FromPos -= 1;
            Optimization(ar, arPos);
            Length--;
        }

        internal override void AddLength(int Count)
        {
            throw new NotImplementedException();
        }

        public override void DeleteFrom(int from)
        {
            var arPos = ar.BinarySearch(new ArrayInstance() { FromPos = from }).Index;
            if (arPos < 0)
            {
                arPos = (arPos * -1) - 2;
                var MyAr = ar[arPos];
                MyAr.ar.DeleteFrom(from - MyAr.FromPos);
            }
            else
            {
                if (arPos == 0)
                    ar[0].ar.DeleteFrom(0);
                else
                    ar.DeleteFrom(arPos);
            }
            Length = from;
        }

        internal override void AddFromTo(int From, System.Array Ar, int Ar_From, int Ar_Len)
        {
            var arPos = this.ar.BinarySearch(new ArrayInstance() { FromPos = From }).Index;
            if (arPos < 0)
            {
                arPos = (arPos * -1) - 2;
                var FromAr = ar[arPos];
                FromAr = new ArrayInstance()
                {
                    ar = (Base.IArray<ArrayType>)FromAr.ar.PopFrom(From - FromAr.FromPos)
                };
                arPos += 1;
                ar.Insert(new ArrayInstance()
                {
                    FromPos = From,
                    ar = FromAr.ar
                }, arPos);
            }

            var Instance = new ArrayInstance()
            {
                ar = new FixedSize.Array<ArrayType>((ArrayType[])Ar),
                FromPos = From
            };

            var Opt = new DynamicSize.Array<ArrayInstance>(10);
            Opt.Insert(Instance);
            Optimization(Opt);
            ar.Insert(arPos,Opt.ToArray());
            for (int i = arPos + Opt.Length; i < ar.Length; i++)
            {
                ar[i].FromPos += Ar_Len;
            }
            var Pos = Optimization(ar, arPos);
            Pos = (arPos + Opt.Length) + Pos;
            Optimization(ar, Pos);
            this.Length += Ar_Len;
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
    }
}