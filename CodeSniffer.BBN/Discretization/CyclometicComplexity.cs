using System;

namespace CodeSniffer.BBN.Discretization
{
    public enum CYCLO
    {
        LOW,
        MEDIUM,
        HIGH,
        VERY_HIGH
    }

    public class CyclometicComplexity
    {
        public static CYCLO Discretize(double value)
        {
            if (value <= 5 && value >= 0) return CYCLO.LOW;
            else
            if (value <= 10 && value > 5) return CYCLO.MEDIUM;
            else
            if (value <= 20 && value > 10) return CYCLO.HIGH;
            else

                return CYCLO.VERY_HIGH;
        }
    }
}
