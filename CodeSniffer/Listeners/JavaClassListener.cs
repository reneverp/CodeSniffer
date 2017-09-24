using System;
using System.Linq;
using Antlr4.Runtime.Misc;
using System.IO;
using CodeSniffer.Models;
using System.Diagnostics;

namespace CodeSniffer.Listeners
{
    //TODO: THIS CODE IS STILL EXPERIMENTAL, REFACTORING NEEDED AFTER EXPERIMENT IS FINAL
    class JavaClassListener : JavaBaseListener
    {
        private string _logfileName;

        private static int id = 0;

        private Project _project;

        private Class currentClass;
        private Method currentMethod;

        public JavaClassListener(Project project)
        {
            _logfileName = "test" + id++ + ".log";
            _project = project;
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

                Method methodModel = new Method(numberOfParams, inputStream.GetText(interval));
                if (currentClass != null)
                    currentClass.AddMethod(methodModel);

                currentMethod = methodModel;
            }
        }

        public override void EnterStatement([NotNull] JavaParser.StatementContext context)
        {
            if (currentMethod != null)
            {
                currentMethod.NumberOfStatements++;

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
                    startText.StartsWith("case") )
                {
                    Debug.WriteLine("--------");
                    Debug.WriteLine(inputStream.GetText(interval));
                    Debug.WriteLine("--------\n");
                    currentMethod.Complexity++;
                }
            }
        }

        public override void EnterClassDeclaration([NotNull] JavaParser.ClassDeclarationContext context)
        {
            Console.WriteLine("parsing class");

            using (var stream = File.Open(_logfileName, FileMode.Append))
            {
                StreamWriter writer = new StreamWriter(stream);

                var inputStream = context.Start.InputStream;

                var interval = new Interval(context.Start.StartIndex, context.Stop.StopIndex);

                writer.WriteLine(inputStream.GetText(interval));
                writer.Flush();

                Class classModel = new Class(inputStream.GetText(interval));
                _project.AddClass(classModel);

                currentClass = classModel;
            }
        }

        public override void ExitMethodDeclaration([NotNull] JavaParser.MethodDeclarationContext context)
        {
            currentMethod = null;
        }

        public override void ExitClassDeclaration([NotNull] JavaParser.ClassDeclarationContext context)
        {
            currentClass = null;
        }
    }
}
