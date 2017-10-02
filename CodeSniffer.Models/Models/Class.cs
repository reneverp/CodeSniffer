using CodeSniffer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeSniffer.Models
{
    public class Class : ICodeFragment
    {
        public IList<Method> Methods { get; private set; }
        public IList<Class> Classes { get; private set; }

        public double LinesOfCode { get; private set; }

        public string Name { get; private set; }

        public string Content { get; private set; }

        public IList<ICodeFragment> Children => Methods.Cast<ICodeFragment>().ToList();

        private Object lockObj = new Object();


        public int NumberOfMethods
        {
            get
            {
                return Methods.Count;
            }
        }

        public int Complexity
        {
            get
            {
                int totalComplexity = 0;
                foreach(var met in Methods)
                {
                    totalComplexity += met.Complexity;
                }

                return totalComplexity;
            }
        }

        public Class(string name, string text)
        {
            Name = name;
            LinesOfCode = Metrics.LinesOfCode.Calculate(text);
            Content = text;
            Methods = new List<Method>();
            Classes = new List<Class>();
        }

        public void AddMethod(Method method)
        {
            lock (lockObj)
            {
                Methods.Add(method);
            }
        }

        public void AddClass(Class classToAdd)
        {
            Classes.Add(classToAdd);
        }
    }
}
