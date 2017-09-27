using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Models
{
    public class Method
    {
        public double LinesOfCode { get; private set; }

        public int Complexity { get; set; }

        public int NumberOfStatements { get; set; }

        public int NumberOfParams { get; set; }

        public string Text { get; private set; }

        public IList<Statement> Statements { get; private set; }


        public Method(int numberOfParameters, string text)
        {
            LinesOfCode = Metrics.LinesOfCode.Calculate(text);
            Text = text;
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
