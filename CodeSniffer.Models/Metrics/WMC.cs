using CodeSniffer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Models.Metrics
{
    //Foreign Data Providers
    class WMC : IMetric
    {
        IList<Method> _methods;

        public string Name
        {
            get { return "WMC"; }
            set { }
        }

        private double _value = -1;

        public double Value
        {
            get
            {
                if (_value == -1)
                {
                    _value = Calculate();
                }

                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public WMC(IList<Method> methods)
        {
            _methods = methods;
        }

        public double Calculate()
        {
            double wmc = 0;

            _methods.ToList().ForEach(x =>
                                {
                                    if (x.Content != null)
                                    {
                                        var metric = new CyclometicComplexity(x.Content);

                                        wmc = metric.Calculate();
                                    }
                                }
             );

            return wmc;
        }
    }
}
