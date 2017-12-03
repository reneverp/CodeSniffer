using System;

namespace CodeSniffer.BBN.Discretization
{
    public enum LOC
    {
        LOW,
        MEDIUM,
        HIGH,
        VERY_HIGH
    }

    public class LinesOfCode
    {
        public static LOC Discretize(double value)
        {
            if (value <= 100 && value > 0  ) return LOC.LOW;    else
            if (value <= 200 && value > 100) return LOC.MEDIUM; else
            if (value <= 300 && value > 200) return LOC.HIGH;   else

            return LOC.VERY_HIGH;
        }
    }
}
