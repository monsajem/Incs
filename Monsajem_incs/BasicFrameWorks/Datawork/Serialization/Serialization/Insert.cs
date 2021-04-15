using Monsajem_Incs.Collection.Array.ArrayBased.DynamicSize;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using static Monsajem_Incs.Collection.Array.Extentions;
using static System.Runtime.Serialization.FormatterServices;
using static System.Text.Encoding;

namespace Monsajem_Incs.Serialization
{
    public partial class Serialization
    {
        private partial class SerializeInfo<t> : SerializeInfo
        {

            public static SerializeInfo<t> InsertSerializer(
                Action<SerializeData,object> Serializer,
                Func<DeserializeData,object> Deserializer, bool IsFixedType = false)
            {
                _Serializer = Serializer;
                _Deserializer = Deserializer;
                return InsertSerializer(() => (Serializer, Deserializer), IsFixedType);
            }
            private static SerializeInfo<t> InsertSerializer(
                Func<(Action<SerializeData,object> Sr, Func<DeserializeData,object> Dr)> Serializer,
                bool IsFixedType = false)
            {
                var MainType = typeof(t);
                var Serialize = GetSerialize();
                var IsSrNull = Serialize.Serializer == null;
                if (MainType.IsAbstract && IsSrNull)
                {
                    Serialize.Serializer = (Data,obj) =>
                    {
                        var ObjType = obj.GetType();
                        Serializere.WriteSerializer(Data,ObjType).Serializer(Data,obj);
                    };
                    Serialize.Deserializer = (Data) =>
                    {
                        var Inherit_SR = Serializere.ReadSerializer(Data);
                        // security reason for assign
                        if (Inherit_SR.Type.IsAssignableTo(MainType) == false)
                            throw new AccessViolationException($"Type of {Inherit_SR.Type} cant assign to {MainType}");
                        return Inherit_SR.Deserializer(Data);
                    };
                }
                else
                {
                    var Sr = Serializer();
                    if (IsFixedType == false)
                    {
                        var SR = Sr.Sr;
                        var DR = Sr.Dr;
                        Sr.Sr = (Data,obj) =>
                        {
                            if (obj != null)
                            {
                                var ObjType = obj.GetType();
                                if (ObjType != MainType)
                                {
                                    Data.Data.WriteByte(1);
                                    Serializere.WriteSerializer(Data,ObjType).Serializer(Data,obj);
                                    return;
                                }
                            }
                            Data.TrustToType?.Invoke(MainType);
                            Data.Data.WriteByte(0);
                            SR(Data,obj);
                        };
                        Sr.Dr = (Data) =>
                        {
                            var Status = Data.Data[Data.From];
                            Data. From += 1;
                            if (Status == 0)
                            {
                                Data.TrustToType?.Invoke(MainType);
                                return DR(Data);
                            }
                            else
                            {
                                var Inherit_SR = Serializere.ReadSerializer(Data);
                                // security reason for assign
                                if (Inherit_SR.Type.IsAssignableTo(MainType) == false)
                                    throw new AccessViolationException($"Type of {Inherit_SR.Type} cant assign to {MainType}");
                                return Inherit_SR.Deserializer(Data);
                            }
                        };
                    }

                    if (typeof(t).IsAssignableTo(typeof(ICacheSerialize)))
                    {
                        var SR = Sr.Sr;
                        var DR = Sr.Dr;
                        Sr.Sr = (Data,obj) =>
                        {
                            var ICache = (ICacheSerialize)obj;
                            if (ICache.IsReady)
                            {
                                var Cache = ICache.Cache;
                                if (Cache == null)
                                {
                                    var FromPosition = Data.Data.Position;
                                    SR(Data,obj);
                                    var len = (int)(Data.Data.Length - FromPosition);
                                    Cache = new byte[len];
                                    Data.Data.Seek(FromPosition, SeekOrigin.Begin);
                                    var AllLen = len;
                                    while (len > 0)
                                        len -= Data.Data.Read(Cache, AllLen - len, len);
                                    ICache.Cache = Cache;
                                    Data.Data.Seek(Data.Data.Length, SeekOrigin.Begin);
                                }
                                else
                                {
#if DEBUG
                                    var DebugCache = (byte[])ICache.Cache.Clone();
                                    var FromPosition = Data.Data.Position;
                                    SR(Data,obj);
                                    var len = (int)(Data.Data.Length - FromPosition);
                                    Cache = new byte[len];
                                    Data.Data.Seek(FromPosition, SeekOrigin.Begin);
                                    var AllLen = len;
                                    while (len > 0)
                                        len -= Data.Data.Read(Cache, AllLen - len, len);
                                    Data.Data.Seek(Data.Data.Length, SeekOrigin.Begin);

                                    if (DebugCache.Length != Cache.Length)
                                        throw new Exception("Cache having wrong data!");
                                    for (int i = 0; i < DebugCache.Length; i++)
                                        if (DebugCache[i] != Cache[i])
                                            throw new Exception("Cache having wrong data!");
#else
                                    S_Data.Write(Cache, 0, Cache.Length);
#endif
                                }
                            }
                            else
                                SR(Data,obj);
                        };
                        Sr.Dr = (Data) =>
                        {

                            var FromPosition = Data.From;
                            var Result = DR(Data);
                            var ICache = (ICacheSerialize)Result;
                            if (ICache.IsReady)
                            {
                                var len = Data.From - FromPosition;
                                var Cache = new byte[len];
                                Array.Copy(Data.Data, FromPosition, Cache, 0, len);
                                ICache.Cache = Cache;
                            }
                            return Result;
                        };
                    }
                    Serialize.Serializer = Sr.Sr;
                    Serialize.Deserializer = Sr.Dr;
                }

#if DEBUG
                {
                    var Sr = (Sr:Serialize.Serializer,Dr: Serialize.Deserializer);
                    var SR = Sr.Sr;
                    var DR = Sr.Dr;

                    Sr.Sr = (Data,obj) =>
                    {
                        var Pos = Data.Data.Position;
                        Tracer(Data,$"Type: {Serialize.Type} Pos:({Pos})");
                        Check_SR(Data);
                        SR(Data,obj);
                        UnTracer(Data, $"Type: {Serialize.Type} Pos:({Pos})");
                    };
                    Sr.Dr = (Data) =>
                    {
                        var Pos = Data.From;
                        Tracer(Data, $"Type: {Serialize.Type} Pos:({Pos})");
                        Check_DR(Data);
                        var Result = DR(Data);
                        UnTracer(Data, $"Type: {Serialize.Type} Pos:({Pos})");
                        return Result;
                    };
                    Serialize.Serializer = Sr.Sr;
                    Serialize.Deserializer = Sr.Dr;
                }
#endif
                if(IsFixedType)
                {
                    var Sr = (Sr: Serialize.Serializer, Dr: Serialize.Deserializer);
                    var SR = Sr.Sr;
                    var DR = Sr.Dr;

                    SR = (Data,obj) =>
                    {
                        Data.TrustToType?.Invoke(MainType);
                        SR(Data,obj);
                    };
                    Sr.Dr = (Data) =>
                    {
                        Data.TrustToType?.Invoke(MainType);
                        return DR(Data);
                    };
                    Serialize.Serializer = Sr.Sr;
                    Serialize.Deserializer = Sr.Dr;
                }
                return Serialize;
            }
        }
    }
}