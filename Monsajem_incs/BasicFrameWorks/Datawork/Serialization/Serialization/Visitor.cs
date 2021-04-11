using System;
using static Monsajem_Incs.Collection.Array.Extentions;
using static System.Text.Encoding;

namespace Monsajem_Incs.Serialization
{
    public partial class Serialization
    {
        [ThreadStatic]
        private static Action AtLast;
        private void VisitedSerialize(
            object obj,
            SerializeInfo serializer)
        {
            if (obj == null)
            {
                S_Data.Write(Byte_Int_N_2, 0, 4);
                return;
            }
            if (serializer.CanStoreInVisit == false)
            {
                S_Data.Write(Byte_Int_N_1, 0, 4);
                serializer.Serializer(obj);
                return;
            }
            var Key = new ObjectContainer()
            {
                HashCode = obj.GetHashCode(),
                obj = obj
            };
            if(Visitor.TryGetValue(Key, out var VisitedObj)== false)
            {
                Visitor.Add(Key);
#if DEBUG
                Key.obj = obj;
#endif
                Key.FromPos = (int)S_Data.Position;
                S_Data.Write(Byte_Int_N_1, 0, 4);
                serializer.Serializer(obj);
            }
            else
            {
#if DEBUG
                if (VisitedObj.obj.GetType() != obj.GetType())
                    throw new Exception("Type of visited object is wrong" +
                                        "\n\nMain: " + obj.GetType().ToString() +
                                        "\n\nVisited: " + VisitedObj.obj.GetType().ToString());
#endif
                S_Data.Write(BitConverter.GetBytes(VisitedObj.FromPos), 0, 4);
            }
        }
        private void VisitedDeserialize(
            Action<object> Set,
            SerializeInfo deserializer)
        {
            var LastFrom = From;
            var Fr = BitConverter.ToInt32(D_Data, From);
            From += 4;
            if (deserializer.CanStoreInVisit == false)
            {
                if (Fr == -2)
                    return;
                Set(deserializer.Deserializer());
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
                    Visitor.Add(VisitedObj);
                    VisitedObj.obj = deserializer.Deserializer();
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
            Visitor.TryGetValue(VisitedObj, out VisitedObj);
            if (VisitedObj.obj == null)
                AtLast += () => Set(VisitedObj.obj);
            else
                Set(VisitedObj.obj);
        }

        private t VisitedInfoSerialize<t>(
            object obj,
            Func<(byte[] Bytes, t Obj)> GetData)
        {
            var Key = new ObjectContainer()
            {
                HashCode = obj.GetHashCode(),
                obj = obj
            };
            if (Visitor_info.TryGetValue(Key, out var VisitedObj)== false)
            {
                Visitor_info.Add(Key);
                Key.FromPos = (int)S_Data.Position;
                var Data = GetData();
                var DataBytes = Data.Bytes;
                Key.Data =(DataBytes, Data.Obj);
                S_Data.Write(Byte_Int_N_1, 0, 4);
                S_Data.Write(DataBytes, 0, DataBytes.Length);
                return Data.Obj;
            }
            else
            {
                S_Data.Write(BitConverter.GetBytes(VisitedObj.FromPos), 0, 4);
                return (t)VisitedObj.Data.Obj;
            }
        }
        private t VisitedInfoDeserialize<t>(
            Func<t> Get)
        {
            var LastFrom = From;
            var Fr = BitConverter.ToInt32(D_Data, From);
            From += 4;
            ObjectContainer VisitedObj;
            if (Fr == -1)
            {
                VisitedObj = new ObjectContainer()
                {
                    HashCode = LastFrom,
                    IsUniqueHashCode = true,
                    obj = Get()
                };
                Visitor_info.Add(VisitedObj);
                return (t)VisitedObj.obj;
            }
            VisitedObj = new ObjectContainer()
            {
                HashCode = Fr,
                IsUniqueHashCode = true
            };
            Visitor_info.TryGetValue(VisitedObj, out VisitedObj);
            return (t)VisitedObj.obj;
        }
    }

}
