using CodeSniffer.Models;

namespace CodeSniffer.Interfaces
{
    public interface IParser
    {
        void Parse(string filename, Project project);
    }
}
