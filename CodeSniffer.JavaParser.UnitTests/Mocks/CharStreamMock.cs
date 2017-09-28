using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Parser.Java.UnitTests.Mocks
{
    public class CharStreamMock : ICharStream
    {
        public int Index => throw new NotImplementedException();

        public int Size => throw new NotImplementedException();

        public string SourceName => throw new NotImplementedException();

        public void Consume()
        {
            throw new NotImplementedException();
        }

        [return: NotNull]
        public string GetText(Interval interval)
        {
            return "test";
        }

        public int LA(int i)
        {
            throw new NotImplementedException();
        }

        public int Mark()
        {
            throw new NotImplementedException();
        }

        public void Release(int marker)
        {
            throw new NotImplementedException();
        }

        public void Seek(int index)
        {
            throw new NotImplementedException();
        }
    }
}
