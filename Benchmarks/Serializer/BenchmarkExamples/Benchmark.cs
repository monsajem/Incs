using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using BenchmarkDotNet.Attributes;
using Monsajem_Incs.Serialization;
using MessagePack;

namespace BenchmarkExample
{
    [MessagePackObject]
    public class Cls
    {
        [Key(0)]
        public int IntValue;
        [Key(1)]
        public string StringValue;
    }

    [MessagePackObject]
    public struct Str_Unmanaged
    {
        [Key(0)]
        public int Int1Value;
        [Key(1)]
        public int Int2Value;
    }

    [MessagePackObject]
    public struct Str_Managed
    {
        [Key(0)]
        public int IntValue;
        [Key(1)]
        public Cls ClsValue;
    }

    public class Benchmark
    {
        public int Int_Value;
        public int[] Int_Ar;
        public string String_Value;
        public Cls Cls_Value;
        public Str_Unmanaged StrUN_Value;
        public Str_Managed StrMN_Value;

        [GlobalSetup]
        public void GlobalS()
        {
            {
                Int_Value = 12;
                Int_Value = SrDr(Int_Value);
                Int_Value = SrDrMP(Int_Value);
            }
            {
                Int_Ar = new int[1_000_000];
                Int_Ar = SrDr(Int_Ar);
                Int_Ar = SrDrMP(Int_Ar);
            }
            {
                String_Value = "aaa";
                String_Value = SrDr(String_Value);
                String_Value = SrDrMP(String_Value);
            }
            {
                Cls_Value = new Cls();
                Cls_Value.IntValue = Int_Value;
                Cls_Value.StringValue = String_Value;
                Cls_Value = SrDr(Cls_Value);
                Cls_Value = SrDrMP(Cls_Value);
            }
            {
                StrUN_Value.Int1Value = Int_Value;
                StrUN_Value.Int2Value = Int_Value;
                StrUN_Value = SrDr(StrUN_Value);
                StrUN_Value = SrDrMP(StrUN_Value);
            }
            {
                StrMN_Value.ClsValue = Cls_Value;
                StrMN_Value.IntValue = Int_Value;
                StrMN_Value = SrDr(StrMN_Value);
                StrMN_Value =SrDrMP(StrMN_Value);
            }
        }

        public static T SrDr<T>(T Value) => Value.Serialize().Deserialize<T>();
        public static T SrDrMP<T>(T Value) => MessagePackSerializer.Deserialize<T>(
                                                MessagePackSerializer.Serialize(Value));

        [Benchmark]
        public object Integer()=>SrDr(Int_Value);

        [Benchmark]
        public object ArInteger() => SrDr(Int_Ar);

        [Benchmark]
        public object String() => SrDr(String_Value);

        [Benchmark]
        public object Class() => SrDr(Cls_Value);

        [Benchmark]
        public object Struct_UnManaged() => SrDr(StrUN_Value);

        [Benchmark]
        public object Struct_Managed() => SrDr(StrMN_Value);




        [Benchmark]
        public object Integer_MP() => SrDrMP(Int_Value);

        [Benchmark]
        public object ArInteger_MP() => SrDr(Int_Ar);

        [Benchmark]
        public object String_MP() => SrDrMP(String_Value);

        [Benchmark]
        public object Class_MP() => SrDrMP(Cls_Value);

        [Benchmark]
        public object Struct_UnManaged_MP() => SrDrMP(StrUN_Value);

        [Benchmark]
        public object Struct_Managed_MP() => SrDrMP(StrMN_Value);
    }
}
