using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CodeSniffer.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CodeSniffer
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.ReadKey();

            ThreadPool.SetMaxThreads(8, 0);

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

            System.Console.WriteLine("Detected number of classes:: " + project.Classes.Count);

            foreach(var cl in project.Classes)
            {
                System.Console.WriteLine("NumberOfMethods: " + cl.NumberOfMethods);
                System.Console.WriteLine("Classname: " + cl.Text);
                System.Console.WriteLine("---------");
                System.Console.WriteLine("\n");

            }
        }
    }
}
