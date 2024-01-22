using Monsajem_Incs.Collection.Array.ArrayBased.DynamicSize;
using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using static Monsajem_Incs.Collection.Array.Extentions;

namespace Calculate_wall
{
    partial class Calculator
    {

        public struct Point : IQuote
        {
            public float FIRST;
            public float HIGH;
            public float LOW;
            public float CLOSE;
            public float VALUE;
            public float VOL;
            public float OPENINT;
            public string PER;
            public float OPEN;
            public float LAST;
            public float[] OtherValues;

            public Int64 Uni_Total_Value;
            public float Uni_ChangeToUping;
            public float Uni_ChangeToDown;
            public float Uni_Posetive;
            public float Uni_Negative;

            public DateTime date;

            DateTime IQuote.Date => date;

            decimal IQuote.Open => (decimal)OPEN;

            decimal IQuote.High => (decimal)HIGH;

            decimal IQuote.Low => (decimal)LOW;

            decimal IQuote.Close => (decimal)CLOSE;

            decimal IQuote.Volume => (decimal)VOL;
        }

        public static Point[] GetPoints(string DataAsString)
        {

            static string[] GetValues(string Value)
            {
                var Values = new string[0];
                var LastPos = 0;
                var PosOfNext = Value.IndexOf(",") + 1;
                while (PosOfNext != 0)
                {
                    Insert(ref Values, Value.Substring(LastPos, (PosOfNext - LastPos) - 1));
                    LastPos = PosOfNext;
                    PosOfNext = Value.IndexOf(",", PosOfNext + 1) + 1;
                }
                Insert(ref Values, Value.Substring(LastPos));
                return Values;
            }

            Point[] Datas;

            {
                var Count = 0;
                var StringData = new System.IO.StringReader(DataAsString);
                var Values = GetValues(StringData.ReadLine());
                var Line = StringData.ReadLine();
                while (Line != null)
                {
                    Count++;
                    Line = StringData.ReadLine();
                }

                Datas = new Point[Count];
            }

            {
                var Pos = Datas.Length-1;
                var StringData = new System.IO.StringReader(DataAsString);
                var Values = GetValues(StringData.ReadLine());
                var Line = StringData.ReadLine();
                while (Line != null)
                {
                    //این فایل حاوی اطلاعاتی از جمله عنوان لاتین شرکت (TICKER)، تاریخ های معاملات(DTYYYYMMDD)، اولین قیمت(FIRST)، بالاترین قیمت(HIGH)، پایین ترین قیمت(LOW)، قیمت پایانی (CLOSE)، ارزش معاملات (VALUE)، حجم معاملات (VOL)، حجم معامله ابتدایی (OPENINT)، دوره زمانی اطلاعات (PER)، قیمت آغازین (OPEN) و قیمت آخرین معامله (LAST) است
                    //<TICKER>,<DTYYYYMMDD>,<FIRST>,<HIGH>,<LOW>,<CLOSE>,<VALUE>,<VOL>,<OPENINT>,<PER>,<OPEN>,<LAST>
                    Values = GetValues(Line);

                    var Point = new Point()
                    {

                        date = new DateTime(
                            int.Parse(Values[1].Substring(0, 4)),
                            int.Parse(Values[1].Substring(4, 2)),
                            int.Parse(Values[1].Substring(6, 2))),
                        FIRST = float.Parse(Values[2]),
                        HIGH = float.Parse(Values[3]),
                        LOW = float.Parse(Values[4]),
                        CLOSE = float.Parse(Values[5]),
                        VALUE = float.Parse(Values[6]),
                        VOL = float.Parse(Values[7]),
                        OPENINT = float.Parse(Values[8]),
                        PER = Values[9],
                        OPEN = float.Parse(Values[10]),
                        LAST = float.Parse(Values[11]),
                        OtherValues = new float[0]
                    };

                    Datas[Pos--]=Point;
                    Line = StringData.ReadLine();
                }
            }

            return Datas;
        }
    }
}
