using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Metrics
{
    interface IMetric
    {
        double Calculate(string text);
    }
}
