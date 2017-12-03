using System;

namespace CodeSniffer.BBN.Discretization
{
    public enum TCC
    {
        LOW,
        MEDIUM,
        HIGH
    }

    public class TightClassCohesion
    {
        public static TCC Discretize(double value)
        {
            if (value <= 0.1 && value >= 0  ) return TCC.LOW;    else
            if (value <= 0.3 && value > 0.1) return TCC.MEDIUM; else

            return TCC.HIGH;
        }
    }
}
