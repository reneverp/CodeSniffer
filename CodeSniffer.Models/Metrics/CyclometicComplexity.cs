using CodeSniffer.Interfaces;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

namespace CodeSniffer.Models.Metrics
{
    class CyclometicComplexity : IMetric
    {
        private string _block;

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

        public CyclometicComplexity(string block)
        {
            _block = block;
        }

        public double Calculate()
        {
            //measure the complexity: http://metrics.sourceforge.net/
            //Cyclometic complexity starts with 1;
            //for every conditional, 1 is added.
            int complexity = 1;

            complexity += CountOccurrences(_block, @"if\s*?\(");
            complexity += CountOccurrences(_block, @"else(\s|\n)*?{");
            complexity += CountOccurrences(_block, @"for\s*?\(");
            complexity += CountOccurrences(_block, @"foreach\s*?\(");
            complexity += CountOccurrences(_block, @"while\s*?\(");
            complexity += CountOccurrences(_block, @"do(\s|\n)*?{");
            complexity += CountOccurrences(_block, @"try(\s|\n)*?{");
            complexity += CountOccurrences(_block, @"finally(\s|\n)*?{");
            complexity += CountOccurrences(_block, @"catch\s*?\(?");
            complexity += CountOccurrences(_block, @"default.*?:");
            complexity += CountOccurrences(_block, @"case.*?:");
            complexity += CountOccurrences(_block, @"continue\s*?;");
            complexity += CountOccurrences(_block, @"&&\s*?");
            complexity += CountOccurrences(_block, @"\|\|\s*?");
            complexity += CountOccurrences(_block, @"\?\s*?");
            complexity += CountOccurrences(_block, @"\?.*?:\s*?");

            var returnCount = CountOccurrences(_block, @"return.*?;"); //only multiple return statements count
            if(returnCount > 0)
            {
                complexity += returnCount - 1;
            }

            return complexity;
        }

        private static int CountOccurrences(string line, string keyword)
        {
            return Regex.Matches(line, keyword).Count;
        }
    }
}
