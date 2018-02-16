using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Models.Utils
{
    static class LineEndingConverter
    {
        public static string ConvertToCRLF(string input)
        {
            //if the file doesn't contain CRLF, we assume it only has LF.
            if(!input.Contains(Environment.NewLine))
            {
                return input.Replace("\n", Environment.NewLine);
            }

            return input;
        }
    }
}
