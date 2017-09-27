using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Metrics
{
    class CyclometicComplexity
    {
        public double Calculate()
        {
            var text = "";//context.GetText();
            var startText = text.Substring(0, Math.Min(text.Length - 1, 10)).ToLower();

            if (startText.StartsWith("if") ||
                startText.StartsWith("else") ||
                startText.StartsWith("for") ||
                startText.StartsWith("foreach") ||
                startText.StartsWith("while") ||
                startText.StartsWith("do") ||
                startText.StartsWith("catch") ||
                startText.StartsWith("switch") ||
                startText.StartsWith("case"))
            {
                //currentMethod.Complexity++;
            }


            return 0; //TODO!
        }
    }
}
