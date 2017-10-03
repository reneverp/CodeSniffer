using CodeSniffer.Interfaces;
using System;
using System.Linq;

namespace CodeSniffer.Models.Metrics
{
    class LinesOfCode : IMetric
    {
        private string _code;

        public string Name => "Lines of Code";

        public LinesOfCode(string code)
        {
            _code = code;
        }

        public double Calculate()
        {
            var lines = _code.Split(Environment.NewLine.ToArray());

            return lines.Count();
        }
    }
}
