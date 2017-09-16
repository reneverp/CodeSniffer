using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeSniffer
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryUtil dirUtil = new DirectoryUtil();
            dirUtil.DeleteLogFile();

            var files = dirUtil.GetFileNames(@"D:\svn\ganttproject-ganttproject-2.8.5\ganttproject-ganttproject-2.8.5\ganttproject\src\net\sourceforge\ganttproject", "java");

            Parser parser = new Parser();

            List<Task> tasks = new List<Task>();

            foreach (var file in files)
            {
                tasks.Add(Task.Run(() => parser.Parse(files[0])));
            }

            Task.WaitAll(tasks.ToArray());

        }
    }
}
