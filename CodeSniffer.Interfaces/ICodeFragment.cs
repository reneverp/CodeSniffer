using System.Collections.Generic;

namespace CodeSniffer.Interfaces
{
    public interface ICodeFragment
    {
        string Content { get; }
        string Name { get; }

        IList<ICodeFragment> Children { get; }
        IList<IMetric> Metrics { get; }
        IList<ICodeSmell> CodeSmells { get; }

        //TODO: move implementations to separate class
        void WriteToTrainingSet();
    }
}
