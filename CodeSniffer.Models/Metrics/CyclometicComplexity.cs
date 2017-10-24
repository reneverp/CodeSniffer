using CodeSniffer.Interfaces;
using System.Collections.Generic;
using System;

namespace CodeSniffer.Models.Metrics
{
    class CyclometicComplexity : IMetric
    {
        private IList<Statement> _statements;

        public string Name
        {
            get { return "Cyclometic Complexity"; }
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

        public CyclometicComplexity(IList<Statement> statements)
        {
            _statements = statements;
        }

        public double Calculate()
        {
            //measure the complexity: https://www.leepoint.net/principles_and_practices/complexity/complexity-java-method.html
            //Cyclometic complexity starts with 1;
            //for every conditional, 1 is added.
            int complexity = 1;

            foreach (var statement in _statements)
            {
                var text = statement.Content;

                if (text.StartsWith("if") ||
                    text.StartsWith("else") ||
                    text.StartsWith("for") ||
                    text.StartsWith("foreach") ||
                    text.StartsWith("while") ||
                    text.StartsWith("do") ||
                    text.StartsWith("catch") ||
                    text.StartsWith("switch") ||
                    text.StartsWith("case"))
                {
                    complexity++;
                }
            }


            return complexity;
        }
    }
}
