using CodeSniffer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeSniffer.Models.Metrics
{
    //Locality of attribute access
    class NOAV : IMetric
    {
        private IList<MethodInvocation> _totalFieldAccessInvocations;

        public string Name
        {
            get { return "NOAV"; }
            set { }
        }

        private double _value = -1;
        private IList<string> _parameters;
        private Class _class;
        private string _methodContent;
        private IList<string> _localVariableDeclarations;

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

        public NOAV(Class parentClass, IList<string> parameters, IList<string> localVariableDeclarations, IList<MethodInvocation> totalFieldAccessInvocations, string methodContent)
        {
            _totalFieldAccessInvocations = totalFieldAccessInvocations;
            _parameters = parameters;
            _class = parentClass;
            _methodContent = methodContent;
            _localVariableDeclarations = localVariableDeclarations;
        }

        public double Calculate()
        {
            IList<string> variablesAccessed = new List<string>();

            foreach (var invocation in _totalFieldAccessInvocations)
            {
                if (!string.IsNullOrEmpty(invocation.AccessedField) && !variablesAccessed.Contains(invocation.AccessedField))
                {
                    variablesAccessed.Add(invocation.AccessedField);
                }
            }


            int accessFields = variablesAccessed.Count;
            int nVar = FindInstanceVariablesAccessedCount(_class.InstanceVariables, _methodContent) + _localVariableDeclarations.Count;
            int nParams = _parameters.Count;
            return accessFields + nVar + nParams;
        }

        public static int FindInstanceVariablesAccessedCount(IList<string> instanceVariables, string methodContent)
        {
            string variableNames = "";

            foreach(var instanceVariable in instanceVariables)
            {
                variableNames += instanceVariable;

                if(instanceVariable != instanceVariables.Last())
                {
                    variableNames += "|";
                }
            }

            if (!string.IsNullOrEmpty(variableNames))
            {
                var matches = Regex.Matches(methodContent, @"\b(" + variableNames + @")\b(?![\s\S] *\b\1\b)");

                return matches.Count;
            }

            return 0;
        }
    }
}
