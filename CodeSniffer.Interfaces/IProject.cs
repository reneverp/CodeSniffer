using System.Collections.Generic;

namespace CodeSniffer.Interfaces
{
    public interface IProject
    {
        IList<ICompilationUnit> CompilationUnits { get; }

        void AddComilationUnit(ICompilationUnit compilationUnitToAdd);
        int GetClassCount();
        int GetCompilationUnitsCount();
        void FindClassRelations();

    }
}
