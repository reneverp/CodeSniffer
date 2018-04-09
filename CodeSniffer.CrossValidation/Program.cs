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

            _basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            DeleteFiles();

            SplitDataSet();
        }


        private static void SplitDataSet()
        {
            var classDataSetFile  = _basePath + @"\..\..\CodeSniffer.BBN\TrainingsData\ClassTrainingSet_2319_17022018.csv";
            var methodDataSetFile = _basePath + @"\..\..\CodeSniffer.BBN\TrainingsData\MethodTrainingSet_2319_17022018.csv";

            var classDataset = BBN.Discretization.DataSetHelper.GetDataSetForCSV(classDataSetFile);
            var methodDataset = BBN.Discretization.DataSetHelper.GetDataSetForCSV(methodDataSetFile);

            //using K-Fold cross validation
            int K = 5;

            var classBinSize = classDataset.Tables[0].Rows.Count / K;
            var methodBinSize = classDataset.Tables[0].Rows.Count / K;

            for (int i = 0; i < K; i++)
            {
                WriteToCsv("ClassTestSet_" + i + ".csv", classDataset, i*classBinSize, classBinSize);
                WriteToCsv("ClassTrainingSet_" + i + ".csv", classDataset, i * classBinSize, classBinSize, true);
                WriteToCsv("MethodTestSet_" + i + ".csv", methodDataset, i*methodBinSize, methodBinSize);
                WriteToCsv("MethodTrainingSet_" + i + ".csv", methodDataset, i * methodBinSize, methodBinSize, true);

                RunCrossValidation(i);
            }
        }

        private static void RunCrossValidation(int run)
        {
            BBN.Discretization.Discretizer.Reset();

            File.Copy(_basePath + @"\ClassTrainingSet_" + run + ".csv", _basePath + @"\TrainingsData\ClassTrainingSet_2319_17022018.csv", true);
            File.Copy(_basePath + @"\MethodTrainingSet_" + run + ".csv", _basePath + @"\TrainingsData\MethodTrainingSet_2319_17022018.csv", true);

            File.Delete(_basePath + @"\Networks\FeatureEnvy_network_naive.json");
            File.Delete(_basePath + @"\Networks\LargeClass_network_naive.json");
            File.Delete(_basePath + @"\Networks\LongMethod_network_naive.json");

            BBN.Discretization.Discretizer.DiscretizeTrainingSets();

            var classDataSetFile = _basePath + @"\ClassTestSet_" + run + ".csv";
            var methodDataSetFile = _basePath + @"\MethodTestSet_" + run + ".csv";

            var classDataset = BBN.Discretization.DataSetHelper.GetDataSetForCSV(classDataSetFile);
            var methodDataset = BBN.Discretization.DataSetHelper.GetDataSetForCSV(methodDataSetFile);

            var largeClassBBN = BBN.LargeClass.Instance;
            var featureEnvyBBN = BBN.FeatureEnvy.Instance;
            var longMethodBBN = BBN.LongMethod.Instance;

            largeClassBBN.Learn();
            featureEnvyBBN.Learn();
            longMethodBBN.Learn();

            foreach (DataRow row in classDataset.Tables[0].Rows)
            {
                largeClassBBN.SetEvidenceForAtfd(getValueForField(row, "ATFD"));
                largeClassBBN.SetEvidenceForLoc(getValueForField(row, "LOC"));
                largeClassBBN.SetEvidenceForTcc(getValueForField(row, "TCC"));
                largeClassBBN.SetEvidenceForWmc(getValueForField(row, "WMC"));


                if (largeClassBBN.IsLargeClass() > 0.5)
                {
                    row.SetField<string>("Large_Class", "True");
                }
                else
                {
                    row.SetField<string>("Large_Class", "False");
                }
            }

            foreach (DataRow row in methodDataset.Tables[0].Rows)
            {
                featureEnvyBBN.SetEvidenceForAtfd(getValueForField(row, "ATFD"));
                featureEnvyBBN.SetEvidenceForLaa(getValueForField(row, "LAA"));
                featureEnvyBBN.SetEvidenceForFdp(getValueForField(row, "FDP"));

                if (featureEnvyBBN.IsFeatureEnvy() > 0.5)
                {
                    row.SetField<string>("Feature_Envy", "True");
                }
                else
                {
                    row.SetField<string>("Feature_Envy", "False");
                }

                longMethodBBN.SetEvidenceForLoc(getValueForField(row, "LOC"));
                longMethodBBN.SetEvidenceForCyclo(getValueForField(row, "CYCLO"));
                longMethodBBN.SetEvidenceForMaxNesting(getValueForField(row, "MAXNESTING"));
                longMethodBBN.SetEvidenceForNoav(getValueForField(row, "NOAV"));

                if (longMethodBBN.IsLongMethod() > 0.5)
                {
                    row.SetField<string>("Long_Method", "True");
                }
                else
                {
                    row.SetField<string>("Long_Method", "False");
                }

            }

            WriteToCsv("ClassSetOutput_" + run + ".csv", classDataset, 0, -1, true);
            WriteToCsv("MethodSetOutput_" + run + ".csv", methodDataset, 0, -1, true);
        }

        private static double getValueForField(DataRow row, string col)
        {
            double value = 0.0;

            if(row.Table.Columns[col].DataType.FullName.Contains("Double"))
            { 
                value = row.Field<double>(col);
            }
            else
            {
                value = row.Field<int>(col);
            }

            return value;
        }


        private static void WriteToCsv(string filename, DataSet dataset, int startIndex, int count, bool writeTrainingSet = false)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                var rows = dataset.Tables[0].Select();

                string toWrite = "";
                foreach (DataColumn col in dataset.Tables[0].Columns)
                {
                    toWrite += col.ColumnName + ", ";
                }

                sw.WriteLine(toWrite.Substring(0, toWrite.LastIndexOf(',')));

                int start = startIndex;
                int limit = startIndex + count;

                if (writeTrainingSet)
                {
                    start = 0;
                    limit = rows.Count();
                }

                for(int i = start; i < limit; i++)
                {
                    if ((writeTrainingSet && (i < startIndex || i > startIndex + count)) || !writeTrainingSet)
                    {
                        toWrite = "";

                        for (int y = 0; y < rows[i].ItemArray.Count(); y++)
                        {
                            string row = rows[i].ItemArray[y].ToString();

                            var rowType = dataset.Tables[0].Columns[y].DataType.FullName;
                            if (rowType.Contains("Double"))
                            {
                                row = !row.Contains(".") ? row + ".00" : row;
                            }
                           
                            toWrite += row + ",";
                        }

                        sw.WriteLine(toWrite.Substring(0, toWrite.LastIndexOf(',')));
                    }
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

            filesToDelete = Directory.EnumerateFiles(_basePath, "ClassTestSet_*.csv");
            foreach (var file in filesToDelete)
            {
                File.Delete(file);
            }

            filesToDelete = Directory.EnumerateFiles(_basePath, "MethodTestSet_*.csv");
            foreach (var file in filesToDelete)
            {
                File.Delete(file);
            }

            filesToDelete = Directory.EnumerateFiles(_basePath, "ClassTrainingSet_*.csv");
            foreach (var file in filesToDelete)
            {
                File.Delete(file);
            }

            filesToDelete = Directory.EnumerateFiles(_basePath, "MethodTrainingSet_*.csv");
            foreach (var file in filesToDelete)
            {
                File.Delete(file);
            }

            filesToDelete = Directory.EnumerateFiles(_basePath, "ClassSetOutput_*.csv");
            foreach (var file in filesToDelete)
            {
                File.Delete(file);
            }

            filesToDelete = Directory.EnumerateFiles(_basePath, "MethodSetOutput_*.csv");
            foreach (var file in filesToDelete)
            {
                File.Delete(file);
            }

            File.Delete(_basePath + @"\TrainingsData\ClassAdditionalData.csv");
            File.Delete(_basePath + @"\TrainingsData\MethodAdditionalData.csv");
        }

    }
}
