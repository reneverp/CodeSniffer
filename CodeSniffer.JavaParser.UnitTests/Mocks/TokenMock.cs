using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Parser.Java.UnitTests.Mocks
{
    public class TokenMock : IToken
    {
        public string Text { get { return ""; } }

        public int Type { get { return 1; } }

        public int Line { get { return 1; } }

        public int Column { get { return 1; } }

        public int Channel => throw new NotImplementedException();

        public int TokenIndex => throw new NotImplementedException();

        public int StartIndex => 1;

        public int StopIndex => 1;

        public ITokenSource TokenSource => throw new NotImplementedException();

        public ICharStream InputStream { get { return new CharStreamMock(); } }
    }
}
