using CodeSniffer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Models.Metrics
{
    //Foreign Data Providers
    class FDP : IMetric
    {
        private IList<MethodInvocation> _outerFieldAccessInvocations;

        public string Name
        {
            get { return "FDP"; }
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

        public FDP(IList<MethodInvocation> outerFieldAccessInvocations)
        {
            _outerFieldAccessInvocations = outerFieldAccessInvocations;        
        }

        public double Calculate()
        {
            IList<string> results = new List<string>();

            _outerFieldAccessInvocations.ToList().ForEach(x =>
                                {
                                    if(x.DeclaredClass != null && !results.Contains(x.DeclaredClass))
                                    {
                                        results.Add(x.DeclaredClass);
                                    }
                                }
                            );

            return results.Count();
        }
    }
}
