using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Interfaces
{
    public interface ICodeSmell
    {
        string Name { get; }
        
        double Confidence { get; set; }

        bool IsDetected { get; set; }
    }
}
