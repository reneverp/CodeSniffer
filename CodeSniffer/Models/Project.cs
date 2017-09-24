using System;
using System.Collections.Generic;
using System.Linq;

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

        public void Sort()
        {
            Classes = Classes.OrderByDescending(x => x.Complexity).ToList();
            foreach(var cl in Classes)
            {
                cl.Sort();
            }
        }
    }
}
