using System;

namespace CodeSniffer.BBN.Discretization
{
    public enum NOAV
    {
        LOW,
        MEDIUM,
        HIGH,
        VERY_HIGH
    }

    public class NumberOfAccessedVariables
    {
        public static NOAV Discretize(double value)
        {
            if (value <= 5 && value >= 0) return NOAV.LOW;
            else
            if (value <= 10 && value > 5) return NOAV.MEDIUM;
            else
            if (value <= 20 && value > 10) return NOAV.HIGH;
            else

                return NOAV.VERY_HIGH;
        }
    }
}
