using CodeSniffer.Interfaces;
using CodeSniffer.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Models
{
    public class MethodInvocation : ICodeFragment
    {
        public string Content { get; private set; }

        public string Name => "statement";

        public IList<IMetric> Metrics { get; set; }

        public IList<ICodeSmell> CodeSmells { get; set; }

        public IList<ICodeFragment> Children { get; set; }

        public string AccessedField { get; set; }
        public string DeclaredClass { get; set; }

        public MethodInvocation(string text)
        {
            Content = LineEndingConverter.ConvertToCRLF(text);
            AccessedField = getAccessedField(text);
        }

        private string getAccessedField(string text)
        {
            string fieldName = "";

            //To keep things simple, let's asssume field access start with get or set
            if ((text.StartsWith("get") || text.StartsWith("set")) && text.Length > 3)
            {
                fieldName = text.Substring(3);
            }
            else if (text.StartsWith("is") && text.Length > 2)
            {
                fieldName = text.Substring(2);
            }

            return fieldName;
        }

        public void WriteToTrainingSet()
        {
        }
    }
}
