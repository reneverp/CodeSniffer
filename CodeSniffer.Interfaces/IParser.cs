using System;

namespace CodeSniffer.Interfaces
{
    public interface IParser
    {
        event Action<string> NotifyParseInfoUpdated;

        void Parse(string filename, IProject project);
    }
}
