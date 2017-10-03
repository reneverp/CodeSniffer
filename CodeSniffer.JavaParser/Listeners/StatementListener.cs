using Antlr4.Runtime.Misc;
using CodeSniffer.Models;
using NLog;
using System;

namespace CodeSniffer.Listeners
{
    public class StatementListener : BaseListener
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        
        private Method _currentMethod;

        public void setCurrentMethod(Method currentMethod)
        {
            _currentMethod = currentMethod;
        }

        public void resetCurrentMethod()
        {
            _currentMethod = null;
        }

        public override void EnterStatement([NotNull] JavaParser.StatementContext context)
        {
            Logger.Debug("parsing statement");

            if (_currentMethod != null)
            {
                var inputStream = context.Start.InputStream;

                var interval = new Interval(context.Start.StartIndex, context.Stop.StopIndex);

                var text = context.GetText();

                Statement statement = new Statement(text);

                _currentMethod.AddStatement(statement);

                InvokeParseInfoUpdate("Parsing statement: " + statement.Content);
            }
        }

    }
}
