using CodeSniffer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Models.Metrics
{
    //Locality of attribute access
    class LAA : IMetric
    {
        private IList<MethodInvocation> _totalFieldAccessInvocations;

        public string Name
        {
            get { return "LAA"; }
            set { }
        }

        private double _value = -1;
        private Class _parentClass;

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

        public LAA(Class parentClass, IList<MethodInvocation> totalFieldAccessInvocations)
        {
            _totalFieldAccessInvocations = totalFieldAccessInvocations;
            _parentClass = parentClass;
        }

        public double Calculate()
        {
            float localFieldCount = (_parentClass.MemberDeclarartions.Count - _parentClass.NumberOfMethods) * 1.0f;

            IList<string> variablesAccessed = new List<string>();

            foreach(var invocation in _totalFieldAccessInvocations)
            {
                if(!string.IsNullOrEmpty(invocation.AccessedField) && !variablesAccessed.Contains(invocation.AccessedField))
                {
                    variablesAccessed.Add(invocation.AccessedField);
                }
            }


            double result = variablesAccessed.Count > 0 ?  localFieldCount / _totalFieldAccessInvocations.Count : 0;

            return Math.Round(result, 2);
        }
    }
}
