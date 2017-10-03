using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CodeSniffer.Interfaces;
using CodeSniffer.Listeners;
using CodeSniffer.Models;

namespace CodeSniffer
{
    public class Parser : IParser
    {
        public event Action<string> NotifyParseInfoUpdated;

        public void Parse(string file, IProject project)
        {
            AntlrFileStream filestream = new AntlrFileStream(file);

            JavaLexer lexer = new JavaLexer(filestream);
            CommonTokenStream tokenStream = new CommonTokenStream(lexer);

            JavaParser parser = new JavaParser(tokenStream);

            var startingPonit = parser.compilationUnit();

            OrchestratingListener genericListener = new OrchestratingListener(project);

            genericListener.ParseInfoUpdate += (string info) => NotifyParseInfoUpdated?.Invoke(info);

            ParseTreeWalker walker = new ParseTreeWalker();

            walker.Walk(genericListener, startingPonit);
        }
    }
}
