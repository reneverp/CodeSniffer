using System.Collections.Generic;

namespace CodeSniffer.Models
{
    class Class
    {
        public IList<Method> Methods { get; private set; }

        public int LinesOfCode { get; private set; }

        public string Text { get; private set; }

        public int NumberOfMethods
        {
            get
            {
                return Methods.Count;
            }
        }

        public Class(int linesOfCode, string text)
        {
            LinesOfCode = linesOfCode;
            Text = text;
            Methods = new List<Method>();
        }

        public void AddMethod(Method method)
        {
            Methods.Add(method);
        }


    }
}
