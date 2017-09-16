using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer
{
    public class MyListener : JavaBaseListener
    {
        public override void EnterCompilationUnit(JavaParser.CompilationUnitContext context)
        {
            System.Console.WriteLine(context.GetText());
        }
    }
}
