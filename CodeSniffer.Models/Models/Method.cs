using CodeSniffer.Interfaces;
using CodeSniffer.Models.CodeSmells;
using CodeSniffer.Models.Metrics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeSniffer.Models
{
    public class Method : ICodeFragment
    {
        private bool _writtenToDataSet;

        public string Name { get; private set; }

        public string Content { get; private set; }

        public IList<ICodeFragment> Children
        {
            get
            {
                return null;
                //return MethodInvocations.Cast<ICodeFragment>().ToList();
            }
            set
            {
                //not impl. setter is needed for json Serialization / Deserialization :-(
            }
        }

        public IList<IMetric> Metrics { get; set; }

        public IList<ICodeSmell> CodeSmells { get; set; }

        public IList<MethodInvocation> MethodInvocations      { get; private set; }
        public IList<MethodInvocation> OuterMethodInvocations { get; private set; }
        public IList<MethodInvocation> InnerMethodInvocations { get; private set; }
        public List<MethodInvocation> ForeignDataAccessInvocations { get; private set; }


        public IList<string> Parameters { get; private set; }

        private Class ParentClass { get; set; }

        private string _filename;

        public Method(Class parent, string name, string text)
        {
            Name = name;
            Content = text;
            MethodInvocations      = new List<MethodInvocation>();
            InnerMethodInvocations = new List<MethodInvocation>();
            OuterMethodInvocations = new List<MethodInvocation>();
            ForeignDataAccessInvocations = new List<MethodInvocation>();

            Parameters = new List<string>();

            Metrics = new List<IMetric>();
            Metrics.Add(new LinesOfCode(Content));
            Metrics.Add(new CyclometicComplexity(Content));
            Metrics.Add(new NumberOfParameters(Parameters));
            Metrics.Add(new NumberOfInnerMethodInvocations(InnerMethodInvocations));
            Metrics.Add(new NumberOfOuterMethodInvocations(OuterMethodInvocations));
            Metrics.Add(new ATFD(ForeignDataAccessInvocations));

            CodeSmells = new List<ICodeSmell>();
            CodeSmells.Add(new FeatureEnvy());
            CodeSmells.Add(new LongMethod());

            _filename = "MethodTrainingSet" + System.DateTime.Now.ToString("_Hmm_ddMMyyyy") + ".csv";
            _writtenToDataSet = false;

            ParentClass = parent;
        }

        public void AddMethodInvocation(MethodInvocation invocation)
        {
            MethodInvocations.Add(invocation);
        }

        public void ExtractInnerAndOuterMethodInvocations()
        {
            //Note: call this after the class has been fully parsed to avoid missing inner invocations.
            foreach (var invocation in MethodInvocations)
            {
                if (ParentClass != null && ParentClass.Methods.Any(x => x.Name.Equals(invocation.Content)))
                {                    
                    InnerMethodInvocations.Add(invocation);
                }
                else
                {
                    OuterMethodInvocations.Add(invocation);
                }
            }

            ForeignDataAccessInvocations.AddRange(OuterMethodInvocations.Where(x => !string.IsNullOrEmpty(x.AccessedField)).ToList());
        }

        public void AddParameter(string param)
        {
            Parameters.Add(param);
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
