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
                Action<object> Serializer,
                Func<object> Deserializer, bool IsFixedType = false)
            {
                _Serializer = Serializer;
                _Deserializer = Deserializer;
                return InsertSerializer(() => (Serializer, Deserializer), IsFixedType);
            }
            private static SerializeInfo<t> InsertSerializer(
                Func<(Action<object> Sr, Func<object> Dr)> Serializer,
                bool IsFixedType = false)
            {
                var MainType = typeof(t);
                var Serialize = GetSerialize();
                var IsSrNull = Serialize.Serializer == null;
                if (MainType.IsAbstract && IsSrNull)
                {
                    Serialize.Serializer = (obj) =>
                    {
                        var ObjType = obj.GetType();
                        Serializere.WriteSerializer(ObjType).Serializer(obj);
                    };
                    Serialize.Deserializer = () =>
                    {
                        var Inherit_SR = Serializere.ReadSerializer();
                        // security reason for assign
                        if (Inherit_SR.Type.IsAssignableTo(MainType) == false)
                            throw new AccessViolationException($"Type of {Inherit_SR.Type} cant assign to {MainType}");
                        return Inherit_SR.Deserializer();
                    };
                }
                else
                {
                    var Sr = Serializer();
                    if (IsFixedType == false)
                    {
                        var SR = Sr.Sr;
                        var DR = Sr.Dr;
                        Sr.Sr = (obj) =>
                        {
                            if (obj != null)
                            {
                                var ObjType = obj.GetType();
                                if (ObjType != MainType)
                                {
                                    S_Data.WriteByte(1);
                                    Serializere.WriteSerializer(ObjType).Serializer(obj);
                                    return;
                                }
                            }
                            Trust?.Invoke(MainType);
                            S_Data.WriteByte(0);
                            SR(obj);
                        };
                        Sr.Dr = () =>
                        {
                            var Status = D_Data[From];
                            From += 1;
                            if (Status == 0)
                            {
                                Trust?.Invoke(MainType);
                                return DR();
                            }
                            else
                            {
                                var Inherit_SR = Serializere.ReadSerializer();
                                // security reason for assign
                                if (Inherit_SR.Type.IsAssignableTo(MainType) == false)
                                    throw new AccessViolationException($"Type of {Inherit_SR.Type} cant assign to {MainType}");
                                return Inherit_SR.Deserializer();
                            }
                        };
                    }

                    if (typeof(t).IsAssignableTo(typeof(ICacheSerialize)))
                    {
                        var SR = Sr.Sr;
                        var DR = Sr.Dr;
                        Sr.Sr = (obj) =>
                        {
                            var ICache = (ICacheSerialize)obj;
                            if (ICache.IsReady)
                            {
                                var Cache = ICache.Cache;
                                if (Cache == null)
                                {
                                    var FromPosition = S_Data.Position;
                                    SR(obj);
                                    var len = (int)(S_Data.Length - FromPosition);
                                    Cache = new byte[len];
                                    S_Data.Seek(FromPosition, SeekOrigin.Begin);
                                    var AllLen = len;
                                    while (len > 0)
                                        len -= S_Data.Read(Cache, AllLen - len, len);
                                    ICache.Cache = Cache;
                                    S_Data.Seek(S_Data.Length, SeekOrigin.Begin);
                                }
                                else
                                {
#if DEBUG
                                    var DebugCache = (byte[])ICache.Cache.Clone();
                                    var FromPosition = S_Data.Position;
                                    SR(obj);
                                    var len = (int)(S_Data.Length - FromPosition);
                                    Cache = new byte[len];
                                    S_Data.Seek(FromPosition, SeekOrigin.Begin);
                                    var AllLen = len;
                                    while (len > 0)
                                        len -= S_Data.Read(Cache, AllLen - len, len);
                                    S_Data.Seek(S_Data.Length, SeekOrigin.Begin);

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
                                SR(obj);
                        };
                        Sr.Dr = () =>
                        {

                            var FromPosition = From;
                            var Result = DR();
                            var ICache = (ICacheSerialize)Result;
                            if (ICache.IsReady)
                            {
                                var len = From - FromPosition;
                                var Cache = new byte[len];
                                Array.Copy(D_Data, FromPosition, Cache, 0, len);
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

                    Sr.Sr = (obj) =>
                    {
                        var Pos = S_Data.Position;
                        Tracer($"Type: {Serialize.Type} Pos:({Pos})");
                        Check_SR();
                        SR(obj);
                        UnTracer($"Type: {Serialize.Type} Pos:({Pos})");
                    };
                    Sr.Dr = () =>
                    {
                        var Pos = From;
                        Tracer($"Type: {Serialize.Type} Pos:({Pos})");
                        Check_DR();
                        var Result = DR();
                        UnTracer($"Type: {Serialize.Type} Pos:({Pos})");
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

                    SR = (obj) =>
                    {
                        Trust?.Invoke(MainType);
                        SR(obj);
                    };
                    Sr.Dr = () =>
                    {
                        Trust?.Invoke(MainType);
                        return DR();
                    };
                    Serialize.Serializer = Sr.Sr;
                    Serialize.Deserializer = Sr.Dr;
                }
                return Serialize;
            }
        }
    }
}