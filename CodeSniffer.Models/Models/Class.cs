using CodeSniffer.Interfaces;
using CodeSniffer.Models.CodeSmells;
using CodeSniffer.Models.Metrics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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

        public IList<ICodeSmell> CodeSmells { get; private set; }

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

            CodeSmells = new List<ICodeSmell>();
            CodeSmells.Add(new LargeClass());
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

        public void WriteToTrainingSet()
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder headers = new StringBuilder();

            for (int i = 0; i < Metrics.Count; i++)
            {
                var metric = Metrics[i];
                sb.Append(metric.Calculate());
                headers.Append(metric.Name);

                sb.Append(",");
                headers.Append(",");
            }

            for (int i = 0; i < CodeSmells.Count; i++)
            {
                var codeSmell = CodeSmells[i];
                sb.Append(codeSmell.IsDetected.ToString());
                headers.Append(codeSmell.Name);

                if (i < CodeSmells.Count - 1)
                {
                    sb.Append(",");
                    headers.Append(",");
                }
            }

            if (!File.Exists("ClassTrainingSet.csv"))
            {
                WriteLine(headers.ToString());
            }

            WriteLine(sb.ToString());
        }

        private static void WriteLine(string line)
        {
            using (StreamWriter writer = new StreamWriter(File.Open("ClassTrainingSet.csv", FileMode.Append, FileAccess.Write)))
            {
                writer.WriteLine(line);
            }
        }
    }
}
