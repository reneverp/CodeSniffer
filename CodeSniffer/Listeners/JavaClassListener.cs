using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using System.IO;
using System.Threading;
using CodeSniffer.Models;

namespace CodeSniffer.Listeners
{
    class JavaClassListener : JavaBaseListener
    {
        private string _logfileName;

        private static int id = 0;

        private Project _project;

        private Class currentClass;

        public JavaClassListener(Project project)
        {
            _logfileName = "test" + id++ + ".log";
            _project = project;
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

            //if (currentClass != null)
                //currentClass.
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

                var parameters = context.formalParameters()?.formalParameterList()?.formalParameter();

                int numberOfParams = 0;

                if (parameters != null)
                    numberOfParams = parameters.Count();

                Method methodModel = new Method(inputStream.GetText(interval).Split('\n').Count(), 1, 1, numberOfParams, inputStream.GetText(interval));
                if (currentClass != null)
                    currentClass.AddMethod(methodModel);
            }
        }

        public override void EnterClassDeclaration([NotNull] JavaParser.ClassDeclarationContext context)
        {
            using (var stream = File.Open(_logfileName, FileMode.Append))
            {
                StreamWriter writer = new StreamWriter(stream);

                var inputStream = context.Start.InputStream;

                var interval = new Interval(context.Start.StartIndex, context.Stop.StopIndex);

                writer.WriteLine(inputStream.GetText(interval));
                writer.Flush();

                Class classModel = new Class(inputStream.GetText(interval).Split('\n').Count(), inputStream.GetText(interval));
                _project.AddClass(classModel);

                currentClass = classModel;
            }
        }

        public override void ExitClassDeclaration([NotNull] JavaParser.ClassDeclarationContext context)
        {
            currentClass = null;
        }
    }
}
