using System;

namespace CodeSniffer.BBN.Discretization
{
    public enum LOC_METHOD
    {
        LOW,
        MEDIUM,
        HIGH,
        VERY_HIGH
    }

    public class LinesOfCodeMethod
    {
        public static LOC_METHOD Discretize(double value)
        {
            if (value <= 20 && value >= 0) return LOC_METHOD.LOW;
            else
            if (value <= 50 && value > 20) return LOC_METHOD.MEDIUM;
            else
            if (value <= 85 && value > 50) return LOC_METHOD.HIGH;
            else

                return LOC_METHOD.VERY_HIGH;
        }
    }
}
