using System;
using System.Collections.Generic;

namespace CodeSniffer.Interfaces
{
    public interface ICodeFragment
    {
        string Content { get; }
        string Name { get; }

        IList<ICodeFragment> Children { get; set; }
        IList<IMetric> Metrics { get; set; }
        IList<ICodeSmell> CodeSmells { get; set; }

        void WriteToTrainingSet(string filename, string dir = "");
    }
}
