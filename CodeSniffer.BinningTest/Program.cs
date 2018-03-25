using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CodeSniffer.AdaptationTest
{
    class Program
    {
        private static string _basePath;
        private static string _additionalClassCasesFile;
        private static string _additionalMethodCasesFile;

        private static string _additionalClassCasesFileDiscretized;
        private static string _additionalMethodCasesFileDiscretized;
        private static string _sourcePath;

        static void Main(string[] args)
        {

            if (args.Length == 1)
            {
                _sourcePath = args[0];
            }


            XmlDocument doc = new XmlDocument();

            _basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            DeleteFiles();

            for (int i = 0; i < 10; i++)
            {
                doc.Load("CodeSniffer.Console.exe.config");

                var root = doc.DocumentElement;
                var node = root.SelectSingleNode("appSettings//add");
                node.Attributes["value"].Value = (i+2).ToString();

                doc.Save("CodeSniffer.Console.exe.config");

                GenerateDataSet(i);
            }
        }

        private static void DeleteFiles()
        {
            var filesToDelete = Directory.EnumerateFiles(_basePath, "Classrun_*.csv");

            foreach (var file in filesToDelete)
            {
                File.Delete(file);
            }

            filesToDelete = Directory.EnumerateFiles(_basePath, "Methodrun_*.csv");
            foreach (var file in filesToDelete)
            {
                File.Delete(file);
            }

            filesToDelete = Directory.EnumerateFiles(_basePath + @"\Networks\", "*.json");
            foreach (var file in filesToDelete)
            {
                File.Delete(file);
            }

            File.Delete(_basePath + @"\TrainingsData\ClassAdditionalData.csv");
            File.Delete(_basePath + @"\TrainingsData\MethodAdditionalData.csv");
        }

        private static void GenerateDataSet(int runId)
        {
            System.Console.WriteLine("RunId: " + runId);

            //we write continuous data, so discretize the additional data first.
            ProcessStartInfo startInfo = new ProcessStartInfo { FileName = _basePath + "\\CodeSniffer.Console.exe", Arguments = runId.ToString() + " " + _sourcePath, CreateNoWindow = true, WindowStyle = ProcessWindowStyle.Hidden };
            Process.Start(startInfo).WaitForExit();
        }
    }
}
