using CodeSniffer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Models.Metrics
{
    public class NumberOfInnerMethodInvocations : IMetric
    {
        public string Name
        {
            get { return "# Inner Invocations"; }
            set { }
        }

        private double _value = -1;
        private IList<MethodInvocation> _list;

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

        public NumberOfInnerMethodInvocations(IList<MethodInvocation> listOfInnerMethodInvocations)
        {
            _list = listOfInnerMethodInvocations;
        }

        public double Calculate()
        {
            return _list.Count;
        }
    }
}
