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
                public float RSI_14;
                public int Rate_RSI_14;

                public float RSI_100;
                public int Rate_RSI_100;

                public float MFI_14;
                public int Rate_MFI_14;

                public float MFI_100;
                public int Rate_MFI_100;

                public float CRSI;
                public int Rate_CRSI;
                
                public float DEMA_Cross;
                public int Rate_DEMA_Cross;

                public bool DEMA_Cross_Closing;
                public int DEMA_Cross_Len;
            }

            public class Info_HLC
            {
                public Info Close;
                public Info High;
                public Info Low;
            }

            public UniverseSummary(Point[] Values)
            {

                var OrgValues = (Point[])Values.Clone();

                Func<int, Info_HLC> Calc =
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
                        AddMFI(Values, 14);
                        AddRSI(Values, 100);
                        AddMFI(Values, 100);
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

                        var RSI_14 = Last.OtherValues[0];
                        var MFI_14 = Last.OtherValues[1];
                        var RSI_100 = Last.OtherValues[2];
                        var MFI_100 = Last.OtherValues[3];
                        var CRSI = Last.OtherValues[4];
                        var DEMA = Last.OtherValues[5];
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
                            RSI_14 = RSI_14,
                            RSI_100 = RSI_100,
                            MFI_14 = MFI_14,
                            MFI_100 = MFI_100,
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

                    return new Info_HLC()
                    {
                        Close = Close,
                        High = HIGH,
                        Low = LOW
                    };
                };


                Info_1 = Calc(1);
                Info_7 = Calc(7);
                Info_15 = Calc(15);
                Info_30 = Calc(30);

            }

            public Info_HLC Info_1;
            public Info_HLC Info_7;
            public Info_HLC Info_15;
            public Info_HLC Info_30;

            public float PercentDown;
            public int PercentDownRate;

            public float PercentUp;
            public int PercentUpRate;

            public string Name;
            public string OtherInfo;
        }
    }
}