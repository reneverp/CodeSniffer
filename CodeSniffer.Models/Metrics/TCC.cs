using CodeSniffer.Interfaces;
using System.Collections.Generic;
using System.Linq;
namespace CodeSniffer.Models.Metrics
{
    //Foreign Data Providers
    class TCC : IMetric
    {
        public string Name
        {
            get { return "TCC"; }
            set { }
        }

        private double _value = -1;
        private IList<Method> _methods;

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

        public TCC(IList<Method> methods)
        {
            _methods = methods;
        }

        public double Calculate()
        {
            int n = _methods.Count;
            int npc = (n * (n - 1)) / 2;
            int ndc = 0;

            List<List<string>> accessedFieldsByMethods = new List<List<string>>();
            foreach (var method in _methods)
                accessedFieldsByMethods.Add(processAccessedFields(method));

            for (int i = 0; i < accessedFieldsByMethods.Count; i++)
                for (int j = i + 1; j < accessedFieldsByMethods.Count; j++)
                    if (isConnected(accessedFieldsByMethods[i], accessedFieldsByMethods[j]))
                        ndc++;

            float result = npc > 0 ? ndc * 1.0f / npc : 0;
            return System.Math.Round(result, 2);
        }

        public List<string> processAccessedFields(Method method)
        {
            List<string> variablesAccessed = new List<string>();

            foreach (var invocation in method.InnerDataAccessInvocations)
            {
                if (!string.IsNullOrEmpty(invocation.AccessedField) && !variablesAccessed.Contains(invocation.AccessedField))
                {
                    variablesAccessed.Add(invocation.AccessedField);
                }
            }
            return variablesAccessed;
        }

        private bool isConnected(List<string> method1, List<string> method2)
        {
            foreach (string field in method1)
                if (method2.Contains(field))
                    return true;
            return false;
        }
    }
}
