using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Models
{
    class Method
    {
        public int LinesOfCode { get; private set; }

        public int Complexity { get; private set; }

        public int NumberOfStatements { get; private set; }

        public string Text { get; private set; }

        public Method(int linesOfCode, int complexity, int numberOfStatements, string text)
        {
            LinesOfCode = linesOfCode;
            Complexity = complexity;
            NumberOfStatements = numberOfStatements;
            Text = text;
        }
    }
}
