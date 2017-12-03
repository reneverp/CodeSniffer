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
                    "SMILE LICENSE 56038812 0efa0e45 822892fe " +
                    "THIS IS AN ACADEMIC LICENSE AND CAN BE USED  " +
                    "SOLELY FOR ACADEMIC RESEARCH AND TEACHING, " +
                    "AS DEFINED IN THE BAYESFUSION ACADEMIC  " +
                    "SOFTWARE LICENSING AGREEMENT. " +
                    "Serial #: 2saqhhnj3qi75z9hms75x55hb " +
                    "Issued for: Ren\u00e9 van Erp (rene@vanerp-it.nl) " +
                    "Academic institution: Open Universiteit Nederland " +
                    "Valid until: 2018-06-06 " +
                    "Issued by BayesFusion activation server",
                    new byte[] {
            0x16,0x03,0x5e,0x5d,0xf5,0x6b,0x34,0x36,0xbc,0xf5,0x71,0x9f,0x3a,0x20,0x6f,0x63,
            0xf5,0x31,0x30,0xac,0x58,0x7b,0xfc,0x1b,0x28,0x29,0xd6,0xc4,0x86,0xf4,0x03,0x7e,
            0x1a,0x5c,0x6d,0xc1,0x04,0xec,0x38,0x6d,0x41,0xdc,0x09,0x8c,0x6c,0x8f,0x73,0x9b,
            0xb5,0x1e,0x23,0xe7,0xea,0xfe,0xd0,0x9b,0x6c,0xdd,0xdc,0x4e,0x82,0x1e,0x3e,0xeb
                    }
                );

                licenseInitialized = true;
            }
        }
    }
}
