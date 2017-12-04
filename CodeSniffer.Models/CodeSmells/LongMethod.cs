using CodeSniffer.Interfaces;
using System;

namespace CodeSniffer.Models.CodeSmells
{
    class LongMethod : ICodeSmell
    {
        private static BBN.LongMethod _bbn;
        private bool _isDetected;
        private IMetric _loc;
        private IMetric _cyclo;
        private IMetric _maxnesting;
        private IMetric _noav;

        public LongMethod(IMetric loc, IMetric cyclo, IMetric maxnesting, IMetric noav)
        {
            _bbn = new BBN.LongMethod();

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
                _bbn.SetEvidenceForLoc(_loc.Value);
                _bbn.SetEvidenceForCyclo(_cyclo.Value);
                _bbn.SetEvidenceForMaxNesting(_maxnesting.Value);
                _bbn.SetEvidenceForNoav(_noav.Value);

                return Math.Round(_bbn.IsLongMethod() * 100, 2);
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
