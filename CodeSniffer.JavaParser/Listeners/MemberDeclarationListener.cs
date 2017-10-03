using Antlr4.Runtime.Misc;
using CodeSniffer.Listeners;
using CodeSniffer.Models;
using NLog;

namespace CodeSniffer.Listeners
{
    public class MemberDeclarationListener : BaseListener
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private Class _currentClass;

        public void setCurrentClass(Class currentClass)
        {
            _currentClass = currentClass;
        }

        public void resetCurrentClass()
        {
            _currentClass = null;
        }

        public override void EnterMemberDeclaration([NotNull] JavaParser.MemberDeclarationContext context)
        {
            Logger.Debug("parsing member declaration");

            var inputStream = context.Start.InputStream;

            var interval = new Interval(context.Start.StartIndex, context.Stop.StopIndex);

            if (_currentClass != null)
                _currentClass.AddMemberDecleration(inputStream.GetText(interval));
        }
    }
}
