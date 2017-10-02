using Antlr4.Runtime.Misc;
using CodeSniffer.Interfaces;
using CodeSniffer.Models;
using NLog;

namespace CodeSniffer.Listeners
{
    public class CompilationUnitListener : BaseListener
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        
        private IProject _project;
        private ClassListener _classListener;

        public CompilationUnitListener(IProject project, ClassListener classListener)
        {
            _project = project;
            _classListener = classListener;
        }

        public override void EnterCompilationUnit([NotNull] JavaParser.CompilationUnitContext context)
        {
            Logger.Debug("parsing comppilationUnit");

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
