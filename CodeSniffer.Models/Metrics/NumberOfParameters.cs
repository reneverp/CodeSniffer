using CodeSniffer.Interfaces;
using System.Collections.Generic;

namespace CodeSniffer.Models.Metrics
{
    class NumberOfParameters : IMetric
    {
        private IList<string> _parameters;

        public string Name => "Number Of Parameters";

        public NumberOfParameters(IList<string> parameters)
        {
            _parameters = parameters;
        }

        public double Calculate()
        {
            return _parameters.Count;
        }
    }
}
