using CodeSniffer.Interfaces;
using System.Collections.Generic;

namespace CodeSniffer.Models.Metrics
{
    class NumberOfParameters : IMetric
    {
        private IList<string> _parameters;

        public string Name
        {
            get { return "Number Of Parameters"; }
            set { }
        }

        private double _value = -1;
        public double Value
        {
            get
            {
                if (_value == -1)
                {
                    _value = Calculate();
                }

                return _value;
            }
            set
            {
                _value = value;
            }
        }

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
