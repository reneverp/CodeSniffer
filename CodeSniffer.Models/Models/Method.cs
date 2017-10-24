using CodeSniffer.Interfaces;
using CodeSniffer.Models.CodeSmells;
using CodeSniffer.Models.Metrics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
                return Statements.Cast<ICodeFragment>().ToList();
            }
            set
            {
                //not impl. setter is needed for json Serialization / Deserialization :-(
            }
        }

        public IList<IMetric> Metrics { get; set; }

        public IList<ICodeSmell> CodeSmells { get; set; }

        public IList<Statement> Statements { get; private set; }

        public IList<string> Parameters { get; private set; }

        private string _filename;

        public Method(string name, string text)
        {
            Name = name;
            Content = text;
            Statements = new List<Statement>();
            Parameters = new List<string>();

            Metrics = new List<IMetric>();
            Metrics.Add(new LinesOfCode(Content));
            Metrics.Add(new CyclometicComplexity(Statements));
            Metrics.Add(new NumberOfParameters(Parameters));

            CodeSmells = new List<ICodeSmell>();
            CodeSmells.Add(new FeatureEnvy());
            CodeSmells.Add(new LongMethod());

            _filename = "MethodTrainingSet" + System.DateTime.Now.ToString("_Hmm_ddMMyyyy") + ".csv";
        }

        public void AddStatement(Statement statement)
        {
            Statements.Add(statement);
        }

        public void AddParameter(string param)
        {
            Parameters.Add(param);
        }

        public void WriteToTrainingSet()
        {
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
