using Antlr4.Runtime.Misc;
using CodeSniffer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Listeners
{
    class CompilationUnitListener : JavaBaseListener
    {
        private Project _project;
        private ClassListener _classListener;

        public CompilationUnitListener(Project project, ClassListener classListener)
        {
            _project = project;
            _classListener = classListener;
        }

        public override void EnterCompilationUnit([NotNull] JavaParser.CompilationUnitContext context)
        {
            Console.WriteLine("parsing comppilationUnit");

            var currentCompilationUnit = new CompilationUnit(context.GetText());

            _classListener.setCurrentCompilationUnit(currentCompilationUnit);
            _project.AddComilationUnit(currentCompilationUnit);
        }

        public override void ExitCompilationUnit([NotNull] JavaParser.CompilationUnitContext context)
        {
            _classListener.resetCurrentComilationUnit();
        }        
    }
}
