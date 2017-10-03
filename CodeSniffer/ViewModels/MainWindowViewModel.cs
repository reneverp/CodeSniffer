﻿using CodeSniffer.Interfaces;
using CodeSniffer.Models;
using CodeSniffer.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using CodeSniffer.ApplicationInterfaces;

namespace CodeSniffer.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private readonly AsyncParserWrapper _parser;
        private readonly ApplicationInterfaces.IOService _ioService;

        private ObservableCollection<CodeFragmentViewModel> _codeFragments;

        private IProject _project;
        private string _sourcePath;
        private ObservableCollection<MetricViewModel> _metrics;

        private string _parseInfo;
        private CodeFragmentViewModel _currentCodeFragment;

        public CodeFragmentViewModel CurrentCodeFragment
        {
            get { return _currentCodeFragment; }
            set
            {
                _currentCodeFragment = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<MetricViewModel> Metrics
        {
            get { return _metrics; }
            set
            {
                _metrics = value;
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

        public ObservableCollection<CodeFragmentViewModel> CodeFragments
        {
            get { return _codeFragments; }
            set
            {
                _codeFragments = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand ExitCommand { get; private set; }

        public ICommand RefreshCommand { get; private set; }

        public ICommand OpenCommand { get; private set; }

        public ICommand ShowCodeFragmentCommand { get; private set; }


        public MainWindowViewModel(AsyncParserWrapper asyncParser, ApplicationInterfaces.IOService ioService)
        {
            //TODO: remove hardcoded path
            _sourcePath = @"D:\svn\ganttproject-ganttproject-2.8.5\ganttproject-ganttproject-2.8.5\ganttproject\src\net\sourceforge\ganttproject";

            _project = new Project();
            _parser = asyncParser;
            _ioService = ioService;
            CurrentCodeFragment = new CodeFragmentViewModel();

            ExitCommand = new RelayCommand(() => Environment.Exit(0));
            RefreshCommand = new RelayCommand(Refresh);
            OpenCommand = new RelayCommand(OpenFolder);
            ShowCodeFragmentCommand = new RelayCommand<CodeFragmentViewModel>(ShowCodeFragment);
        }

        private void OpenFolder()
        {
            _sourcePath = _ioService.OpenFolderDialog();
            Refresh();
        }

        private void ShowCodeFragment(CodeFragmentViewModel codeFragment)
        {
            if (codeFragment != null)
            {
                CurrentCodeFragment = codeFragment;
                Metrics = new ObservableCollection<MetricViewModel>();

                foreach(var metric in codeFragment.Model.Metrics)
                {
                    Metrics.Add(new MetricViewModel(metric));
                }
            }
        }

        public async void Refresh()
        {
            ParseInfoLines = "";
            CurrentCodeFragment = new CodeFragmentViewModel();
            CodeFragments = new ObservableCollection<CodeFragmentViewModel>();
            Metrics = new ObservableCollection<MetricViewModel>();

            OnParseInfoUpdated("Parsing files started");

            _project = await _parser.ParseAsync(_sourcePath);

            if (_project != null)
            {
                OnParseInfoUpdated("Parsing files finished");
            }
            else
            {
                OnParseInfoUpdated("Error: Parsing failed");
                return;
            }

            FillCodeFragments();

            OnParseInfoUpdated("Parsed " + _project.GetClassCount() + " classes in " + _project.GetCompilationUnitsCount() + " compilation units");
        }

        private void FillCodeFragments()
        {
            foreach (var compilationUnit in _project.CompilationUnits)
            {
                foreach (var cl in compilationUnit.Classes)
                {
                    var clItem = new CodeFragmentViewModel(cl);
                    CodeFragments.Add(clItem);

                    foreach (var method in cl.Children)
                    {
                        var mItem = new CodeFragmentViewModel(method);
                        clItem.AddChild(mItem);
                    }
                }
            }

            CodeFragments = SortCodeFragments(CodeFragments);

            var fragment = CodeFragments.FirstOrDefault();
            if (fragment != null)
                fragment.IsSelected = true;
        }

        private ObservableCollection<CodeFragmentViewModel> SortCodeFragments(ObservableCollection<CodeFragmentViewModel> codeFragments)
        {
            //TODO: MOVE TO SEPARATE CLASS
            foreach (var codeFragment in codeFragments)
            {
                if (codeFragment.Children != null)
                    codeFragment.Children = SortCodeFragments(codeFragment.Children);
            }

            return new ObservableCollection<CodeFragmentViewModel>(codeFragments.OrderBy(x => x.Name));
        }

        private void OnParseInfoUpdated(string line)
        {
            Application.Current.Dispatcher.Invoke(() => ParseInfoLines += DateTime.Now + " :: Info: " + line + "\n");
        }

    }
}
