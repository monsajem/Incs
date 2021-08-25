using Monsajem_Incs.Collection.Array.ArrayBased.DynamicSize;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using static Monsajem_Incs.Collection.Array.Extentions;
using static System.Runtime.Serialization.FormatterServices;
using static System.Text.Encoding;

namespace Monsajem_Incs.Serialization
{
    public interface ICacheSerialize
    {
        byte[] Cache { get; set; }
        bool IsReady { get; }
    }
    public class MemoryCacheSerialize : ICacheSerialize
    {
        [NonSerialized]
        private byte[] _Cache;
        public byte[] Cache {
            [MethodImpl(MethodImplOptions.AggressiveOptimization|MethodImplOptions.AggressiveInlining)]
            get => _Cache;
            [MethodImpl(MethodImplOptions.AggressiveOptimization|MethodImplOptions.AggressiveInlining)]
            set => _Cache = value; }
        public bool IsReady => true;
    }

    public class StreamCacheSerialize : ICacheSerialize, IDisposable
    {
        public static Stream Stream
        {
            set
            {
                Data = new Collection.StreamCollection();
                Data.Stream = value;
                GUID = new Collection.Array.TreeBased.Array<string>();
            }
        }
        private static Collection.StreamCollection Data;
        private static Collection.Array.TreeBased.Array<string> GUID;
        [NonSerialized]
        private string MyGUID = Guid.NewGuid().ToString();


        /// <summary>
        /// First please set "Monsajem_Incs.Serialization.StreamCacheSerialize.Stream"
        /// </summary>
        public byte[] Cache
        {
            [MethodImpl(MethodImplOptions.AggressiveOptimization|MethodImplOptions.AggressiveInlining)]
            get
            {
                if (Data == null)
                    return null;

                lock(Data)
                {
                    var Position = GUID.BinarySearch(MyGUID).Index;
                    if (Position > -1)
                    {
                        return Data[Position];
                    }
                    return null;
                }
            }
            [MethodImpl(MethodImplOptions.AggressiveOptimization|MethodImplOptions.AggressiveInlining)]
            set
            {
                if (Data == null)
                    return;
                lock (Data)
                {
                    var Position = GUID.BinarySearch(MyGUID).Index;
                    if (Position > -1)
                    {
                        if (value == null)
                            GUID.DeleteByPosition(Position);
                        else
                            Data[Position] = value;
                    }
                    else
                    {
                        if (value == null)
                            return;

                        Position = ~Position;
                        GUID.Insert(MyGUID, Position);
                        Data.Insert(value, Position);

                    }
                }
            }
        }

        public bool IsReady
        {
            [MethodImpl(MethodImplOptions.AggressiveOptimization|MethodImplOptions.AggressiveInlining)]
            get
            {
                if (Data != null)
                {
                    if (MyGUID == null)
                        MyGUID = Guid.NewGuid().ToString();
                    return true;
                }
                else
                    return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization|MethodImplOptions.AggressiveInlining)]
        void IDisposable.Dispose()
        {
            var Position = GUID.BinarySearch(MyGUID).Index;
            if (Position > -1)
            {
                GUID.DeleteByPosition(Position);
                Data.DeleteByPosition(Position);
            }
        }
    }

}