using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Models
{
    public class CompilationUnit
    {
        public IList<Class> Classes { get; private set; }

        private Object _lockObj = new Object();

        public CompilationUnit(string text)
        {
            Classes = new List<Class>();
        }

        public void AddClass(Class classToAdd)
        {
            lock (_lockObj)
            {
                Classes.Add(classToAdd);
            }
        }
    }
}
