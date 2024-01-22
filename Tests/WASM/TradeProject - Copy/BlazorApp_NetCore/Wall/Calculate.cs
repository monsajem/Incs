using Monsajem_Incs.Collection.Array.ArrayBased.DynamicSize;
using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Monsajem_Incs.Collection.Array.Extentions;

namespace Calculate_wall
{
    partial class Calculator
    {
        static async Task Calculate()
        {
            var Values = KeysNames.Select((c) => c.Summary).ToArray();
            
            //var ValidValues = Values.Where((c) =>
            //{
            //    //if (c.DayRSI == 0 &&
            //    //    c.CRSI == 0 &&
            //    //    c.DEMA_Cross == 100)
            //    //    return false;
            //    return true;
            //}).ToArray();

            //Values = ValidValues;

            //0
            MakeRateOnGrowing(Values, (c) => c.Info_1.Close.RSI);
            //1
            MakeRateOnGrowing(Values, (c) => c.Info_1.Close.DEMA_Cross);
            //2
            MakeRateOnGrowing(Values, (c) => c.Info_1.Close.CRSI);

            //3
            MakeRateOnGrowing(Values, (c) => c.Info_7.Close.RSI);
            //4
            MakeRateOnGrowing(Values, (c) => c.Info_7.Close.DEMA_Cross);
            //5
            MakeRateOnGrowing(Values, (c) => c.Info_7.Close.CRSI);

            //6
            MakeRateOnGrowing(Values, (c) => c.Info_30.Close.RSI);
            //7
            MakeRateOnGrowing(Values, (c) => c.Info_30.Close.DEMA_Cross);
            //8
            MakeRateOnGrowing(Values, (c) => c.Info_30.Close.CRSI);

            //9
            MakeRateOnGrowing(Values, (c) => c.PercentDown);

            //10 Day
            MakeRateOnGrowing(Values, (c) => c.Rate.Rates[0]+
                                             c.Rate.Rates[1]+
                                             c.Rate.Rates[2]);

            //11 Week
            MakeRateOnGrowing(Values, (c) => c.Rate.Rates[3] +
                                             c.Rate.Rates[4] +
                                             c.Rate.Rates[5]);

            //12 Mount
            MakeRateOnGrowing(Values, (c) => c.Rate.Rates[6] +
                                             c.Rate.Rates[7] +
                                             c.Rate.Rates[8]);


            //13
            MakeRateOnGrowing(Values, (c) => c.Info_1.High.RSI);
            //14
            MakeRateOnGrowing(Values, (c) => c.Info_1.High.DEMA_Cross);
            //15
            MakeRateOnGrowing(Values, (c) => c.Info_1.High.CRSI);

            //16
            MakeRateOnGrowing(Values, (c) => c.Info_1.Low.RSI);
            //17
            MakeRateOnGrowing(Values, (c) => c.Info_1.Low.DEMA_Cross);
            //18
            MakeRateOnGrowing(Values, (c) => c.Info_1.Low.CRSI);

            var Rated = Values;

            DeleteOutLiersRates(ref Rated);

            Rated = Rated.OrderBy((c) => c.Rate.RateAvg).ToArray();
        }
    }
}
