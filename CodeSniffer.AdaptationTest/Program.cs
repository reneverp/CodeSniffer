using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

            _basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            DeleteFiles();



            //var classVerificationName = _basePath + @"\..\..\CodeSniffer.BBN\VerificationData\ClassTrainingSet_1357_18022018.csv";

            var classVerificationName = _basePath + @"\ClassTest.csv";
            //var methodVerificationName = _basePath + @"\..\..\CodeSniffer.BBN\VerificationData\MethodTrainingSet_1357_18022018.csv";
            var methodVerificationName = _basePath + @"\MethodTest.csv";

            WaitForFile(classVerificationName);
            WaitForFile(methodVerificationName);

            var classVerificationDataset = BBN.Discretization.DataSetHelper.GetDataSetForCSV(classVerificationName);
            var methodVerificationDataset = BBN.Discretization.DataSetHelper.GetDataSetForCSV(methodVerificationName);


            var annotatedLargeClasses = classVerificationDataset.Tables[0].Select("Large_Class = True");
            var annotatedFeatureEnvyMethods = methodVerificationDataset.Tables[0].Select("Feature_Envy = True");
            var annotatedLongMethods = methodVerificationDataset.Tables[0].Select("Long_Method = True");

            var annotatedLargeClassesFalse = classVerificationDataset.Tables[0].Select("Large_Class = False");
            var annotatedFeatureEnvyMethodsFalse = methodVerificationDataset.Tables[0].Select("Feature_Envy = False");
            var annotatedLongMethodsFalse = methodVerificationDataset.Tables[0].Select("Long_Method = False");

            Random largeClassRandom = new Random();
            Random featureEnvyRandom = new Random();
            Random longMethodRandom = new Random();


            _additionalClassCasesFile = _basePath + @"\TrainingsData\ClassAdditionalData.csv";
            _additionalMethodCasesFile = _basePath + @"\TrainingsData\MethodAdditionalData.csv";

            IList<string> classHeader = new List<string>();
            IList<string> methodHeader = new List<string>();

            foreach (DataColumn col in classVerificationDataset.Tables[0].Columns)
            {
                classHeader.Add(col.ColumnName);
            }

            foreach (DataColumn col in methodVerificationDataset.Tables[0].Columns)
            {
                methodHeader.Add(col.ColumnName);
            }

            for (int i = 0; i < 10; i++)
            {
                GenerateDataSet(i);

                //generate new adaptation files every time we start the app
                using (var sw = new StreamWriter(_additionalClassCasesFile))
                {
                    sw.WriteLine(string.Join(",", classHeader.ToArray()));
                }

                using (var sw = new StreamWriter(_additionalMethodCasesFile))
                {
                    sw.WriteLine(string.Join(",", methodHeader.ToArray()));
                }

                var classDataSet = BBN.Discretization.DataSetHelper.GetDataSetForCSV(_basePath + "\\Classrun_" + i + ".csv");
                var methodDataSet = BBN.Discretization.DataSetHelper.GetDataSetForCSV(_basePath + "\\Methodrun_" + i + ".csv");

                IList<DataRow> wrongCasesClass = new List<DataRow>();
                IList<DataRow> wrongCasesMethodFE = new List<DataRow>();
                IList<DataRow> wrongCasesMethodLM = new List<DataRow>();



                //pick all wrongly predicted class rows
                int z = 0;
                foreach (DataRow row in classDataSet.Tables[0].Rows)
                {
                    if (row.Field<string>("Large_Class") != classVerificationDataset.Tables[0].Rows[z].Field<string>("Large_Class"))
                    {
                        wrongCasesClass.Add(classVerificationDataset.Tables[0].Rows[z]);
                    }

                    z++;
                }

                //pick all wrongly predicted method rows
                z = 0;
                foreach (DataRow row in methodDataSet.Tables[0].Rows)
                {
                    if (row.Field<string>("Feature_Envy") != methodVerificationDataset.Tables[0].Rows[z].Field<string>("Feature_Envy"))
                    {
                        wrongCasesMethodFE.Add(methodVerificationDataset.Tables[0].Rows[z]);
                    }

                    if (row.Field<string>("Long_Method") != methodVerificationDataset.Tables[0].Rows[z].Field<string>("Long_Method"))
                    {
                        wrongCasesMethodLM.Add(methodVerificationDataset.Tables[0].Rows[z]);
                    }

                    z++;
                }

                System.Console.WriteLine("Wrong cases class: " + wrongCasesClass.Count);
                System.Console.WriteLine("Wrong cases methodFE: " + wrongCasesMethodFE.Count);
                System.Console.WriteLine("Wrong cases classLM: " + wrongCasesMethodLM.Count);

                StringBuilder classSb = new StringBuilder();
                StringBuilder featureEnvSb = new StringBuilder();
                StringBuilder longmethodSb = new StringBuilder();

                for (int y = 0; y < 10; y++)
                {
                    int largeClassIndex = largeClassRandom.Next(wrongCasesClass.Count);
                    var largeClassRow = largeClassIndex > 0 ? wrongCasesClass[largeClassIndex] : null;


                    if (largeClassRow != null)
                    {
                        var fields = largeClassRow.ItemArray.Select(field => field.ToString()).ToArray();
                        classSb.AppendLine(string.Join(",", fields));
                    }

                    for (int x = 0; x < 10; x++)
                    {
                        int featureEnvyIndex = featureEnvyRandom.Next(wrongCasesMethodFE.Count);
                        int longMehodIndex = longMethodRandom.Next(wrongCasesMethodLM.Count);

                        var featureEnvyRow = featureEnvyIndex > 0 ? wrongCasesMethodFE[featureEnvyIndex] : null;
                        var longMethodRow = longMehodIndex > 0 ? wrongCasesMethodLM[longMehodIndex] : null;

                        if (featureEnvyRow != null)
                        {
                            var fields = featureEnvyRow.ItemArray.Select(field => field.ToString()).ToArray();
                            featureEnvSb.AppendLine(string.Join(",", fields));
                        }

                        if (longMethodRow != null)
                        {
                            var fields = longMethodRow.ItemArray.Select(field => field.ToString()).ToArray();
                            longmethodSb.AppendLine(string.Join(",", fields));
                        }
                    }
                }

                using (var sw = new StreamWriter(_additionalClassCasesFile, true))
                {
                    sw.Write(classSb.ToString());
                }

                using (var sw = new StreamWriter(_additionalMethodCasesFile, true))
                {
                    sw.Write(featureEnvSb.ToString());
                    sw.Write(longmethodSb.ToString());

                }
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

        private static void WaitForFile(string filename)
        {
            for(int i =0; i < 10; i++)
            {
                if(File.Exists(filename))
                {
                    return;
                }
            }
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
