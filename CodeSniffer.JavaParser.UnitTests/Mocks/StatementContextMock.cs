using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JavaParser;

namespace CodeSniffer.Parser.Java.UnitTests.Mocks
{
    public class StatementContextMock : StatementContext
    {
        public StatementContextMock(ParserRuleContext parent, int invokingState) : base(parent, invokingState)
        {
        }

        public override IToken Start
        {
            get
            {
                return new TokenMock();
            }
        }

        public override IToken Stop
        {
            get
            {
                return new TokenMock();
            }
        }

        public override string GetText()
        {
            return "test";
        }
    }

    public class MethodContextMock : MethodDeclarationContext
    {
        public MethodContextMock(ParserRuleContext parent, int invokingState) : base(parent, invokingState)
        {
        }

        public override IToken Start
        {
            get
            {
                return new TokenMock();
            }
        }

        public override IToken Stop
        {
            get
            {
                return new TokenMock();
            }
        }

        public override string GetText()
        {
            return "test";
        }
    }

    public class ClassContextMock : ClassDeclarationContext
    {
        public ClassContextMock(ParserRuleContext parent, int invokingState) : base(parent, invokingState)
        {
        }

        public override IToken Start
        {
            get
            {
                return new TokenMock();
            }
        }

        public override IToken Stop
        {
            get
            {
                return new TokenMock();
            }
        }

        public override string GetText()
        {
            return "test";
        }
    }

    public class CompilationUnitContextMock : CompilationUnitContext
    {
        public CompilationUnitContextMock(ParserRuleContext parent, int invokingState) : base(parent, invokingState)
        {
        }

        public override IToken Start
        {
            get
            {
                return new TokenMock();
            }
        }

        public override IToken Stop
        {
            get
            {
                return new TokenMock();
            }
        }

        public override string GetText()
        {
            return "test";
        }
    }
}
