using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Metrics
{
    class LinesOfCode //: IMetric
    {
        public static double Calculate(string text)
        {
            var lines = text.Split(Environment.NewLine.ToArray());

            return lines.Count();
        }
    }
}
