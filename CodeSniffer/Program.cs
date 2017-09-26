using CodeSniffer.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using NLog;
using System;

namespace CodeSniffer
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            Console.ReadKey();

            var stopWatch = Stopwatch.StartNew();

            System.Console.WriteLine("Parsing started at: " + DateTime.Now);

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

            int totalnumberOfClasses = 0;

            project.Sort();

            logger.Info("Time needed for parsing::: " + stopWatch.Elapsed.ToString());

            var compilationUnits = project.CompilationUnits;

            logger.Info("Detected number of compilationUnits:: " + compilationUnits.Count);

            foreach (var compilationUnit in compilationUnits)
            {
                var classes = compilationUnit.Classes;

                if (classes != null)
                {
                    totalnumberOfClasses = PrintClasses(totalnumberOfClasses, classes);
                }
            }

            logger.Info("TOTAL CLASSES: " + totalnumberOfClasses);

            //logger.Info(Newtonsoft.Json.JsonConvert.SerializeObject(project));

            System.Console.WriteLine("Parsing finished at: " + DateTime.Now);
        }

        private static int PrintClasses(int totalnumberOfClasses, IList<Class> classes)
        {
 
            totalnumberOfClasses += classes.Count;

            foreach (var cl in classes)
            {
                if(cl == null)
                {
                    break;
                }

                logger.Info("ClassName: " + cl.Name);
                logger.Info("NumberOfMethods: " + cl.NumberOfMethods);
                logger.Info("Class complexity: " + cl.Complexity);
                logger.Info("Class loc: " + cl.LinesOfCode);

                foreach (var met in cl.Methods)
                {
                    logger.Info("Method number of params: " + met.NumberOfParams);
                    logger.Info("Method loc: " + met.LinesOfCode);
                    logger.Info("Method complexity: " + met.Complexity);
                    logger.Info("Method Number of Statements: " + met.NumberOfStatements);
                }


                if(cl.Classes.Count > 0)
                {
                    PrintClasses(totalnumberOfClasses, cl.Classes);
                }

            }

            return totalnumberOfClasses;
        }
    }
}
