using System;

namespace CodeSniffer.BBN.Discretization
{
    public enum MAXNESTING
    {
        LOW,
        MEDIUM,
        HIGH,
        VERY_HIGH
    }

    public class MaximumNesting
    {
        public static MAXNESTING Discretize(double value)
        {
            if (value <= 2 && value >= 0) return MAXNESTING.LOW;
            else
            if (value <= 3 && value > 2) return MAXNESTING.MEDIUM;
            else
            if (value <= 4 && value > 3) return MAXNESTING.HIGH;
            else

                return MAXNESTING.VERY_HIGH;
        }
    }
}
