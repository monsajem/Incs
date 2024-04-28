using System;
using System.IO;

namespace Monsajem_Incs.DataWork
{

    public partial class StreamController : System.IO.Stream
    {
        private int HeaderSize;
        private Stream Stream;

        private long MinLen;
        private long MaxLen;
        private long minCount;
        private long StreamLen;
        public StreamController(Stream Stream, int HeaderSize = 0, int MinLen = 0)
        {
            this.Stream = Stream;
            this.HeaderSize = HeaderSize;
            minCount = MinLen;
            if (Stream.Length < HeaderSize)
                Stream.SetLength(HeaderSize);
            SetLength(Stream.Length - HeaderSize);
            Position = 0;
        }

        public byte[] GetHeader()
        {
            var OldPos = Stream.Position;
            Stream.Position = 0;
            var Res = Read(HeaderSize);
            Stream.Position = OldPos;
            return Res;
        }
        public void SetHeader(byte[] Bytes)
        {
            var OldPos = Stream.Position;
            Stream.Position = 0;
            Stream.Write(Bytes, 0, HeaderSize);
            Stream.Position = OldPos;
        }

        public override bool CanRead => Stream.CanRead;
        public override bool CanSeek => Stream.CanSeek;
        public override bool CanWrite => Stream.CanWrite;
        public override long Length => StreamLen - HeaderSize;

        public override long Position { get => Stream.Position - HeaderSize; set => Stream.Position = value + HeaderSize; }

        public override void Flush() => Stream.Flush();

        public override int Read(byte[] buffer, int offset, int count) => Stream.Read(buffer, offset, count);
        public byte[] Read(int count)
        {
            var DataAsByte = new byte[count];
            var Pos = 0;
            while (count > 0)
            {
                var CLen = Stream.Read(DataAsByte, Pos, count);
                if (CLen == 0)
                    throw new Exception("Stream Error");
                count -= CLen;
                Pos += CLen;
            }
            return DataAsByte;
        }


        public override long Seek(long offset, SeekOrigin origin)
        {
            return origin == SeekOrigin.Begin
                ? Stream.Seek(offset + HeaderSize, SeekOrigin.Begin) - HeaderSize
                : Stream.Seek(offset, origin) - HeaderSize;
        }

        public override void SetLength(long value)
        {
            var StreamLen = value + HeaderSize;
            this.StreamLen = StreamLen;

            if (StreamLen < MinLen)
            {
                var minCount = this.minCount;
                var MaxLen = StreamLen + minCount;
                this.MaxLen = MaxLen;
                MinLen = StreamLen - minCount;
                Stream.SetLength(MaxLen);
            }
            else if (StreamLen > MaxLen)
            {
                var minCount = this.minCount;
                var MaxLen = StreamLen + minCount;
                this.MaxLen = MaxLen;
                MinLen = StreamLen - minCount;
                Stream.SetLength(MaxLen);
            }
        }

        public void DropLen(int Count)
        {
            SetLength(Length - Count);
        }
        public void AddLen(long Count)
        {
            SetLength(Length + Count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            var NewLen = Position + count - Length;
            if (NewLen > 0)
                AddLen(NewLen);
            Stream.Write(buffer, offset, count);
        }
    }
}
