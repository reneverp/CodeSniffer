using CodeSniffer.Interfaces;
using CodeSniffer.Models.Metrics;
using System.Collections.Generic;
using System.Linq;

namespace CodeSniffer.Models
{
    public class Method : ICodeFragment
    {
        public string Name { get; private set; }

        public string Content { get; private set; }

        public IList<ICodeFragment> Children => Statements.Cast<ICodeFragment>().ToList();

        public IList<IMetric> Metrics { get; private set; }

        public IList<Statement> Statements { get; private set; }

        public IList<string> Parameters { get; private set; }

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
        }

        public void AddStatement(Statement statement)
        {
            Statements.Add(statement);
        }

        public void AddParameter(string param)
        {
            Parameters.Add(param);
        }
    }
}
