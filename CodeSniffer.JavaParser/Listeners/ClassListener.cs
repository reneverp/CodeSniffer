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
        private Class _currentClassModel;

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

            _currentClassModel = new Class(context.Identifier()?.GetText(), inputStream.GetText(interval));
            if (_currentComilationUnit != null)
                _currentComilationUnit.AddClass(_currentClassModel);

            _methodListener.setCurrentClass(_currentClassModel);
            _memberListener.setCurrentClass(_currentClassModel);

            InvokeParseInfoUpdate("Finished parsing class: " + _currentClassModel.Name);

        }

        public override void ExitClassDeclaration([NotNull] JavaParser.ClassDeclarationContext context)
        {
            if (_currentClassModel != null)
            {
                foreach (var method in _currentClassModel.Methods)
                {
                    method.ExtractInnerAndOuterMethodInvocations();
                }
            }

            _methodListener.resetCurrentClass();
            _memberListener.resetCurrentClass();

            _currentClassModel = null;
        }
    }
}
