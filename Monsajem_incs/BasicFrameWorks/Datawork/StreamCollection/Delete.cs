using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monsajem_Incs.Serialization;

namespace Monsajem_Incs.Collection
{
    public partial class StreamCollection<ValueType>
    {
        public override void DeleteByPosition(int Position)
        {
#if DEBUG
            Info.Browse(this);
#endif
            var DataLoc = Info.PopData(Position);
            var NGap = DataLoc;
            Data BGap;
            if (Info.PopNextGap(ref NGap))
            {
                DataLoc.To = NGap.To;
                DataLoc.Len += NGap.Len;
            }
            if (DataLoc.To == Info.StreamLen - 1)//// is last data;
            {
                BGap = DataLoc;
                if (Info.PopBeforeGap(ref BGap))
                    DataLoc.Len += BGap.Len;
                DeleteLen(DataLoc.Len);
                Stream.Flush();
            }
            else
            {
                BGap = DataLoc;
                if (Info.PopBeforeGap(ref BGap))
                {
                    DataLoc.From = BGap.From;
                    DataLoc.Len += BGap.Len;
                }
                Info.InsertGap(DataLoc);
            }
            Length -= 1;
#if DEBUG
            Info.Browse(this);
#endif
        }

        protected override StreamCollection<ValueType> MakeSameNew()
        {
            throw new NotImplementedException();
        }

        public override void DeleteFrom(int from)
        {
            throw new NotImplementedException();
        }
    }
}
