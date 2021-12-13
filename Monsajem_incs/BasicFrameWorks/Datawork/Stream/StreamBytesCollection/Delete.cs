using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monsajem_Incs.Serialization;

namespace Monsajem_Incs.Collection
{
    public partial class StreamCollection
    {
        public override void DeleteByPosition(int Position)
        {
#if DEBUG
            Debug(this);
#endif
            var DataLoc = PopInfo(Position);
            var NGap = DataLoc;
            Data BGap;
            if (PopNextGap(ref NGap))
            {
                DataLoc.To = NGap.To;
                DataLoc.Len += NGap.Len;
            }
            if (DataLoc.To == Stream.Length - 1)//// is last data;
            {
                BGap = DataLoc;
                if (PopBeforeGap(ref BGap))
                    DataLoc.Len += BGap.Len;
                Stream.DropLen(DataLoc.Len);
                Stream.Flush();
            }
            else
            {
                BGap = DataLoc;
                if (PopBeforeGap(ref BGap))
                {
                    DataLoc.From = BGap.From;
                    DataLoc.Len += BGap.Len;
                }
                InsertGap(DataLoc);
            }
            Length -= 1;
#if DEBUG
            Debug(this);
#endif
        }

        protected override StreamCollection MakeSameNew()
        {
            throw new NotImplementedException();
        }

        public override void DeleteFrom(int from)
        {
            throw new NotImplementedException();
        }
    }
}
