using System.Collections.Generic;

namespace CodeSniffer.Models
{
    class Class
    {
        private IList<Method> _methodList;

        public int LinesOfCode { get; private set; }

        public string Text { get; private set; }

        public int NumberOfMethods
        {
            get
            {
                return _methodList.Count;
            }
        }

        public Class(int linesOfCode, string text)
        {
            LinesOfCode = linesOfCode;
            Text = text;
            _methodList = new List<Method>();
        }

        public void AddMethod(Method method)
        {
            _methodList.Add(method);
        }


    }
}
