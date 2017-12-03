﻿using CodeSniffer.Interfaces;
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
        private bool _writtenToDataSet;

        public IList<Method> Methods { get; private set; }
        public IList<Class> Classes { get; private set; }

        public string Name { get; private set; }

        public string Content { get; private set; }

        public IList<ICodeFragment> Children
        {
            get
            {
                return Methods.Cast<ICodeFragment>().ToList();
            }
            set
            {
                //not impl. setter is needed for json Serialization / Deserialization :-(
            }
        }

        public IList<IMetric> Metrics { get; set; }

        public IList<ICodeSmell> CodeSmells { get; set; }

        public IList<string> MemberDeclarartions { get; private set; }
        public IList<string> InstanceVariables { get; private set; }


        private Object lockObj = new Object();


        public int NumberOfMethods
        {
            get
            {
                return Methods.Count;
            }
        }

        public void AddInstanceVariable(string text)
        {
            InstanceVariables.Add(text);
        }

        private string _filename;

        public Class(string name, string text)
        {
            Name = name;
            Content = text;
            Methods = new List<Method>();
            Classes = new List<Class>();
            MemberDeclarartions = new List<string>();
            InstanceVariables = new List<string>();

            Metrics = new List<IMetric>();
            Metrics.Add(new LinesOfCode(Content));
            Metrics.Add(new NumberOfMembers(MemberDeclarartions));
            Metrics.Add(new TCC(Methods));
            Metrics.Add(new WMC(Methods));
            Metrics.Add(new ATFD(Methods));

            CodeSmells = new List<ICodeSmell>();
            CodeSmells.Add(new LargeClass(Metrics[2], Metrics[3], Metrics[4], Metrics[0])); //TODO: REFACTOR THIS TEMP SOLUTION

            _filename = "ClassTrainingSet" + System.DateTime.Now.ToString("_Hmm_ddMMyyyy") + ".csv";
            _writtenToDataSet = false;
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

        public void FindClassRelations(List<Class> totalClassOverView)
        {
            Methods.ToList().ForEach(x => x.FindRelatedClassForOutboundInvocation(totalClassOverView));
        }


        public void WriteToTrainingSet()
        {
            if(_writtenToDataSet)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            StringBuilder headers = new StringBuilder();

            for (int i = 0; i < Metrics.Count; i++)
            {
                var metric = Metrics[i];
                sb.Append(metric.Value);
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

            if (!File.Exists(_filename))
            {
                WriteLine(headers.ToString());
            }

            WriteLine(sb.ToString());

            _writtenToDataSet = true;
        }

        private void WriteLine(string line)
        {
            using (StreamWriter writer = new StreamWriter(File.Open(_filename, FileMode.Append, FileAccess.Write)))
            {
                writer.WriteLine(line);
            }
        }
    }
}
