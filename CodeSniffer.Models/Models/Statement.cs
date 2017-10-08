using CodeSniffer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Models
{
    public class Statement : ICodeFragment
    {
        public string Content { get; private set; }

        public string Name => "statement";

        public IList<IMetric> Metrics => null;

        public IList<ICodeSmell> CodeSmells => null;


        public IList<ICodeFragment> Children => null;

        public Statement(string text)
        {
            Content = text;
        }
    }
}
