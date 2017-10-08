using CodeSniffer.Interfaces;

namespace CodeSniffer.Models.CodeSmells
{
    class LongMethod : ICodeSmell
    {
        public string Name => "Long Method";

        public double Confidence { get; set; }
        public bool IsDetected { get; set; }
    }
}
