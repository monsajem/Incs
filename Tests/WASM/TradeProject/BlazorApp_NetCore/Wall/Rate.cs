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
