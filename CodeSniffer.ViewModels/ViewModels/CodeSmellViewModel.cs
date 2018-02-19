using CodeSniffer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CodeSniffer.ViewModels
{
    public class CodeSmellViewModel : ViewModelBase
    {
        private string _name;
        private double _value;

        private ICodeSmell _codeSmell;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }

        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsDetected
        {
            get { return _codeSmell.IsDetected; }
            set
            {
                _codeSmell.IsDetected = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand CodeSmellDetectedCommand { get; private set; }

        public CodeSmellViewModel(ICodeSmell codeSmell)
        {
            Name = codeSmell.Name;
            Value = codeSmell.Confidence;

            _codeSmell = codeSmell;
        }
    }
}
