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
        public class math
        {
            public static float Distacnce(float A, float B)
            {
                A = A - B;
                if (A < 0)
                    A = A * -1f;
                return A;
            }

            public static float GetCloser(float Value, float A, float B, float Default = float.NaN)
            {
                if (Default == float.NaN)
                    Default = A;
                var Close = CloserTo(Value, A, B);
                if (Close == 0)
                    return Default;
                else if (Close == -1)
                    return A;
                else
                    return B;
            }

            public static int CloserTo(float Value, float A, float B)
            {
                var ALen = Distacnce(Value, A);
                var BLen = Distacnce(Value, B);
                if (ALen < BLen)
                    return -1;
                else if (ALen > BLen)
                    return 1;
                else
                    return 0;
            }

            public static float Max(float A, float B)
            {
                if (A > B)
                    return A;
                else
                    return B;
            }

            public static float Min(float A, float B)
            {
                if (A < B)
                    return A;
                else
                    return B;
            }

            public static float GrowsPercent(float A, float B)
            {
                if (A == B)
                    return 0f;
                if (A == 0)
                    A = 1;
                var Result = ((A - B) / Math.Abs(A)) * -1f;
                return Result;
            }

            internal static bool SameSign(float A, float B)
            {
                if (A == 0 & B == 0)
                    return true;
                else if (A < 0 & B < 0)
                    return true;
                else if (A > 0 & B > 0)
                    return true;
                else
                    return false;
            }
        }


    }
}
