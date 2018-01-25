using CodeSniffer.Interfaces;
using System;

namespace CodeSniffer.Models.CodeSmells
{
    class LongMethod : ICodeSmell
    {
        private bool _isDetected;
        private IMetric _loc;
        private IMetric _cyclo;
        private IMetric _maxnesting;
        private IMetric _noav;

        public LongMethod(IMetric loc, IMetric cyclo, IMetric maxnesting, IMetric noav)
        {
            _loc = loc;
            _cyclo = cyclo;
            _maxnesting = maxnesting;
            _noav = noav;
        }

        public string Name => "Long Method";

        public double Confidence
        {
            get
            {
                BBN.LongMethod.Instance.SetEvidenceForLoc(_loc.Value);
                BBN.LongMethod.Instance.SetEvidenceForCyclo(_cyclo.Value);
                BBN.LongMethod.Instance.SetEvidenceForMaxNesting(_maxnesting.Value);
                BBN.LongMethod.Instance.SetEvidenceForNoav(_noav.Value);

                return Math.Round(BBN.LongMethod.Instance.IsLongMethod() * 100, 2);
            }
            set
            {
            }
        }

        public bool IsDetected
        {
            get
            {
                if (Confidence > 50)
                {
                    _isDetected = true;
                }

                return _isDetected;
            }
            set
            {
                _isDetected = value;
            }
        }
    }
}
