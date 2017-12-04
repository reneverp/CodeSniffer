using System;

namespace CodeSniffer.BBN.Discretization
{
    public enum LAA
    {
        LOW,
        MEDIUM,
        HIGH,
        VERY_HIGH,
        MEGA_HIGH
    }

    public class LocalityOfAttributeAccesses
    {
        public static LAA Discretize(double value)
        {
            if (value <= 0.3 && value >= 0) return LAA.LOW;
            else
            if (value <= 1 && value > 0.3) return LAA.MEDIUM;
            else
            if (value <= 3 && value > 1) return LAA.HIGH;
            else
            if (value <= 10 && value > 3) return LAA.VERY_HIGH;
            else

                return LAA.MEGA_HIGH;
        }
    }
}
