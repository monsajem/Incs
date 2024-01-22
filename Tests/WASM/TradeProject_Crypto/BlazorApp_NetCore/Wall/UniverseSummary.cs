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
                public float DEMA_50;
                public float DEMA_100;
                public float DEMA_150;
                public float DEMA_200;
                public float DEMA_250;
                public float DEMA_300;
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

                        var Last = Values.Last();


                        if (Len == 1)
                        {
                            var MaxPrice = Values.Max((c) => c.CLOSE);
                            PercentDown = math.GrowsPercent(MaxPrice, Last.CLOSE);
                            var MinPrice = Values.SkipWhile((c) => c.CLOSE != MaxPrice).Min((c) => c.CLOSE);
                            PercentUp = math.GrowsPercent(MinPrice, Last.CLOSE);
                        }

                        var DEMA = new float[0];

                        var Info = new Info
                        {
                            
                        };

                        return Info;

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
                //Info_7 = Calc(7);
                //Info_15 = Calc(15);
                //Info_30 = Calc(30);

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