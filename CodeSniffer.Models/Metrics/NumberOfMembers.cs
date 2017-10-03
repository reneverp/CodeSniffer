using System;
using CodeSniffer.Interfaces;
using System.Collections.Generic;

namespace CodeSniffer.Models.Metrics
{
    public class NumberOfMembers : IMetric
    {
        private IList<string> _members;

        public string Name => "Number Of Members";

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
