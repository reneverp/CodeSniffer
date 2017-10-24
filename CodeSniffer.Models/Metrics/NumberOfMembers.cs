using System;
using CodeSniffer.Interfaces;
using System.Collections.Generic;

namespace CodeSniffer.Models.Metrics
{
    public class NumberOfMembers : IMetric
    {
        private IList<string> _members;

        public string Name
        {
            get { return "Number Of Members"; }
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

        public NumberOfMembers(IList<string> members)
        {
            _members = members;
        }

        public double Calculate()
        {
            return _members.Count;
        }
    }
}
