using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Interfaces
{
    public interface ICodeFragment
    {
        string Content { get; }
        string Name { get; }

        IList<ICodeFragment> Children { get; }
    }
}
