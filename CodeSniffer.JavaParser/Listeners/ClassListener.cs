using System;
using System.Linq;
using Antlr4.Runtime.Misc;
using CodeSniffer.Models;
using NLog;

namespace CodeSniffer.Listeners
{
    class ClassListener : JavaBaseListener
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private CompilationUnit _currentComilationUnit;
        private MethodListener _methodListener;

        public ClassListener(MethodListener methodListener)
        {
            _methodListener = methodListener;
        }

        public void setCurrentCompilationUnit(CompilationUnit currentCompilationUnit)
        {
            _currentComilationUnit = currentCompilationUnit;
        }

        public void resetCurrentComilationUnit()
        {
            _currentComilationUnit = null;
        }

        public override void EnterClassDeclaration([NotNull] JavaParser.ClassDeclarationContext context)
        {
            Logger.Debug("parsing class");

            var inputStream = context.Start.InputStream;

            var interval = new Interval(context.Start.StartIndex, context.Stop.StopIndex);

            Class classModel = new Class(context.Identifier()?.GetText(), inputStream.GetText(interval));
            _currentComilationUnit.AddClass(classModel);

            _methodListener.setCurrentClass(classModel);
        }

        public override void ExitClassDeclaration([NotNull] JavaParser.ClassDeclarationContext context)
        {
            _methodListener.resetCurrentClass();
        }
    }
}
