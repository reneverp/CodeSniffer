using CodeSniffer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Models.CodeSmells
{
    class LargeClass : ICodeSmell
    {
        public string Name => "Large Class";

        public double Confidence { get; set; }

        public bool IsDetected { get; set; }
    }
}
