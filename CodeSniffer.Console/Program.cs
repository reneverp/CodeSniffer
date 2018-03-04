using CodeSniffer.ViewModels;
using CodeSniffer.ViewModels.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Console
{
    class Program
    {
        private static MainWindowViewModel _viewModel;

        static void Main(string[] args)
        {
            if (args.Length < 1)
                return;

            string runId = args[0];
            string path = null;

            if(args.Length == 2)
            {
                path = args[1];
            }

            Parser parser = new Parser();
            DirectoryUtil dirUtil = new DirectoryUtil();
            AsyncParserWrapper asyncParser = new AsyncParserWrapper(parser, dirUtil);

            ViewModels.ApplicationInterfaces.IOService ioService = new IOService();

            _viewModel = new MainWindowViewModel(asyncParser, ioService, path);

            _viewModel.RefreshAsync().Wait();

            var filename = "run_" + runId + ".csv";

            System.Console.WriteLine("Writing output to: " + filename);

            _viewModel.GenerateDataset(filename);
        }
    }
}
