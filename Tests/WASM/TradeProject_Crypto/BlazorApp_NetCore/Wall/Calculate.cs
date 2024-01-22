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

        static (UniverseSummary Info, int Rate)[] MakeRateOnGrowing(
                UniverseSummary[] Values,
                Func<UniverseSummary, float> GetInfo)
        {
            var Rate = 0;
            return Values.OrderBy((c) => GetInfo(c)).ToArray()
                               .Select((c) => (c, Rate++)).ToArray();
        }

        static (UniverseSummary Info, int Rate)[] MakeRateOnDroping(
                UniverseSummary[] Values,
                Func<UniverseSummary, float> GetInfo)
        {
            var Rate = 0;
            return Values.OrderByDescending((c) => GetInfo(c)).ToArray()
                               .Select((c) => (c, Rate++)).ToArray();
        }

        static void MakeRateOnGrowing(
                UniverseSummary[] Values,
                Func<UniverseSummary, float> GetInfo,
                Action<(UniverseSummary Info, int Rate)> SetRate)
        {
            foreach (var Rate in MakeRateOnGrowing(Values, GetInfo))
                SetRate(Rate);
        }

        static void MakeRateOnDroping(
                UniverseSummary[] Values,
                Func<UniverseSummary, float> GetInfo,
                Action<(UniverseSummary Info,int Rate)> SetRate)
        {
            foreach (var Rate in MakeRateOnDroping(Values, GetInfo))
                SetRate(Rate);
        }

        static void MakeRate(
                UniverseSummary[] Values,
                Func<UniverseSummary, UniverseSummary.Info> GetInfo)
        {
            
        }

        static void MakeRate(
            UniverseSummary[] Values,
            Func<UniverseSummary, UniverseSummary.Info_HLC> GetInfo)
        {
            MakeRate(Values,(c) =>GetInfo(c).High);
            MakeRate(Values, (c) => GetInfo(c).Low);
            MakeRate(Values, (c) => GetInfo(c).Close);
        }

        static void MakeRate(
            UniverseSummary[] Values)
        {
            MakeRate(Values,(c) =>c.Info_1);
            MakeRate(Values, (c) => c.Info_7);
            MakeRate(Values, (c) => c.Info_15);
            MakeRate(Values, (c) => c.Info_30);
        }

        static async Task Calculate()
        {
            var Values = KeysNames.Select((c) => c.Summary).ToArray();
            MakeRate(Values);
        }
    }
}
