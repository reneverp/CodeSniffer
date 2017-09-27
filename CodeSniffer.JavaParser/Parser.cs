using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CodeSniffer.Interfaces;
using CodeSniffer.Listeners;
using CodeSniffer.Models;

namespace CodeSniffer
{
    public class Parser : IParser
    {
        public void Parse(string file, Project project)
        {
            AntlrFileStream filestream = new AntlrFileStream(file);

            JavaLexer lexer = new JavaLexer(filestream);
            CommonTokenStream tokenStream = new CommonTokenStream(lexer);

            JavaParser parser = new JavaParser(tokenStream);

            var startingPonit = parser.compilationUnit();

            GenericListener genericListener = new GenericListener(project);

            ParseTreeWalker walker = new ParseTreeWalker();

            walker.Walk(genericListener, startingPonit);
        }
    }
}
