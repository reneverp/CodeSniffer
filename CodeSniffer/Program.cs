using CodeSniffer.Utilities;
using CodeSniffer.ViewModels;
using System;
using System.Windows;

namespace CodeSniffer
{
    public class Program
    {
        private static MainWindowViewModel _viewModel;

        [STAThread]
        public static void Main(string[] args)
        {
            var app = new App();

            Parser parser = new Parser();
            DirectoryUtil dirUtil = new DirectoryUtil();
            AsyncParserWrapper asyncParser = new AsyncParserWrapper(parser, dirUtil);

            _viewModel = new MainWindowViewModel(asyncParser);
            var window = new MainWindow { DataContext = _viewModel };

            window.Loaded += OnWindowLoaded;
            window.Show();

            app.Run();
        }

        private static void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Refresh(null);
        }
    }
}
