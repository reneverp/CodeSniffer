using CodeSniffer.Interfaces;
using CodeSniffer.Models.CodeSmells;
using CodeSniffer.Models.Metrics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System;
using CodeSniffer.Models.Utils;

namespace CodeSniffer.Models
{
    public class Method : ICodeFragment
    {
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
        public List<MethodInvocation> DataAccessInvocations { get; private set; }
        public List<MethodInvocation> ForeignDataAccessInvocations { get; private set; }
        public List<MethodInvocation> InnerDataAccessInvocations { get; private set; }

        public IList<string> Parameters { get; private set; }
        public IList<string> LocalFields { get; private set; }

        private Class ParentClass { get; set; }

        private string _filename;
        private string _additionalCasesFilename;

        public Method(Class parent, string name, string text)
        {
            Name = name;
            Content = LineEndingConverter.ConvertToCRLF(text);
            MethodInvocations      = new List<MethodInvocation>();
            InnerMethodInvocations = new List<MethodInvocation>();
            OuterMethodInvocations = new List<MethodInvocation>();
            DataAccessInvocations  = new List<MethodInvocation>();
            ForeignDataAccessInvocations = new List<MethodInvocation>();
            InnerDataAccessInvocations = new List<MethodInvocation>();


            Parameters = new List<string>();
            LocalFields = new List<string>();

            Metrics = new List<IMetric>();
            Metrics.Add(new LinesOfCode(Content));
            Metrics.Add(new CyclometicComplexity(Content));
            Metrics.Add(new NumberOfParameters(Parameters));
            Metrics.Add(new NumberOfInnerMethodInvocations(InnerMethodInvocations));
            Metrics.Add(new NumberOfOuterMethodInvocations(OuterMethodInvocations));
            Metrics.Add(new ATFD(ForeignDataAccessInvocations));
            Metrics.Add(new FDP(ForeignDataAccessInvocations));
            Metrics.Add(new LAA(parent, DataAccessInvocations));
            Metrics.Add(new MAXNESTING(Content));
            Metrics.Add(new NOAV(parent, Parameters, LocalFields, DataAccessInvocations, Content));

            CodeSmells = new List<ICodeSmell>();
            CodeSmells.Add(new FeatureEnvy(Metrics[5], Metrics[7], Metrics[6]));
            CodeSmells.Add(new LongMethod(Metrics[0], Metrics[1], Metrics[8], Metrics[9]));

            _filename = "MethodTrainingSet" + System.DateTime.Now.ToString("_Hmm_ddMMyyyy") + ".csv";
            _additionalCasesFilename = "AdditionalMethodData.csv";

            ParentClass = parent;

            foreach(var codesmell in CodeSmells)
            {
                codesmell.Updated += OnCodeSmellUpdated;
            }

        }

        private void OnCodeSmellUpdated()
        {
            WriteToTrainingSet(_additionalCasesFilename);
        }

        public void FindRelatedClassForOutboundInvocation(List<Class> totalClassOverView)
        {
            OuterMethodInvocations.ToList().ForEach(x => x.DeclaredClass = (
                                                           totalClassOverView.FirstOrDefault(y => y.Methods.Any(z => z.Name == x.Content))?.Name
                                                           ));
        }

        public void AddMethodInvocation(MethodInvocation invocation)
        {
            MethodInvocations.Add(invocation);
        }

        public void AddLocalField(string localFieldName)
        {
            LocalFields.Add(localFieldName);
        }

        public void ExtractInnerAndOuterMethodInvocations()
        {
            ForeignDataAccessInvocations.Clear();
            InnerDataAccessInvocations.Clear();

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
            InnerDataAccessInvocations  .AddRange(InnerMethodInvocations.Where(x => !string.IsNullOrEmpty(x.AccessedField)).ToList());


            DataAccessInvocations.AddRange(ForeignDataAccessInvocations);
            DataAccessInvocations.AddRange(InnerDataAccessInvocations);

        }

        public void AddParameter(string param)
        {
            Parameters.Add(param);
        }

        public void WriteToTrainingSet()
        {
            WriteToTrainingSet(_filename);
        }

        public void WriteToTrainingSet(string file)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder headers = new StringBuilder();

            headers.Append("Name,");
            sb.Append(Name + ",");

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
                sb.Append("," + codeSmell.Confidence.ToString());
                headers.Append(codeSmell.Name);
                headers.Append("," + codeSmell.Name + "Score");

                if (i < CodeSmells.Count - 1)
                {
                    sb.Append(",");
                    headers.Append(",");
                }
            }

            if (!File.Exists(file))
            {
                WriteLine(headers.ToString(), file);
            }

            WriteLine(sb.ToString(), file);
        }

        private void WriteLine(string line, string file)
        {
            using (StreamWriter writer = new StreamWriter(File.Open(file, FileMode.Append, FileAccess.Write)))
            {
                writer.WriteLine(line);
            }
        }
    }
}
