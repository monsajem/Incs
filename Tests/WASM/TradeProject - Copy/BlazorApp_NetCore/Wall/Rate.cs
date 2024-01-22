using Monsajem_Incs.Collection.Array.ArrayBased.DynamicSize;
using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using static Monsajem_Incs.Collection.Array.Extentions;

namespace Calculate_wall
{
    public partial class Calculator
    {

        static void MakeRateOnGrowing(
                UniverseSummary[] Values,
                Func<UniverseSummary, float> GetInfo)
        {
            var Rate = 0;
            foreach (var Value in Values.OrderBy((c) => GetInfo(c)))
                Value.Rate.AddRate(Rate++, GetInfo(Value));
        }

        static void MakeRateOnDroping(
            UniverseSummary[] Values,
            Func<UniverseSummary, float> GetInfo)
        {
            var Rate = 0;
            foreach (var Value in Values.OrderByDescending((c) => GetInfo(c)))
                Value.Rate.AddRate(Rate++, GetInfo(Value));
        }

        static void DeleteOutLiersRates(
            ref UniverseSummary[] Values)
        {
            var Range = Values;
            {
                var Avg = Range.Average((c) => c.Rate.RateAvg);
                var Max = Range.Max((c) => c.Rate.RateAvg);
                var x = Max / Avg;
                Avg = Avg / x;
                Values = Values.Where((c) => c.Rate.RateAvg < Avg).ToArray();
            }
            {
                var Avg = Range.Average((c) => c.Rate.RateMax);
                var Max = Range.Max((c) => c.Rate.RateMax);
                var x = Max / Avg;
                Avg = Avg / x;
                Values = Values.Where((c) => c.Rate.RateMax < Avg).ToArray();
            }
            {
                var Avg = Range.Average((c) => c.Rate.RateMin);
                var Max = Range.Max((c) => c.Rate.RateMin);
                var x = Max / Avg;
                Avg = Avg / x;
                Values = Values.Where((c) => c.Rate.RateMin < Avg).ToArray();
            }
            {
                var Avg = Range.Average((c) => c.Rate.DiffRatesAvg);
                var Max = Range.Max((c) => c.Rate.DiffRatesAvg);
                var x = Max / Avg;
                Avg = Avg / x;
                Values = Values.Where((c) => c.Rate.DiffRatesAvg < Avg).ToArray();
            }
            {
                var Avg = Range.Average((c) => c.Rate.DiffRatesMax);
                var Max = Range.Max((c) => c.Rate.DiffRatesMax);
                var x = Max / Avg;
                Avg = Avg / x;
                Values = Values.Where((c) => c.Rate.DiffRatesMax < Avg).ToArray();
            }
            {
                var Avg = Range.Average((c) => c.Rate.DiffRatesMin);
                var Max = Range.Max((c) => c.Rate.DiffRatesMin);
                var x = Max / Avg;
                Avg = Avg / x;
                Values = Values.Where((c) => c.Rate.DiffRatesMin < Avg).ToArray();
            }
        }

        public class Rate
        {
            public int[] Rates = new int[0];
            public float[] Values = new float[0];
            public float RateAvg;
            public int RateMax;
            public int RateMin;
            public float[][] DiffRates;
            public float DiffRatesAvg;
            public float DiffRatesMax;
            public float DiffRatesMin;

            public void AddRate(int Rate, float Value)
            {
                Insert(ref Rates, Rate);
                Insert(ref Values, Value);
                RateAvg = (float)Rates.Average();
                RateMax = Rates.Max();
                RateMin = Rates.Min();
                DiffRates = new float[Rates.Length][];
                for (int i = 0; i < Rates.Length; i++)
                {
                    DiffRates[i] = new float[0];
                    for (int j = 0; j < Rates.Length; j++)
                    {
                        if (i != j)
                        {
                            var Diff = math.Distacnce(Rates[i], Rates[j]);
                            Insert(ref DiffRates[i], Diff);
                        }
                    }
                }
                if (Rates.Length > 1)
                {
                    DiffRatesAvg = DiffRates.Average((c) => c.Average());
                    DiffRatesMax = DiffRates.Max((c) => c.Max());
                    DiffRatesMin = DiffRates.Min((c) => c.Min());
                }
            }

            public override string ToString()
            {
                var Res = "AVG: " + RateAvg.ToString() + " Contents:";

                foreach (var Rate in this.Rates)
                    Res = Res + " , " + Rate.ToString();

                return Res;
            }
        }

    }
}
