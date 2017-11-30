using Antlr4.Runtime.Misc;
using CodeSniffer.Interfaces;

namespace CodeSniffer.Listeners
{
    public class OrchestratingListener : BaseListener
    {
        private StatementListener _statementListener;
        private MethodListener _methodListener;
        private MemberDeclarationListener _memberListener;
        private ClassListener _classListener;
        private CompilationUnitListener _compilationUnitListener;

        public OrchestratingListener(IProject project)
        {
            _statementListener = new StatementListener();
            _methodListener = new MethodListener(_statementListener);
            _memberListener = new MemberDeclarationListener();
            _classListener = new ClassListener(_methodListener, _memberListener);
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

        public override void EnterMemberDeclaration([NotNull] JavaParser.MemberDeclarationContext context)
        {
            _memberListener.EnterMemberDeclaration(context);
        }

        public override void ExitMemberDeclaration([NotNull] JavaParser.MemberDeclarationContext context)
        {
            _memberListener.ExitMemberDeclaration(context);
        }

        public override void EnterStatement([NotNull] JavaParser.StatementContext context)
        {
            _statementListener.EnterStatement(context);
        }

        public override void ExitStatement([NotNull] JavaParser.StatementContext context)
        {
            _statementListener.ExitStatement(context);
        }

        public override void EnterFieldDeclaration([NotNull] JavaParser.FieldDeclarationContext context)
        {
            _memberListener.EnterFieldDeclaration(context);
        }

        public override void ExitFieldDeclaration([NotNull] JavaParser.FieldDeclarationContext context)
        {
            _memberListener.ExitFieldDeclaration(context);
        }

        public override void EnterLocalVariableDeclaration([NotNull] JavaParser.LocalVariableDeclarationContext context)
        {
            _statementListener.EnterLocalVariableDeclaration(context);
        }

        public override void ExitLocalVariableDeclaration([NotNull] JavaParser.LocalVariableDeclarationContext context)
        {
            _statementListener.ExitLocalVariableDeclaration(context);
        }
    }
}
