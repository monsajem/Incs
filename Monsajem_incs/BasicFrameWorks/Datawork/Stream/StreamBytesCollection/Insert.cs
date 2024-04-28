using Monsajem_Incs.Serialization;

namespace Monsajem_Incs.Collection
{

    public partial class StreamCollection
    {

        public override void Insert(byte[] DataAsByte, int Position)
        {
#if DEBUG
            Debug(this);
#endif
            var Info = InsertInfo(DataAsByte.Length, Position);

            Stream.Seek(Info.Info.From, System.IO.SeekOrigin.Begin);
            Stream.Write(Info.Header.Serialize(), 0, HeadSize);
            Stream.Write(DataAsByte, 0, DataAsByte.Length);
            Stream.Flush();
            Length += 1;
#if DEBUG
            Debug(this);
#endif
        }

        private (DataHeader Header, Data Info) InsertInfo(int DataLen, int Pos)
        {
            var TotalLen = DataLen + HeadSize;
            Data Gap;
            Data Data;
            if (PopGapMinLen(TotalLen, out Gap) == false)//// haven't free gap
            {
                var StreamLen = (int)Stream.Length;
                Stream.AddLen(TotalLen);
                Data = new Data()
                {
                    From = StreamLen,
                    Len = TotalLen,
                    To = StreamLen + TotalLen - 1
                };
            }
            else
            {
                Data = new Data()
                {
                    From = Gap.From,
                    Len = TotalLen,
                    To = Gap.From + TotalLen - 1
                };
                if (Gap.Len > TotalLen)
                {
                    Gap.Len -= TotalLen;
                    Gap.From += TotalLen;
                    var NextGap = Gap;
                    if (PopNextGap(ref NextGap))
                    {
                        Gap.Len += NextGap.Len;
                        Gap.To = NextGap.To;
                    }
                    InsertGap(Gap);
                }
            }

            var Header = new DataHeader
            {
                DataLen = Data.Len
            };
            Keys.Insert(Data, Pos);
            Header.NextData = Keys.Length == Pos + 1 ? -1 : Keys[Pos + 1].From;

            return (Header, Data);
        }
    }
}
