using CodeSniffer.Interfaces;
using CodeSniffer.Models.Metrics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeSniffer.Models
{
    public class Class : ICodeFragment
    {
        public IList<Method> Methods { get; private set; }
        public IList<Class> Classes { get; private set; }

        public string Name { get; private set; }

        public string Content { get; private set; }

        public IList<ICodeFragment> Children => Methods.Cast<ICodeFragment>().ToList();

        public IList<IMetric> Metrics { get; private set; }

        public IList<string> MemberDeclarartions { get; private set; }


        private Object lockObj = new Object();


        public int NumberOfMethods
        {
            get
            {
                return Methods.Count;
            }
        }

        public Class(string name, string text)
        {
            Name = name;
            Content = text;
            Methods = new List<Method>();
            Classes = new List<Class>();
            MemberDeclarartions = new List<string>();

            Metrics = new List<IMetric>();
            Metrics.Add(new LinesOfCode(Content));
            Metrics.Add(new NumberOfMembers(MemberDeclarartions));
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
            lock (lockObj)
            {
                Classes.Add(classToAdd);
            }
        }

        public void AddMemberDecleration(string member)
        {
            lock (lockObj)
            {
                MemberDeclarartions.Add(member);
            }
        }
    }
}
