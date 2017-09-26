using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeSniffer.Models
{
    class Project
    {
        public IList<CompilationUnit> CompilationUnits { get; private set; }

        private Object lockObj = new Object();

        public Project()
        {
            CompilationUnits = new List<CompilationUnit>();
        }

        public void AddComilationUnit(CompilationUnit classToAdd)
        {
            lock (lockObj)
            {
                CompilationUnits.Add(classToAdd);
            }
        }

        public void Sort()
        {
            //TODO;
        }
    }
}
