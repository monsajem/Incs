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


        public class UniverseSummary
        {
            public class Info
            {
                public float RSI;
                public int Rate_RSI;

                public float CRSI;
                public int Rate_CRSI;
                
                public float DEMA_Cross;
                public int Rate_DEMA_Cross;

                public bool DEMA_Cross_Closing;
                public int DEMA_Cross_Len;
            }

            public UniverseSummary(Point[] Values)
            {

                var OrgValues = (Point[])Values.Clone();

                Func<int, (Info Close, Info High, Info Low)> Calc =
                (Len) =>
                {
                    Func<int, Info> Calc =
                    (Len) =>
                    {
                        if (Len > 1)
                        {
                            var i = Len;
                            Values = Values.Reverse().Where((c) => ((i++) % Len) == 0).Reverse().ToArray();
                        }

                        AddRSI(Values, 14);
                        AddCRSI(Values, 14, 7, 100);
                        AddDEMA_Cross(Values, 7, 35);
                        var Last = Values.Last();


                        if (Len == 1)
                        {
                            var MaxPrice = Values.Max((c) => c.CLOSE);
                            PercentDown = math.GrowsPercent(MaxPrice, Last.CLOSE);
                            var MinPrice = Values.SkipWhile((c) => c.CLOSE != MaxPrice).Min((c) => c.CLOSE);
                            PercentUp = math.GrowsPercent(MinPrice, Last.CLOSE);
                        }

                        var RSI = Last.OtherValues[0];
                        var CRSI = Last.OtherValues[1];
                        var DEMA = Last.OtherValues[2];
                        var DemaLen = 0;
                        var DemaClosing = false;


                        try
                        {
                            var Dema_BeforeOfLast = Values.Reverse().Skip(1).First().OtherValues[2];
                            if (DEMA < 0)
                            {
                                DemaLen = Values.Reverse()
                                                .TakeWhile((c) => c.OtherValues[2] < 0)
                                                .Count();
                                var LastDema =
                                DemaClosing = DEMA > Dema_BeforeOfLast;
                            }
                            else
                            {
                                DemaLen = Values.Reverse()
                                                .TakeWhile((c) => c.OtherValues[2] >= 0)
                                                .Count();
                                DemaClosing = DEMA < Dema_BeforeOfLast;
                            }
                        }
                        catch { }

                        return new Info
                        {
                            RSI = RSI,
                            CRSI = CRSI,
                            DEMA_Cross = DEMA,
                            DEMA_Cross_Len = DemaLen,
                            DEMA_Cross_Closing = DemaClosing
                        };

                    };

                    Values = (Point[])OrgValues.Clone();
                    var Close = Calc(Len);

                    Values = (Point[])OrgValues.Clone();
                    Values = Values.Select((c) =>
                    {
                        c.CLOSE = c.HIGH;
                        return c;
                    }).ToArray();
                    var HIGH = Calc(Len);

                    Values = (Point[])OrgValues.Clone();
                    Values = Values.Select((c) =>
                    {
                        c.CLOSE = c.LOW;
                        return c;
                    }).ToArray();
                    var LOW = Calc(Len);

                    return (Close, HIGH, LOW);
                };


                Info_1 = Calc(1);
                Info_7 = Calc(7);
                Info_15 = Calc(15);
                Info_30 = Calc(30);

            }

            public (Info Close, Info High,Info Low) Info_1;
            public (Info Close, Info High, Info Low) Info_7;
            public (Info Close, Info High, Info Low) Info_15;
            public (Info Close, Info High, Info Low) Info_30;

            public float PercentDown;
            public float PercentUp;

            public string Name;
            public string OtherInfo;
            public Rate Rate;


        }
    }
}