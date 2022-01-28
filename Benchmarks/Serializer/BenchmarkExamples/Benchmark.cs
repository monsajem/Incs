using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using BenchmarkDotNet.Attributes;
using Monsajem_Incs.Serialization;
using MessagePack;
using StructPacker;
using BinaryPack;
using BinaryPack.Attributes;
using BinaryPack.Enums;

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

    [Pack]
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
        public Cls[] Cls_Ar_Same;
        public Cls[] Cls_Ar_NSame;
        public Str_Unmanaged StrUN_Value;
        public Str_Managed StrMN_Value;

        [GlobalSetup]
        public void GlobalS()
        {
            {
                Int_Value = 12;
                Int_Value = SrDr(Int_Value);
                Int_Value = SrDrMP(Int_Value);
                Int_Value = SrDrBP(Int_Value);
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
                Cls_Value = SrDrBP(Cls_Value);
            }
            {
                Cls_Ar_Same = new Cls[1_000];
                Cls_Ar_NSame = new Cls[1_000];
                for (int i = 0; i < Cls_Ar_Same.Length; i++)
                {
                    Cls_Ar_Same[i] = Cls_Value;
                    Cls_Ar_NSame[i] = new Cls() { IntValue = Int_Value, StringValue = String_Value };

                }
                Cls_Ar_Same = SrDr(Cls_Ar_Same);
                Cls_Ar_Same = SrDrMP(Cls_Ar_Same);
                Cls_Ar_NSame = SrDr(Cls_Ar_NSame);
                Cls_Ar_NSame = SrDrMP(Cls_Ar_NSame);
            }
            {
                StrUN_Value.Int1Value = Int_Value;
                StrUN_Value.Int2Value = Int_Value;
                StrUN_Value = SrDr(StrUN_Value);
                StrUN_Value = SrDrMP(StrUN_Value);
                StrUN_Value = SrDrBP(StrUN_Value);
            }
            {
                StrMN_Value.ClsValue = Cls_Value;
                StrMN_Value.IntValue = Int_Value;
                StrMN_Value = SrDr(StrMN_Value);
                StrMN_Value = SrDrMP(StrMN_Value);
                StrMN_Value = SrDrBP(StrMN_Value);
            }
        }

        public static T SrDr<T>(T Value) => Value.Serialize().Deserialize<T>();
        public static T SrDrMP<T>(T Value) => MessagePackSerializer.Deserialize<T>(
                                                MessagePackSerializer.Serialize(Value));
        public static T SrDrBP<T>(T Value)
            where T:new()
            => BinaryConverter.Deserialize<T>(BinaryConverter.Serialize(Value));

        [Benchmark]
        public object Integer()=>SrDr(Int_Value);

        [Benchmark]
        public object ArInteger() => SrDr(Int_Ar);

        [Benchmark]
        public object ArSameCls() => SrDr(Cls_Ar_Same);

        [Benchmark]
        public object ArNSameCls() => SrDr(Cls_Ar_NSame);

        [Benchmark]
        public object String() => SrDr(String_Value);

        [Benchmark]
        public object Class() => SrDr(Cls_Value);

        [Benchmark]
        public object Struct_UnManaged() => SrDr(StrUN_Value);

        [Benchmark]
        public object Struct_Managed() => SrDr(StrMN_Value);




        [Benchmark]
        public object Integer_MP() => SrDrMP(Int_Value.Serialize());

        [Benchmark]
        public object ArInteger_MP() => SrDrMP(Int_Ar);

        [Benchmark]
        public object ArSameCls_MP() => SrDrMP(Cls_Ar_Same);

        [Benchmark]
        public object ArNSameCls_MP() => SrDrMP(Cls_Ar_NSame);

        [Benchmark]
        public object String_MP() => SrDrMP(String_Value);

        [Benchmark]
        public object Class_MP() => SrDrMP(Cls_Value);

        [Benchmark]
        public object Struct_UnManaged_MP() => SrDrMP(StrUN_Value);

        [Benchmark]
        public object Struct_Managed_MP() => SrDrMP(StrMN_Value);


        [Benchmark]
        public object Integer_BP() => 
            BinaryConverter.Deserialize<int>(BinaryConverter.Serialize(Int_Value));

        [Benchmark]
        public object Class_BP() => SrDrBP(Cls_Value);

        [Benchmark]
        public object Struct_UnManaged_BP() => SrDrBP(StrUN_Value);

        [Benchmark]
        public object Struct_Managed_BP() => SrDrBP(StrMN_Value);

        [Benchmark]
        public object Struct_UnManaged_SP()
        {
            var y = StrUN_Value.Pack();
            var x = new Str_Unmanaged();
            x.Unpack(y);
            return x;
        }
    }
}
