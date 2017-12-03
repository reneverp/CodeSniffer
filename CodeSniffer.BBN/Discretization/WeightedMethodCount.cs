using System;

namespace CodeSniffer.BBN.Discretization
{
    public enum WMC
    {
        LOW,
        MEDIUM,
        HIGH,
        VERY_HIGH
    }

    public class WeightedMethodCount
    {
        public static WMC Discretize(double value)
        {
            if (value <= 3 && value > 1 ) return WMC.LOW;    else
            if (value <= 5 && value > 3 ) return WMC.MEDIUM; else
            if (value <= 10 && value > 5) return WMC.HIGH;   else

            return WMC.VERY_HIGH;
        }
    }
}
