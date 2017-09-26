using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Models
{
    class Statement
    {
        public string Text { get; private set; }

        public Statement(string text)
        {
            Text = text;
        }
    }
}
