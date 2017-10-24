using CodeSniffer.Interfaces;
using System;
using System.Linq;

namespace CodeSniffer.Models.Metrics
{
    class LinesOfCode : IMetric
    {
        private string _code;

        public string Name
        {
            get { return "Lines of Code"; }
            set { }
        }

        private double _value = -1;
        public double Value {
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
