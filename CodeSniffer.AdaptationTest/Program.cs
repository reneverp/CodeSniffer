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

        static void Main(string[] args)
        {
            _basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var classVerificationName = _basePath + @"\..\..\CodeSniffer.BBN\VerificationData\ClassTrainingSet_1357_18022018.csv";
            var methodVerificationName = _basePath + @"\..\..\CodeSniffer.BBN\VerificationData\MethodTrainingSet_1357_18022018.csv";

            var classDataset = BBN.Discretization.DataSetHelper.GetDataSetForCSV(classVerificationName);
            var methodDataset = BBN.Discretization.DataSetHelper.GetDataSetForCSV(methodVerificationName);


            var annotatedLargeClasses = classDataset.Tables[0].Select("Large_Class = True");
            var annotatedFeatureEnvyMethods = methodDataset.Tables[0].Select("Feature_Envy = True");
            var annotatedLongMethods = methodDataset.Tables[0].Select("Long_Method = True");

            var annotatedLargeClassesFalse = classDataset.Tables[0].Select("Large_Class = False");
            var annotatedFeatureEnvyMethodsFalse = methodDataset.Tables[0].Select("Feature_Envy = False");
            var annotatedLongMethodsFalse = methodDataset.Tables[0].Select("Long_Method = False");

            Random largeClassRandom = new Random();
            Random featureEnvyRandom = new Random();
            Random longMethodRandom = new Random();


            _additionalClassCasesFile = _basePath + @"\TrainingsData\ClassAdditionalData.csv";
            _additionalMethodCasesFile = _basePath + @"\TrainingsData\MethodAdditionalData.csv";

            _additionalClassCasesFileDiscretized = _basePath + @"\TrainingsData\ClassAdditionalData_discretized.csv";
            _additionalMethodCasesFileDiscretized = _basePath + @"\TrainingsData\MethodAdditionalData_discretized.csv";

            IList<string> classHeader = new List<string>();
            IList<string> methodHeader = new List<string>();

            foreach (DataColumn col in classDataset.Tables[0].Columns)
            {
                classHeader.Add(col.ColumnName);
            }

            foreach (DataColumn col in methodDataset.Tables[0].Columns)
            {
                methodHeader.Add(col.ColumnName);
            }

            using (var sw = new StreamWriter(_additionalClassCasesFile))
            {
                sw.WriteLine(string.Join(",", classHeader.ToArray()));
            }

            using (var sw = new StreamWriter(_additionalMethodCasesFile))
            {
                sw.WriteLine(string.Join(",", methodHeader.ToArray()));
            }


            for (int i = 0; i < 8; i++)
            {

                GenerateDataSet(i);


                StringBuilder classSb = new StringBuilder();
                StringBuilder featureEnvSb = new StringBuilder();
                StringBuilder longmethodSb = new StringBuilder();

                for (int y = 0; y < 100; y++)
                {
                    int largeClassIndex = largeClassRandom.Next(annotatedLargeClasses.Length);
                    int featureEnvyIndex = featureEnvyRandom.Next(annotatedFeatureEnvyMethods.Length);
                    int longMehodIndex = longMethodRandom.Next(annotatedLongMethods.Length);

                    var largeClassRow = annotatedLargeClasses[largeClassIndex];
                    //var featureEnvyRow = annotatedFeatureEnvyMethods[featureEnvyIndex];
                    var longMethodRow = annotatedLongMethods[longMehodIndex];

                    var fields = largeClassRow.ItemArray.Select(field => field.ToString()).ToArray();
                    classSb.AppendLine(string.Join(",", fields));

                    //fields = featureEnvyRow.ItemArray.Select(field => field.ToString()).ToArray();
                    //featureEnvSb.AppendLine(string.Join(",", fields));

                    fields = longMethodRow.ItemArray.Select(field => field.ToString()).ToArray();
                    longmethodSb.AppendLine(string.Join(",", fields));
                }

                //for (int y = 0; y < 10; y++)
                //{
                //    int largeClassIndex = largeClassRandom.Next(annotatedLargeClassesFalse.Length);
                //    int featureEnvyIndex = featureEnvyRandom.Next(annotatedFeatureEnvyMethodsFalse.Length);
                //    int longMehodIndex = longMethodRandom.Next(annotatedLongMethodsFalse.Length);

                //    var largeClassRow = annotatedLargeClassesFalse[largeClassIndex];
                //    //var featureEnvyRow = annotatedFeatureEnvyMethodsFalse[featureEnvyIndex];
                //    var longMethodRow = annotatedLongMethodsFalse[longMehodIndex];

                //    var fields = largeClassRow.ItemArray.Select(field => field.ToString()).ToArray();
                //    classSb.AppendLine(string.Join(",", fields));

                //    //fields = featureEnvyRow.ItemArray.Select(field => field.ToString()).ToArray();
                //    //featureEnvSb.AppendLine(string.Join(",", fields));

                //    fields = longMethodRow.ItemArray.Select(field => field.ToString()).ToArray();
                //    longmethodSb.AppendLine(string.Join(",", fields));

                //}

                using (var sw = new StreamWriter(_additionalClassCasesFile, true))
                {
                    sw.Write(classSb.ToString());
                }

                using (var sw = new StreamWriter(_additionalMethodCasesFile, true))
                {
                    //sw.Write(featureEnvSb.ToString());
                    sw.Write(longmethodSb.ToString());
                }
            }
        }

        private static void GenerateDataSet(int runId)
        {
            System.Console.WriteLine("RunId: " + runId);

            //we write continuous data, so discretize the additional data first.
            Process.Start(_basePath + "\\CodeSniffer.Console.exe", runId.ToString()).WaitForExit();
        }
    }
}
