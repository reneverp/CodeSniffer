using CodeSniffer.ViewModels;
using CodeSniffer.ViewModels.Utilities;
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

            for (int i = 0; i < 100; i++)
            {
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
