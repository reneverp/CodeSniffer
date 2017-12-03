using System;

namespace CodeSniffer.BBN.Discretization
{
    public enum ATFD
    {
        LOW,
        MEDIUM,
        HIGH,
        VERY_HIGH
    }

    public class AccessToForeignDataClass
    {
        public static ATFD Discretize(double value)
        {
            if (value <= 10 && value >= 0 ) return ATFD.LOW;    else
            if (value <= 20 && value > 10) return ATFD.MEDIUM; else
            if (value <= 50 && value > 20) return ATFD.HIGH;   else

            return ATFD.VERY_HIGH;
        }
    }
}
