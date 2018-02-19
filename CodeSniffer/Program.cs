using CodeSniffer.ViewModels.Utilities;
using CodeSniffer.ViewModels;
using System;
using System.Windows;
using System.ComponentModel;
using System.Threading.Tasks;

namespace CodeSniffer
{
    public class Program
    {
        private static MainWindowViewModel _viewModel;

        [STAThread]
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += HandleException;

            var app = new App();

            Parser parser = new Parser();
            DirectoryUtil dirUtil = new DirectoryUtil();
            AsyncParserWrapper asyncParser = new AsyncParserWrapper(parser, dirUtil);

            ViewModels.ApplicationInterfaces.IOService ioService = new IOService();

            _viewModel = new MainWindowViewModel(asyncParser, ioService);
            var window = new MainWindow { DataContext = _viewModel };

            window.Loaded += OnWindowLoaded;
            window.Closing += OnClosing;
            window.Show();

            app.Run();
        }

        private static void OnClosing(object sender, CancelEventArgs e)
        {
            _viewModel.ClosingCommand.Execute(null);
        }

        private static void HandleException(object sender, UnhandledExceptionEventArgs e)
        {
            _viewModel.SaveProject();
            MessageBox.Show("Exception Caught: project saved");

        }

        private static void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Refresh();
        }
    }
}
