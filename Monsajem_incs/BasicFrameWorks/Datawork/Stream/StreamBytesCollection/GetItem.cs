using System;

namespace Monsajem_Incs.Collection
{
    public partial class StreamCollection
    {
        public byte[] ReadBytes(int From, int Len)
        {
            var DataAsByte = new byte[Len];
            _ = Stream.Seek(From, System.IO.SeekOrigin.Begin);
            var Pos = 0;
            while (Len > 0)
            {
                var CLen = Stream.Read(DataAsByte, Pos, Len);
                if (CLen == 0)
                    throw new Exception("Stream Error");
                Len -= CLen;
                Pos += CLen;
            }
            return DataAsByte;
        }

        public byte[] GetItem(int Position)
        {
            //Info.Browse();
            var HeadSize = this.HeadSize;
            var DataLoc = GetInfo(Position);
            return ReadBytes(DataLoc.From + HeadSize, DataLoc.Len - HeadSize);
        }

        public void SetItem(int Pos, byte[] Data)
        {
#if DEBUG
            Debug(this);
#endif
            DeleteByPosition(Pos);
            Insert(Data, Pos);
#if DEBUG
            Debug(this);
#endif
        }

        public override byte[] this[int Pos]
        {
            get => GetItem(Pos);
            set => SetItem(Pos, value);
        }
    }
}
