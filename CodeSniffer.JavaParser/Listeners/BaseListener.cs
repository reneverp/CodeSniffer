using CodeSniffer.Interfaces;
using System;

namespace CodeSniffer.Listeners
{
    public class BaseListener : JavaBaseListener, IListener
    {
        public event Action<string> ParseInfoUpdate;

        public void InvokeParseInfoUpdate(string info)
        {
            ParseInfoUpdate?.Invoke(info);
        }
    }
}
