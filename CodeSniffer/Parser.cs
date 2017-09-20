using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CodeSniffer.Listeners;
using CodeSniffer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer
{
    class Parser
    {
        public void Parse(string file, Project project)
        {
            AntlrFileStream filestream = new AntlrFileStream(file);

            JavaLexer lexer = new JavaLexer(filestream);
            CommonTokenStream tokenStream = new CommonTokenStream(lexer);

            JavaParser parser = new JavaParser(tokenStream);

            var bla = parser.compilationUnit();

            ParseTreeWalker walker = new ParseTreeWalker();
            
            JavaClassListener listener = new JavaClassListener(project);

            walker.Walk(listener, bla);

        }
    }
}
