using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace CodeSniffer
{
    class Program
    {
        static void Main(string[] args)
        {
            AntlrFileStream filestream = new AntlrFileStream(@"D:\svn\ganttproject-ganttproject-2.8.5\ganttproject-ganttproject-2.8.5\ganttproject\src\net\sourceforge\ganttproject\shape\PaintCellRenderer.java");

            JavaLexer lexer = new JavaLexer(filestream);
            CommonTokenStream tokenStream = new CommonTokenStream(lexer);

            JavaParser parser = new JavaParser(tokenStream);

            var bla = parser.compilationUnit();

            ParseTreeWalker walker = new ParseTreeWalker();

            MyListener listener = new MyListener();

            walker.Walk(listener, bla);
        }
    }
}
