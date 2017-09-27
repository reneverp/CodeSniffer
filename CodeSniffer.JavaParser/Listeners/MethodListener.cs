using CodeSniffer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using NLog;

namespace CodeSniffer.Listeners
{
    class MethodListener : JavaBaseListener
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

            int numberOfParams = 0;

            if (parameters != null)
                numberOfParams = parameters.Count();

            Method methodModel = new Method(numberOfParams, inputStream.GetText(interval));
            if (_currentClass != null)
                _currentClass.AddMethod(methodModel);

            _statementListener.setCurrentMethod(methodModel);
        }

        public override void ExitMethodDeclaration([NotNull] JavaParser.MethodDeclarationContext context)
        {
            _statementListener.resetCurrentMethod();
        }
    }
}
