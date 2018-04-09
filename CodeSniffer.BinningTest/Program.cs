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

            string appConfCrossValidation = "CodeSniffer.CrossValidation.exe.config";
            string appConfCodeSnifferConsole = "CodeSniffer.Console.exe.config";


            for (int i = 0; i < 20; i++)
            {
                UpdateBinCount(doc, appConfCrossValidation, i);
                UpdateBinCount(doc, appConfCodeSnifferConsole, i);

                RunCrossValidation(i);
            }
        }

        private static void UpdateBinCount(XmlDocument doc, string appConf, int i)
        {
            doc.Load(appConf);

            var root = doc.DocumentElement;
            var nodes = root.SelectNodes("appSettings//add");
            foreach (XmlNode node in nodes)
            {
                if (node.OuterXml.Contains("key=\"NumberOfBinsMethod\"") || node.OuterXml.Contains("key=\"NumberOfBinsClass\""))
                {
                    node.Attributes["value"].Value = (i + 2).ToString();
                }
            }

            doc.Save(appConf);
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

            if (File.Exists(_basePath + @"\BinningTestCrossValidationOutput.csv"))
            {
                File.Delete(_basePath + @"\BinningTestCrossValidationOutput.csv");
            }
        }

        private static void GenerateDataSet(int runId)
        {
            System.Console.WriteLine("RunId: " + runId);

            //we write continuous data, so discretize the additional data first.
            ProcessStartInfo startInfo = new ProcessStartInfo { FileName = _basePath + "\\CodeSniffer.Console.exe", Arguments = runId.ToString() + " " + _sourcePath, CreateNoWindow = true, WindowStyle = ProcessWindowStyle.Hidden };
            Process.Start(startInfo).WaitForExit();
        }

        private static void RunCrossValidation(int runId)
        {
            System.Console.WriteLine("RunId: " + runId);

            //we write continuous data, so discretize the additional data first.
            ProcessStartInfo startInfo = new ProcessStartInfo { FileName = _basePath + "\\CodeSniffer.CrossValidation.exe", CreateNoWindow = true, WindowStyle = ProcessWindowStyle.Hidden };
            Process.Start(startInfo).WaitForExit();

            ProcessStartInfo pythonStartInfo = new ProcessStartInfo { FileName = @"D:\Programs\Python\Python36\python.exe", Arguments = _basePath + @"\..\..\PythonScripts\plot_accuracy_curve_crossvalidation.py CrossValidation_" + runId, UseShellExecute = false, RedirectStandardOutput = true, RedirectStandardError = true, RedirectStandardInput = true };
            var p = Process.Start(pythonStartInfo);
            p.WaitForExit();

            string toWrite ="";
            while (!p.StandardOutput.EndOfStream)
            {
                toWrite += p.StandardOutput.ReadLine() + ",";
            }

            System.Console.WriteLine(toWrite);

            using (StreamWriter sw = new StreamWriter(_basePath + "\\BinningTestCrossValidationOutput.csv", true))
            {
                sw.WriteLine(toWrite.Substring(0, toWrite.Length - 1));
            }

        }
    }
}
