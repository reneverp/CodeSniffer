using System;

namespace CodeSniffer.BBN.Discretization
{
    public enum FDP
    {
        LOW,
        MEDIUM,
        HIGH,
        VERY_HIGH
    }

    public class ForeignDataProviders
    {
        public static FDP Discretize(double value)
        {
            if (value <= 1 && value >= 0) return FDP.LOW;
            else
            if (value <= 3 && value > 1) return FDP.MEDIUM;
            else
            if (value <= 5 && value > 3) return FDP.HIGH;
            else

                return FDP.VERY_HIGH;
        }
    }
}
