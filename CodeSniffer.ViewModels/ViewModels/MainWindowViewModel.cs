using CodeSniffer.Interfaces;
using CodeSniffer.Models;
using CodeSniffer.ViewModels.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Threading.Tasks;

namespace CodeSniffer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly AsyncParserWrapper _parser;
        private readonly ApplicationInterfaces.IOService _ioService;

        private ObservableCollection<CodeFragmentViewModel> _codeFragments;
        public LinkedList<CodeFragmentViewModel> FlatList;


        private IProject _project;
        private string _sourcePath;
        private ObservableCollection<MetricViewModel> _metrics;
        private ObservableCollection<CodeSmellViewModel> _codeSmells;


        private string _parseInfo;
        private CodeFragmentViewModel _currentCodeFragment;
        private string _filename;
        private string _dataSetFilename;

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

        public ObservableCollection<CodeSmellViewModel> CodeSmells
        {
            get { return _codeSmells; }
            set
            {
                _codeSmells = value;
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

        public ICommand LoadProjectCommand { get; private set; }

        public ICommand SaveProjectCommand { get; private set; }
        
        public ICommand ShowCodeFragmentCommand { get; private set; }

        public ICommand NextCommand { get; private set; }

        public ICommand PrevCommand { get; private set; }

        public ICommand ClosingCommand { get; private set; }


        public MainWindowViewModel(AsyncParserWrapper asyncParser, ApplicationInterfaces.IOService ioService, string path = null)
        {
            if(path != null)
            {
                _sourcePath = path;
            }
            else
            {
                //TODO: remove hardcoded paths

                //_sourcePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\CodeProjects\spring-boot\spring-boot-project\spring-boot\src\main\java\org\springframework\boot";
                //_sourcePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\CodeProjects\ganttproject-2.8.5\ganttproject\ganttproject\src\net\sourceforge\ganttproject";
                _sourcePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\CodeProjects\junit-4.12\junit4\src\main";
            }

            _project = new Project();
            _parser = asyncParser;
            _ioService = ioService;
            CurrentCodeFragment = new CodeFragmentViewModel();

            ExitCommand = new RelayCommand(() => Environment.Exit(0));
            RefreshCommand = new RelayCommand(Refresh);
            OpenCommand = new RelayCommand(OpenFolder);
            LoadProjectCommand = new RelayCommand(LoadProject);
            SaveProjectCommand = new RelayCommand(SaveProject);
            ShowCodeFragmentCommand = new RelayCommand<CodeFragmentViewModel>(ShowCodeFragment);

            NextCommand = new RelayCommand(SelectNextFragment);
            PrevCommand = new RelayCommand(SelectPreviousFragment);

            ClosingCommand = new RelayCommand(Closing);

            _filename = "CodeSnifferSavedProject" + System.DateTime.Now.ToString("_Hmm_ddMMyyyy") + ".json";

            _dataSetFilename = "TrainingSet" + System.DateTime.Now.ToString("_Hmm_ddMMyyyy") + ".csv";
        }

        public void Closing()
        {
            var result = MessageBox.Show("Generate Dataset?","CodeSniffer is closing...", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                //SaveProject();
                GenerateDataset(_dataSetFilename);
            }
        }

        public void GenerateDataset(string filename)
        {
            foreach (var fragment in FlatList)
            {
                fragment.Model.WriteToTrainingSet(filename);
            }
        }

        public void SaveProject()
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(CodeFragments, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full
            });


            _ioService.WriteToFile(_filename, json);
        }

        private void LoadProject()
        {
            string filename = _ioService.OpenFileDialog(false);

            if (!string.IsNullOrEmpty(filename))
            {
                string json = _ioService.ReadContentFromFile(filename);

                CodeFragments = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<CodeFragmentViewModel>>(json, new JsonSerializerSettings
                                                                                {
                                                                                    TypeNameHandling = TypeNameHandling.Objects
                                                                                });

                UpdateCodeFragments();
            }
        }

        private void SelectPreviousFragment()
        {
            var previousCodeFragment = FlatList.Find(CurrentCodeFragment).Previous.Value;

            if (previousCodeFragment != null)
            {
                if (!CodeFragments.Contains(previousCodeFragment))
                {
                    //if the next code fragment is not at the root level, set the root level parent to active
                    var parent = FindRelatedParent(previousCodeFragment);
                    if (parent != null)
                        parent.IsActive = true;
                }

                previousCodeFragment.IsSelected = true;
            }
        }

        private void SelectNextFragment()
        {
            var nextCodeFragment = FlatList.Find(CurrentCodeFragment)?.Next?.Value;

            if (nextCodeFragment != null)
            {
                if (!CodeFragments.Contains(nextCodeFragment))
                {
                    //if the next code fragment is not at the root level, set the root level parent to active
                    var parent = FindRelatedParent(nextCodeFragment);
                    if (parent != null)
                        parent.IsActive = true;
                }

                nextCodeFragment.IsSelected = true;
            }
        }

        private CodeFragmentViewModel FindRelatedParent(CodeFragmentViewModel codeFragment)
        {
            return CodeFragments.Where(x => x.Children.Contains(codeFragment)).FirstOrDefault();
        }

        private async void OpenFolder()
        {
            await OpenFolderAsync();
        }

        private async Task OpenFolderAsync()
        {
            _sourcePath = _ioService.OpenFolderDialog();
            await RefreshAsync();
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



                CodeSmells = new ObservableCollection<CodeSmellViewModel>();
                foreach(var codesmell in codeFragment.Model.CodeSmells)
                {
                    CodeSmells.Add(new CodeSmellViewModel(codesmell));
                }
            }
        }

        public async void Refresh()
        {
            await RefreshAsync();
        }

        public async Task RefreshAsync()
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

            UpdateCodeFragments();
        }

        private void UpdateCodeFragments()
        {
            CodeFragments = SortCodeFragments(CodeFragments);

            FlatList = GetLinkedFlatList(CodeFragments);

            var fragment = CodeFragments.FirstOrDefault();
            if (fragment != null)
                fragment.IsSelected = true;
        }

        private LinkedList<CodeFragmentViewModel> GetLinkedFlatList(ObservableCollection<CodeFragmentViewModel> codeFragments, LinkedList<CodeFragmentViewModel> linkedList = null)
        {
            if (linkedList == null)
                linkedList = new LinkedList<CodeFragmentViewModel>();

            //TODO: MOVE TO SEPARATE CLASS
            foreach (var codeFragment in codeFragments)
            {
                linkedList.AddLast(codeFragment);

                if (codeFragment.Children != null)
                    GetLinkedFlatList(codeFragment.Children, linkedList);
            }

            return linkedList;
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
            Dispatcher.CurrentDispatcher.Invoke(() => ParseInfoLines += DateTime.Now + " :: Info: " + line + "\n");
        }

    }
}
