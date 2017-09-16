using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using System.IO;
using System.Threading;

namespace CodeSniffer.Listeners
{
    class JavaClassListener : JavaBaseListener
    {
        private string _logfileName;

        public JavaClassListener()
        {
            _logfileName = "test" + Thread.CurrentThread.ManagedThreadId + ".log";
        }

        public override void EnterFormalParameterList([NotNull] JavaParser.FormalParameterListContext context)
        {
            var list = context.formalParameter();

            using (var stream = File.Open(_logfileName, FileMode.Append))
            {
                StreamWriter writer = new StreamWriter(stream);

                var inputStream = context.Start.InputStream;

                var interval = new Interval(context.Start.StartIndex, context.Stop.StopIndex);

                writer.WriteLine(inputStream.GetText(interval));
                writer.WriteLine("Number of Parameters: " + list.Count());
                writer.Flush();
            }
        }

        public override void EnterStatementExpression([NotNull] JavaParser.StatementExpressionContext context)
        { 
            Console.WriteLine("expr");

            if(context.children.Count()  == 0)
            {
                return;
            }

            using (var stream = File.Open(_logfileName, FileMode.Append))
            {              
                StreamWriter writer = new StreamWriter(stream);

                writer.Write("EXCPLICIT INVCATION SUFFIX: ");


                var inputStream = context.Start.InputStream;

                var interval = new Interval(context.Start.StartIndex, context.Stop.StopIndex);

                writer.WriteLine(inputStream.GetText(interval));
                writer.Flush();
            }
        }

        public override void EnterMethodDeclaration([NotNull] JavaParser.MethodDeclarationContext context)
        {
            using (var stream = File.Open(_logfileName, FileMode.Append))
            {
                StreamWriter writer = new StreamWriter(stream);

                var inputStream = context.Start.InputStream;

                var interval = new Interval(context.Start.StartIndex, context.Stop.StopIndex);

                writer.WriteLine(inputStream.GetText(interval));
                writer.Flush();
            }
        }
    }
}
