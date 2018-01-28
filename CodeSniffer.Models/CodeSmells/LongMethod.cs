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
        private bool _overridden;

        public event Action Updated;

        public LongMethod(IMetric loc, IMetric cyclo, IMetric maxnesting, IMetric noav)
        {
            _loc = loc;
            _cyclo = cyclo;
            _maxnesting = maxnesting;
            _noav = noav;
        }

        public string Name => "Long_Method";

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
                if(!_overridden)
                {
                    _isDetected = DetectOnConfidence();
                }

                return _isDetected;
            }
            set
            {
                if (value != IsDetected)
                {
                    _isDetected = value;
                    _overridden = true;
                    Updated?.Invoke();
                }
            }
        }

        private bool DetectOnConfidence()
        {
            bool detected = false;

            if (Confidence > 50)
            {
                detected = true;
            }
            else
            {
                detected = false;
            }

            return detected;
        }
    }
}
