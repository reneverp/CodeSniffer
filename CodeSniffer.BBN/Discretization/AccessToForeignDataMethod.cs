using System;

namespace CodeSniffer.BBN.Discretization
{
    public enum ATFD_METHOD
    {
        LOW,
        MEDIUM,
        HIGH,
        VERY_HIGH,
        MEGA_HIGH,
        UTLRA_HIGH
    }

    public class AccessToForeignDataMethod
    {
        public static ATFD_METHOD Discretize(double value)
        {
            if (value <= 5 && value >= 0) return ATFD_METHOD.LOW;
            else
            if (value <= 10 && value > 5) return ATFD_METHOD.MEDIUM;
            else
            if (value <= 25 && value > 10) return ATFD_METHOD.HIGH;
            else
            if (value <= 75 && value > 25) return ATFD_METHOD.VERY_HIGH;
            else
            if (value <= 150 && value > 75) return ATFD_METHOD.MEGA_HIGH;
            else

                return ATFD_METHOD.UTLRA_HIGH;
        }
    }
}
