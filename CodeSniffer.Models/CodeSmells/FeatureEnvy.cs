using CodeSniffer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Models.CodeSmells
{
    class FeatureEnvy : ICodeSmell
    {
        public string Name => "Feature Envy";

        public double Confidence { get; set; }

        public bool IsDetected { get; set; }
    }
}
