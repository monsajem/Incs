using System;
using System.Runtime.CompilerServices;
using static Monsajem_Incs.Collection.Array.Extentions;
using static System.Text.Encoding;

namespace Monsajem_Incs.Serialization
{
    public partial class Serialization
    {
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private void VisitedSerialize(
            SerializeData Data,
            object obj,
            SerializeInfo serializer)
        {
            if (serializer.ConstantSize>-1)
            {
                serializer.Serializer(Data, obj);
                return;
            }
            var BytesData = Data.Data;
            if (obj == null)
            {
                BytesData.Write(Byte_Int_N_2, 0, 4);
                return;
            }
            if (serializer.CanStoreInVisit == false)
            {
                BytesData.Write(Byte_Int_N_1, 0, 4);
                serializer.Serializer(Data,obj);
                return;
            }
            var Key = new ObjectContainer()
            {
                HashCode = obj.GetHashCode(),
                obj = obj
            };
            var Visitor = Data.Visitor;
            if (Visitor.TryGetValue(Key, out var VisitedObj)== false)
            {
                Visitor.Add(Key);
#if DEBUG
                Key.obj = obj;
#endif
                Key.FromPos = (int)BytesData.Position;
                BytesData.Write(Byte_Int_N_1, 0, 4);
                serializer.Serializer(Data,obj);
            }
            else
            {
#if DEBUG
                if (VisitedObj.obj.GetType() != obj.GetType())
                    throw new Exception("Type of visited object is wrong" +
                                        "\n\nMain: " + obj.GetType().ToString() +
                                        "\n\nVisited: " + VisitedObj.obj.GetType().ToString());
#endif
                BytesData.Write(BitConverter.GetBytes(VisitedObj.FromPos), 0, 4);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private void VisitedDeserialize(
            DeserializeData Data,
            Action<object> Set,
            SerializeInfo deserializer)
        {
            if (deserializer.ConstantSize>-1)
            {
                Set(deserializer.Deserializer(Data));
                return;
            }
            var LastFrom = Data.From;
            var Fr = BitConverter.ToInt32(Data.Data, LastFrom);
            Data.From += 4;
            if (deserializer.CanStoreInVisit == false)
            {
                if (Fr == -2)
                    return;
                Set(deserializer.Deserializer(Data));
                return;
            }
            ObjectContainer VisitedObj;
            switch (Fr)
            {
                case -1:
                    VisitedObj = new ObjectContainer()
                    {
                        HashCode = LastFrom,
                        IsUniqueHashCode = true
                    };
                    Data.Visitor.Add(VisitedObj);
                    VisitedObj.obj = deserializer.Deserializer(Data);
                    Set(VisitedObj.obj);
                    return;
                case -2:
                    return;
            }
            VisitedObj = new ObjectContainer()
            {
                HashCode = Fr,
                IsUniqueHashCode = true
            };
            Data.Visitor.TryGetValue(VisitedObj, out VisitedObj);
            if (VisitedObj.obj == null)
                Data.AtLast += () => Set(VisitedObj.obj);
            else
                Set(VisitedObj.obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private t VisitedInfoSerialize<t>(
            SerializeData S_Data,
            object obj,
            Func<(byte[] Bytes, t Obj)> GetData)
        {
            var Key = new ObjectContainer()
            {
                HashCode = obj.GetHashCode(),
                obj = obj
            };
            if (S_Data.Visitor_info.TryGetValue(Key, out var VisitedObj)== false)
            {
                S_Data.Visitor_info.Add(Key);
                Key.FromPos = (int)S_Data.Data.Position;
                var Data = GetData();
                var DataBytes = Data.Bytes;
                Key.Data =(DataBytes, Data.Obj);
                S_Data.Data.Write(Byte_Int_N_1, 0, 4);
                S_Data.Data.Write(DataBytes, 0, DataBytes.Length);
                return Data.Obj;
            }
            else
            {
                S_Data.Data.Write(BitConverter.GetBytes(VisitedObj.FromPos), 0, 4);
                return (t)VisitedObj.Data.Obj;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private t VisitedInfoDeserialize<t>(
            DeserializeData Data,
            Func<t> Get)
        {
            var LastFrom = Data.From;
            var Fr = BitConverter.ToInt32(Data.Data, Data.From);
            Data.From += 4;
            ObjectContainer VisitedObj;
            if (Fr == -1)
            {
                VisitedObj = new ObjectContainer()
                {
                    HashCode = LastFrom,
                    IsUniqueHashCode = true,
                    obj = Get()
                };
                Data.Visitor_info.Add(VisitedObj);
                return (t)VisitedObj.obj;
            }
            VisitedObj = new ObjectContainer()
            {
                HashCode = Fr,
                IsUniqueHashCode = true
            };
            Data.Visitor_info.TryGetValue(VisitedObj, out VisitedObj);
            return (t)VisitedObj.obj;
        }
    }

}
