using CodeSniffer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeSniffer.Models
{
    public class Project : IProject
    {
        public IList<ICompilationUnit> CompilationUnits { get; private set; }

        private Object lockObj = new Object();

        public Project()
        {
            CompilationUnits = new List<ICompilationUnit>();
        }

        public void AddComilationUnit(ICompilationUnit compilationUnitToAdd)
        {
            lock (lockObj)
            {
                CompilationUnits.Add(compilationUnitToAdd);
            }
        }

        public int GetClassCount()
        {
            int numberOfClasses = 0;

            foreach(var compilationUnit in CompilationUnits)
            {
                numberOfClasses += compilationUnit.Classes.Count;
            }

            return numberOfClasses;
        }

        public int GetCompilationUnitsCount()
        {
            return CompilationUnits.Count;
        }

        public void FindClassRelations()
        {
            CompilationUnit.FindClassRelations(CompilationUnits);
        }
    }
}
