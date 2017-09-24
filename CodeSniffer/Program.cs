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
            System.Console.ReadKey();

            //ThreadPool.SetMaxThreads(8, 8);

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

            using (var stream = new StreamWriter(File.Open("overall.log", FileMode.OpenOrCreate)))
            {

                project.Sort();

                stream.WriteLine("Time needed for parsing::: " + stopWatch.Elapsed.ToString());

                var classes = project.Classes;

                stream.WriteLine("Detected number of classes:: " + classes.Count);

                foreach (var cl in classes)
                {
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

                }
            }

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(project);

            using (var file = new StreamWriter(File.Open("Output.json", FileMode.OpenOrCreate)))
            {
                file.WriteLine(json);
            }
        }
    }
}
