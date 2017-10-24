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

        public IList<IMetric> Metrics { get; set; }

        public IList<ICodeSmell> CodeSmells { get; set; }


        public IList<ICodeFragment> Children { get; set; }

        public Statement(string text)
        {
            Content = text;
        }

        public void WriteToTrainingSet()
        {
        }
    }
}
