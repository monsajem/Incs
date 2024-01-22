using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monsajem_Incs.Collection
{
    public class UnsafeCollector<t>
    {
        private List<(t Value, t[] Values, int From, int Len)> Collects =
            new List<(t Value, t[] Values, int From, int Len)>();
        private int Len;

        public int Length { get => Len; }
        public int Position { get => Len; }

        public void Write(t[] Values, int From, int Len)
        {
            Collects.Add((default, Values, From, Len));
            this.Len += Len;
        }
        public void Write(t[] Values) => Write(Values, 0, Values.Length);
        public void Write(t Value)
        {
            Collects.Add((Value, null, 0, 1));
            this.Len += 1;
        }

        public void WriteByte(t Value)
        {
            Collects.Add((Value, null, 0, 1));
            this.Len += 1;
        }

        public t[] ToArray()
        {
            if (Collects.Count == 1)
            {
                var Values = Collects[0];
                if (Values.Values == null)
                    return new t[] { Values.Value };
                else if (Values.From == 0 && Values.Len == Values.Values.Length)
                    return Values.Values;
            }

            var Result = new t[Len];
            var Pos = 0;
            foreach (var Values in Collects)
            {
                var BytesLen = Values.Len;
                if (Values.Values == null)
                    Result[Pos] = Values.Value;
                else
                   System.Array.Copy(Values.Values, Values.From, Result, Pos, BytesLen);
                Pos += BytesLen;
            }
            //this.Collects.Clear();
            //this.Collects.Add((default, Result, 0, Result.Length));
            return Result;
        }
    }
}
