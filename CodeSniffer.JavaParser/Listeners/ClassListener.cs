using System;
using Antlr4.Runtime.Misc;
using CodeSniffer.Interfaces;
using CodeSniffer.Models;
using NLog;

namespace CodeSniffer.Listeners
{
    public class ClassListener : BaseListener
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private CompilationUnit _currentComilationUnit;
        private MethodListener _methodListener;
        private MemberDeclarationListener _memberListener;

        public ClassListener(MethodListener methodListener, MemberDeclarationListener memberListener)
        {
            _methodListener = methodListener;
            _memberListener = memberListener;
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
            if (_currentComilationUnit != null)
                _currentComilationUnit.AddClass(classModel);

            _methodListener.setCurrentClass(classModel);
            _memberListener.setCurrentClass(classModel);

            InvokeParseInfoUpdate("Finished parsing class: " + classModel.Name);
        }

        public override void ExitClassDeclaration([NotNull] JavaParser.ClassDeclarationContext context)
        {
            _methodListener.resetCurrentClass();
            _memberListener.resetCurrentClass();
        }
    }
}
