using CodeSniffer.Interfaces;
using System.Collections.Generic;

namespace CodeSniffer.Models
{
    public class Method : ICodeFragment
    {
        public double LinesOfCode { get; private set; }

        public int Complexity { get; set; }

        public int NumberOfStatements { get; set; }

        public int NumberOfParams { get; set; }

        public string Name { get; private set; }

        public string Content { get; private set; }

        public IList<ICodeFragment> Children => null;

        public IList<Statement> Statements { get; private set; }


        public Method(string name, int numberOfParameters, string text)
        {
            LinesOfCode = Metrics.LinesOfCode.Calculate(text);
            Name = name;
            Content = text;
            NumberOfParams = numberOfParameters;
            Complexity = 1; // cyclomatic complexity always starts with 1 for method
            Statements = new List<Statement>();
        }

        public void AddStatement(Statement statement)
        {
            Statements.Add(statement);
        }
    }
}
