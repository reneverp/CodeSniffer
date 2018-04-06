// License issued by BayesFusion Licensing Server
// This code must be executed before any other SMILE.NET object is created

using System;

public class SmileLicense
{
    private static bool licenseInitialized = false;

    private static Object lockObj = new Object();


    public static void ActivateLicense()
    {
        lock(lockObj)
        {
            if (!licenseInitialized)
            {

                new Smile.License(
                    "SMILE LICENSE 5f115353 046adcd1 50bc49bf " +
                    "THIS IS AN ACADEMIC LICENSE AND CAN BE USED " +
                    "SOLELY FOR ACADEMIC RESEARCH AND TEACHING, " +
                    "AS DEFINED IN THE BAYESFUSION ACADEMIC " +
                    "SOFTWARE LICENSING AGREEMENT. " +
                    "Serial #: dnx3ekgiulh1ri8l3flgkbacp " +
                    "Issued for: Ren\u00e9 van Erp (reneverp@gmail.com) " +
                    "Academic institution: Open Universiteit Nederland " +
                    "Valid until: 2018-10-08 " +
                    "Issued by BayesFusion activation server",
                    new byte[] {
            0x13,0xca,0xf9,0xa6,0x3c,0x8c,0x77,0xf6,0x27,0x0f,0x52,0x48,0x31,0x45,0x06,0x44,
            0x84,0x5d,0xae,0x79,0x97,0x7c,0xd6,0xbe,0x11,0x9e,0xfc,0x7f,0x02,0x71,0xb5,0x15,
            0xd5,0x43,0x4b,0xe9,0xe1,0x5f,0x49,0x78,0x7c,0x48,0xfe,0x11,0xe6,0x5b,0x95,0xd1,
            0x47,0x48,0xc0,0x46,0xc8,0xb1,0xf0,0x30,0xad,0xed,0xb4,0x56,0x0c,0x9f,0x14,0x89
                    }
                );



                licenseInitialized = true;
            }
        }
    }
}

