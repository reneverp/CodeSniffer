using CodeSniffer.Interfaces;

namespace CodeSniffer.ViewModels
{
    class MetricViewModel : ViewModelBase
    {
        private string _name;
        private double _value;

        public string Name {
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

        public MetricViewModel(IMetric metric)
        {
            Name = metric.Name;
            Value = metric.Calculate();
        }
    }
}
