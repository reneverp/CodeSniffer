using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Models
{
    class Project
    {
        public IList<Class> Classes { get; private set; }

        private Object lockObj = new Object();

        public Project()
        {
            Classes = new List<Class>();
        }

        public void AddClass(Class classToAdd)
        {
            lock (lockObj)
            {
                Classes.Add(classToAdd);
            }
        }
    }
}
