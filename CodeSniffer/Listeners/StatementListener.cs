using Antlr4.Runtime.Misc;
using CodeSniffer.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Listeners
{
    class StatementListener : JavaBaseListener
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
            
            //measure the complexity: https://www.leepoint.net/principles_and_practices/complexity/complexity-java-method.html

            //TODO: remove this complexity measurement to a metric class
            if (_currentMethod != null)
            {
                _currentMethod.NumberOfStatements++;

                var inputStream = context.Start.InputStream;

                var interval = new Interval(context.Start.StartIndex, context.Stop.StopIndex);

                var text = context.GetText();
                var startText = text.Substring(0, Math.Min(text.Length - 1, 10)).ToLower();

                if (startText.StartsWith("if") ||
                    startText.StartsWith("else") ||
                    startText.StartsWith("for") ||
                    startText.StartsWith("foreach") ||
                    startText.StartsWith("while") ||
                    startText.StartsWith("do") ||
                    startText.StartsWith("catch") ||
                    startText.StartsWith("switch") ||
                    startText.StartsWith("case"))
                {
                    _currentMethod.Complexity++;
                }
            }
        }
    }
}
