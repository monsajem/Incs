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
        public static void AddRSI(Point[] Points, int Len)
        {
            Action MakeZeroRSI = () =>
            {
                for (int i = 0; i < Points.Length; i++)
                {
                    var Point = Points[i];
                    Insert(ref Point.OtherValues, 1000);
                    Points[i] = Point;
                }
            };
            if (Points.Length < Len)
                MakeZeroRSI();
            else
            {

                IEnumerable<RsiResult> Res;
                try
                {
                    Res = Points.GetRsi(Len);
                }
                catch
                {
                    MakeZeroRSI();
                    return;
                }
                int i = 0;
                foreach (var P in Res)
                {
                    var Point = Points[i];
                    if (P.Rsi == null)
                        Insert(ref Point.OtherValues, 1000);
                    else
                        Insert(ref Point.OtherValues, (float)P.Rsi);
                    Points[i++] = Point;
                }
            }
        }

        public static void AddMFI(Point[] Points, int Len)
        {
            Action MakeZeroRSI = () =>
            {
                for (int i = 0; i < Points.Length; i++)
                {
                    var Point = Points[i];
                    Insert(ref Point.OtherValues, 1000);
                    Points[i] = Point;
                }
            };
            if (Points.Length < Len)
                MakeZeroRSI();
            else
            {

                IEnumerable<MfiResult> Res;
                try
                {
                    Res = Points.GetMfi(Len);
                }
                catch
                {
                    MakeZeroRSI();
                    return;
                }
                int i = 0;
                foreach (var P in Res)
                {
                    var Point = Points[i];
                    if (P.Mfi == null)
                        Insert(ref Point.OtherValues, 1000);
                    else
                        Insert(ref Point.OtherValues, (float)P.Mfi);
                    Points[i++] = Point;
                }
            }
        }
        public static void AddCRSI(Point[] Points, int Len, int Up, int ROC)
        {
            Action MakeZeroRSI = () =>
            {
                for (int i = 0; i < Points.Length; i++)
                {
                    var Point = Points[i];
                    Insert(ref Point.OtherValues, 1000);
                    Points[i] = Point;
                }
            };
            if (Points.Length < Len)
                MakeZeroRSI();
            else
            {

                IEnumerable<ConnorsRsiResult> Res;
                try
                {
                    Res = Points.GetConnorsRsi(Len, Up, ROC);
                }
                catch
                {
                    MakeZeroRSI();
                    return;
                }
                int i = 0;
                foreach (var P in Res)
                {
                    var Point = Points[i];
                    if (P.ConnorsRsi == null)
                        Insert(ref Point.OtherValues, 1000);
                    else
                        Insert(ref Point.OtherValues, (float)P.ConnorsRsi);
                    Points[i++] = Point;
                }
            }
        }

        public static void AddCCI(Point[] Points, int Len)
        {
            Action MakeZeroRSI = () =>
            {
                for (int i = 0; i < Points.Length; i++)
                {
                    var Point = Points[i];
                    Insert(ref Point.OtherValues, 1000);
                    Points[i] = Point;
                }
            };
            if (Points.Length < Len)
                MakeZeroRSI();
            else
            {

                IEnumerable<CciResult> Res;
                try
                {
                    Res = Points.GetCci(Len);
                }
                catch
                {
                    MakeZeroRSI();
                    return;
                }
                int i = 0;
                foreach (var P in Res)
                {
                    var Point = Points[i];
                    if (P.Cci == null)
                        Insert(ref Point.OtherValues, 1000);
                    else
                        Insert(ref Point.OtherValues, (float)P.Cci);
                    Points[i++] = Point;
                }
            }
        }

        public static void AddDEMA_Cross(Point[] Points, int MinLen, int MaxLen)
        {
            Action MakeZeroRSI = () =>
            {
                for (int i = 0; i < Points.Length; i++)
                {
                    var Point = Points[i];
                    Insert(ref Point.OtherValues, 1000);
                    Points[i] = Point;
                }
            };
            DemaResult[] Res_7;
            DemaResult[] Res_35;
            {
                try
                {
                    Res_7 = Points.GetDoubleEma(MinLen).ToArray();
                    Res_35 = Points.GetDoubleEma(MaxLen).ToArray();
                }
                catch
                {
                    MakeZeroRSI();
                    return;
                }
                for (int i = 0; i < Points.Length; i++)
                {
                    var DEMA_7 = Res_7[i];
                    var DEMA_35 = Res_35[i];
                    var Point = Points[i];
                    if (DEMA_7.Dema == null || DEMA_35.Dema == null)
                        Insert(ref Point.OtherValues, 1000);
                    else
                        Insert(ref Point.OtherValues,
                            math.GrowsPercent((float)DEMA_35.Dema, (float)DEMA_7.Dema));
                    Points[i] = Point;
                }
            }
        }
    }
}