using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CodeSniffer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CodeSniffer
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopWatch = Stopwatch.StartNew();

            DirectoryUtil dirUtil = new DirectoryUtil();
            dirUtil.DeleteLogFile();

            var files = dirUtil.GetFileNames(@"D:\svn\ganttproject-ganttproject-2.8.5\ganttproject-ganttproject-2.8.5\ganttproject\src\net\sourceforge\ganttproject", "java");

            Parser parser = new Parser();

            List<Task> tasks = new List<Task>();

            Project project = new Project();

            foreach (var file in files)
            {
                tasks.Add(Task.Run(() => parser.Parse(file, project)));
            }

            Task.WaitAll(tasks.ToArray());

            stopWatch.Stop();

            using (var stream = new StreamWriter(File.Open("overall.log", FileMode.Create)))
            {
                int totalnumberOfClasses = 0;

                project.Sort();

                stream.WriteLine("Time needed for parsing::: " + stopWatch.Elapsed.ToString());

                var compilationUnits = project.CompilationUnits;

                stream.WriteLine("Detected number of compilationUnits:: " + compilationUnits.Count);

                foreach (var compilationUnit in compilationUnits)
                {
                    var classes = compilationUnit.Classes;

                    if (classes != null)
                    {
                        totalnumberOfClasses = PrintClasses(stream, totalnumberOfClasses, classes);
                    }
                }

                stream.WriteLine("TOTAL CLASSES: " + totalnumberOfClasses);
            }

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(project);

            using (var file = new StreamWriter(File.Open("Output.json", FileMode.OpenOrCreate)))
            {
                file.WriteLine(json);
            }
        }

        private static int PrintClasses(StreamWriter stream, int totalnumberOfClasses, IList<Class> classes)
        {
 
            totalnumberOfClasses += classes.Count;

            foreach (var cl in classes)
            {
                if(cl == null)
                {
                    break;
                }

                stream.WriteLine("ClassName: " + cl.Name);
                stream.WriteLine("NumberOfMethods: " + cl.NumberOfMethods);
                stream.WriteLine("Class complexity: " + cl.Complexity);
                stream.WriteLine("Class loc: " + cl.LinesOfCode);

                foreach (var met in cl.Methods)
                {
                    stream.WriteLine("Method number of params: " + met.NumberOfParams);
                    stream.WriteLine("Method loc: " + met.LinesOfCode);
                    stream.WriteLine("Method complexity: " + met.Complexity);
                    stream.WriteLine("Method Number of Statements: " + met.NumberOfStatements);
                    stream.WriteLine("---------");
                }

                stream.WriteLine("---------");
                stream.WriteLine("\n");

                if(cl.Classes.Count > 0)
                {
                    PrintClasses(stream, totalnumberOfClasses, cl.Classes);
                }

            }

            return totalnumberOfClasses;
        }
    }
}
