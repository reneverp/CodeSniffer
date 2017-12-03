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
        private List<MethodInvocation> _outerFieldAccessInvocations;
        private IList<Method> _methods;

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

        public ATFD(IList<Method> methods)
        {
            _methods = methods;
            _outerFieldAccessInvocations = new List<MethodInvocation>();
        }

        public ATFD(List<MethodInvocation> outerFieldAccessInvocations)
        {
            _methods = new List<Method>();
            _outerFieldAccessInvocations = outerFieldAccessInvocations;
        }

        public double Calculate()
        {
            if(_methods.Count > 0)
            {
                _outerFieldAccessInvocations.Clear();
                _methods.ToList().ForEach(x => _outerFieldAccessInvocations.AddRange(x.ForeignDataAccessInvocations));
            }

            return _outerFieldAccessInvocations.Count; 
        }
    }
}
