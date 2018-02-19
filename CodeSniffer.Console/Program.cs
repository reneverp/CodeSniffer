using CodeSniffer.ViewModels;
using CodeSniffer.ViewModels.Utilities;
using System;
using System.Threading.Tasks;

namespace CodeSniffer.Console
{
    class Program
    {
        private static MainWindowViewModel _viewModel;

        static void Main(string[] args)
        {
            

            Parser parser = new Parser();
            DirectoryUtil dirUtil = new DirectoryUtil();
            AsyncParserWrapper asyncParser = new AsyncParserWrapper(parser, dirUtil);

            ViewModels.ApplicationInterfaces.IOService ioService = new IOService();

            _viewModel = new MainWindowViewModel(asyncParser, ioService);

            var classVerificationName = @"C:\playground\git\codesniffer\CodeSniffer\CodeSniffer.BBN\VerificationData\ClassTrainingSet_1357_18022018_discretized.csv";
            var methodVerificationName = @"C:\playground\git\codesniffer\CodeSniffer\CodeSniffer.BBN\VerificationData\MethodTrainingSet_1357_18022018_discretized.csv";

            var classDataset = BBN.Discretization.DataSetHelper.GetDataSetForCSV(classVerificationName);
            var methodDataset = BBN.Discretization.DataSetHelper.GetDataSetForCSV(methodVerificationName);

            var annotatedLargeClasses = classDataset.Tables[0].Select("Large_Class = True");
            var annotatedFeatureEnvyMethods = methodDataset.Tables[0].Select("Feature_Envy = True");
            var annotatedLongMethods = methodDataset.Tables[0].Select("Long_Method = True");

            Random largeClassRandom = new Random();
            Random featureEnvyRandom = new Random();
            Random longMethodRandom = new Random();

            for (int i = 0; i < 10; i++)
            {
                int largeClassIndex = largeClassRandom.Next(annotatedLargeClasses.Length);
                int featureEnvyIndex = featureEnvyRandom.Next(annotatedFeatureEnvyMethods.Length);
                int longMehodIndex = longMethodRandom.Next(annotatedLongMethods.Length);

                System.Console.WriteLine(largeClassIndex);
                System.Console.WriteLine(featureEnvyIndex);
                System.Console.WriteLine(longMehodIndex);

                GenerateDataSet(i);
            }
        }

        private static void GenerateDataSet(int runId)
        {
            Task.Run(async () => { await _viewModel.RefreshAsync(); }).Wait();

            System.Console.WriteLine("Code Fragments:: " + _viewModel.CodeFragments.Count);

            _viewModel.GenerateDataset("run_" + runId + ".csv");
        }
    }
}
