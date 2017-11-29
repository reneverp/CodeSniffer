using CodeSniffer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Models
{
    public class CompilationUnit : ICompilationUnit
    {
        public IList<ICodeFragment> Classes { get; private set; }

        private Object _lockObj = new Object();

        public CompilationUnit(string text)
        {
            Classes = new List<ICodeFragment>();
        }

        public void AddClass(ICodeFragment classToAdd)
        {
            lock (_lockObj)
            {
                Classes.Add(classToAdd);
            }
        }

        public static void FindClassRelations(IList<ICompilationUnit> compilationUnits)
        {
            List<Class> totalClassOverView = new List<Class>();

            compilationUnits.ToList().ForEach(x => totalClassOverView.AddRange(x.Classes.Cast<Class>()));

            totalClassOverView.ForEach(x => x.FindClassRelations(totalClassOverView));
        }
    }
}
