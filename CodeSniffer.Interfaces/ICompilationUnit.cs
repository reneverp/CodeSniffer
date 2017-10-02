using System.Collections.Generic;

namespace CodeSniffer.Interfaces
{
    public interface ICompilationUnit
    {
        IList<ICodeFragment> Classes { get; }

        void AddClass(ICodeFragment classToAdd);
    }
}
