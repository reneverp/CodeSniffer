using CodeSniffer.Interfaces;
using CodeSniffer.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace CodeSniffer.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private DirectoryUtil _directoryUtil;
        private Project _project;
        private string _sourcePath;
        private IParser _parser;

        private string _activeCodeFragment;
        private string _parseInfo;

        public string CodeFragment {
            get { return _activeCodeFragment; }
            set {
                _activeCodeFragment = value;
                NotifyPropertyChanged();
            }
        }

        public string ParseInfoLines
        {
            get { return _parseInfo; }
            set {
                _parseInfo = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand ExitCommand { get; private set; }

        public ICommand RefreshCommand { get; private set; }


        public MainWindowViewModel(IParser parser, DirectoryUtil directoryUtility)
        {
            //TODO: remove hardcoded path
            _sourcePath = @"D:\svn\ganttproject-ganttproject-2.8.5\ganttproject-ganttproject-2.8.5\ganttproject\src\net\sourceforge\ganttproject";

            _directoryUtil = directoryUtility;
            _project = new Project();
            _parser = parser;

            ExitCommand = new RelayCommand(() => Environment.Exit(0));
            RefreshCommand = new RelayCommand(Refresh);
        }

        public async void Refresh()
        {
            ParseInfoLines = "";

            OnParseInfoUpdated("Parsing files started");


            bool parseResult = await ParrallelParse();

            if(!parseResult)
            {
                OnParseInfoUpdated("Error: Parsing failed");
            }
            else
            {
                OnParseInfoUpdated("Parsing files finished");
            }

            int count = 0;
            foreach(var cu in _project.CompilationUnits)
            {
                count += cu.Classes.Count;
            }

            OnParseInfoUpdated("Parsed " + count + " classes in " + _project.CompilationUnits.Count + " comiplation units");
        }

        private void OnParseInfoUpdated(string line)
        {
            Application.Current.Dispatcher.Invoke(() => ParseInfoLines += DateTime.Now + " :: Info: " + line + "\n");
        }

        private async Task<bool> ParrallelParse()
        {
            bool success = true;

            var filenames = _directoryUtil.GetFileNames(_sourcePath, "java");

            List<Task> taskList = new List<Task>();

            foreach(var filename in filenames)
            {
                taskList.Add(Task.Run(() => _parser.Parse(filename, _project)));
            }

            await Task.WhenAll(taskList);

            if(taskList.Any(x => x.IsFaulted))
            {
                success = false;
            }

            return success;
        }

    }
}
