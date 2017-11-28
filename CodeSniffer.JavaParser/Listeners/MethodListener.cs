using CodeSniffer.Models;
using System.Linq;
using Antlr4.Runtime.Misc;
using NLog;

namespace CodeSniffer.Listeners
{
    public class MethodListener : BaseListener
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        
        private Class _currentClass;
        private StatementListener _statementListener;

        public MethodListener(StatementListener statementListener)
        {
            _statementListener = statementListener;
        }

        public void setCurrentClass(Class currentClass)
        {
            _currentClass = currentClass;
        }

        public void resetCurrentClass()
        {
            _currentClass = null;
        }

        public override void EnterMethodDeclaration([NotNull] JavaParser.MethodDeclarationContext context)
        {
            Logger.Debug("parsing method");

            var inputStream = context.Start.InputStream;

            var interval = new Interval(context.Start.StartIndex, context.Stop.StopIndex);

            var parameters = context.formalParameters()?.formalParameterList()?.formalParameter();

            Method methodModel = new Method(_currentClass, context.Identifier()?.GetText(), inputStream.GetText(interval));

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    methodModel.AddParameter(param.GetText());
                }
            }

            if (_currentClass != null)
                _currentClass.AddMethod(methodModel);

            _statementListener.setCurrentMethod(methodModel);

            InvokeParseInfoUpdate("Parsing method: " + methodModel.Name);
        }

        public override void ExitMethodDeclaration([NotNull] JavaParser.MethodDeclarationContext context)
        {
            _statementListener.resetCurrentMethod();
        }
    }
}
