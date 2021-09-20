using Monsajem_Incs.Collection.Array.ArrayBased.DynamicSize;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using static Monsajem_Incs.Collection.Array.Extentions;
using static System.Runtime.Serialization.FormatterServices;
using static System.Text.Encoding;
using System.Runtime.CompilerServices;

namespace Monsajem_Incs.Serialization
{
    public partial class Serialization
    {
        public static Serialization Serializere;
        public Serialization()
        {
            Serializere = this;
            SerializeInfo<object>.InsertSerializer(
            (Data, obj) => { },
            (Data) => new object(), false);

            SerializeInfo<DateTime>.InsertSerializer(
            (Data, obj) =>
            {
                Data.Data.Write(BitConverter.GetBytes(((DateTime)obj).Ticks), 0, 8);
            },
            (Data) =>
            {
                int Position = Data.From; Data.From += 8;
                return DateTime.FromBinary(BitConverter.ToInt64(Data.Data, Position));
            }, true);

            SerializeInfo<string>.InsertSerializer(
            (Data, obj) =>
            {
                if (obj == null)
                {
                    Data.Data.Write(BitConverter.GetBytes(-1), 0, 4);
                    return;
                }
                var Str = UTF8.GetBytes((string)obj);
                var Len = BitConverter.GetBytes(Str.Length);
                Data.Data.Write(Len, 0, 4);
                Data.Data.Write(Str, 0, Str.Length);
            },
            (Data) =>
            {
                var StrSize = BitConverter.ToInt32(Data.Data, Data.From);
                Data.From += 4;
                if (StrSize == -1)
                    return null;
                var Position = Data.From;
                Data.From += StrSize;
                return UTF8.GetString(Data.Data, Position, StrSize);
            }, true);

            SerializeInfo<Type>.InsertSerializer(
            (Data, obj) =>
            {
                var Name = Write((obj as Type).MidName());
                Data.Data.Write(Name, 0, Name.Length);
            },
            (Data) =>
            {
                return Read(Data).GetTypeByName();
            }, true);

            {
                var SR = SerializeInfo<object>.GetSerialize();
                SerializeInfo<System.Runtime.InteropServices.GCHandle>.InsertSerializer(
                    (Data, obj) =>
                    {
                        var GC = (System.Runtime.InteropServices.GCHandle)obj;
                        SR.Serializer(Data, GC.Target);
                    },
                    (Data) =>
                    {
                        return System.Runtime.InteropServices.GCHandle.Alloc(SR.Deserializer(Data));
                    }, true);
            }

            SerializeInfo<IEqualityComparer<string>>.InsertSerializer(
                (Data, obj) =>
                {
                    if (obj == null)
                        Data.Data.WriteByte(0);
                    else
                        Data.Data.WriteByte(1);
                },
                (Data) =>
                {
                    if (Data.Data[Data.From++] == 0)
                        return null;
                    else
                        return EqualityComparer<string>.Default;
                }, true);
        }
    }
}