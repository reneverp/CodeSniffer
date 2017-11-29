using Antlr4.Runtime.Misc;
using CodeSniffer.Models;
using NLog;
using System;
using System.Text.RegularExpressions;

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

                //find method invocation using Regex: not conditional, name followed by ( and )
                var methodInvocationMatches = Regex.Matches(text, @"(?!\bif\b|\bfor\b|\bwhile\b|\bswitch\b|\btry\b|\bcatch\b)(\b[\w]+\b)[\s\n\r]*(?=\(.*\))");

                if (methodInvocationMatches.Count > 0)
                {
                    foreach (Match methodInvocationMatch in methodInvocationMatches)
                    {
                        MethodInvocation methodInvocation = new MethodInvocation(methodInvocationMatch.Value);

                        _currentMethod.AddMethodInvocation(methodInvocation);

                        InvokeParseInfoUpdate("Parsing method invocation: " + methodInvocation.Content);
                    }
                }                
            }
        }
    }
}
