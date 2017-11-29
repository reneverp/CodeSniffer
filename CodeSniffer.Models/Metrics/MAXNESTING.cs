using CodeSniffer.Interfaces;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

namespace CodeSniffer.Models.Metrics
{
    class MAXNESTING : IMetric
    {
        private string _block;

        public string Name
        {
            get { return "MAXNESTING"; }
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

        public MAXNESTING(string block)
        {
            _block = block;
        }

        public double Calculate()
        {
            // every block (opening curly brace { ) without a closing brace is considered one level deeper.
            var lines = _block.Split('\n');

            int nesting = 0;
            int maxNesting = 0;

            foreach(var line in lines)
            {
                if (line.Contains("{"))
                {
                    nesting++;
                }

                if (line.Contains("}"))
                {
                    if(nesting > maxNesting)
                    {
                        maxNesting = nesting;
                    }
                    nesting = 0;
                }
            }

            return maxNesting;
        }

        private static int CountOccurrences(string line, string keyword)
        {
            return Regex.Matches(line, keyword).Count;
        }
    }
}
