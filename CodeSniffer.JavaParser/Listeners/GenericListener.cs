using CodeSniffer.Models;
using Antlr4.Runtime.Misc;
using System;
using CodeSniffer.Interfaces;

namespace CodeSniffer.Listeners
{
    public class GenericListener : BaseListener
    {
        private StatementListener _statementListener;
        private MethodListener _methodListener;
        private ClassListener _classListener;
        private CompilationUnitListener _compilationUnitListener;

        public GenericListener(Project project)
        {
            _statementListener = new StatementListener();
            _methodListener = new MethodListener(_statementListener);
            _classListener = new ClassListener(_methodListener);
            _compilationUnitListener = new CompilationUnitListener(project, _classListener);

            _classListener.ParseInfoUpdate += (string info) => InvokeParseInfoUpdate(info);
        }

        public override void EnterCompilationUnit([NotNull] JavaParser.CompilationUnitContext context)
        {
            _compilationUnitListener.EnterCompilationUnit(context);
        }

        public override void ExitCompilationUnit([NotNull] JavaParser.CompilationUnitContext context)
        {
            _compilationUnitListener.ExitCompilationUnit(context);
        }

        public override void EnterClassDeclaration([NotNull] JavaParser.ClassDeclarationContext context)
        {
            _classListener.EnterClassDeclaration(context);
        }

        public override void ExitClassDeclaration([NotNull] JavaParser.ClassDeclarationContext context)
        {
            _classListener.ExitClassDeclaration(context);
        }

        public override void EnterMethodDeclaration([NotNull] JavaParser.MethodDeclarationContext context)
        {
            _methodListener.EnterMethodDeclaration(context);
        }

        public override void ExitMethodDeclaration([NotNull] JavaParser.MethodDeclarationContext context)
        {
            _methodListener.ExitMethodDeclaration(context);
        }

        public override void EnterStatement([NotNull] JavaParser.StatementContext context)
        {
            _statementListener.EnterStatement(context);
        }

        public override void ExitStatement([NotNull] JavaParser.StatementContext context)
        {
            _statementListener.ExitStatement(context);
        }
    }
}
