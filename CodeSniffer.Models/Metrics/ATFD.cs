using CodeSniffer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Models.Metrics
{
    //Access to Foreign Data
    class ATFD : IMetric
    {
        private IList<MethodInvocation> _outerFieldAccessInvocations;

        public string Name
        {
            get { return "ATFD"; }
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

        public ATFD(IList<MethodInvocation> outerFieldAccessInvocations)
        {
            _outerFieldAccessInvocations = outerFieldAccessInvocations;
        }

        public double Calculate()
        {
            return _outerFieldAccessInvocations.Count;
        }
    }
}
