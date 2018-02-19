using CodeSniffer.Interfaces;
using System;
using CodeSniffer.BBN;

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
        private BBN.LongMethod _network;

        public event Action Updated;

        public LongMethod(IMetric loc, IMetric cyclo, IMetric maxnesting, IMetric noav)
        {
            _loc = loc;
            _cyclo = cyclo;
            _maxnesting = maxnesting;
            _noav = noav;

            _network = BBN.LongMethod.Instance;
        }

        public string Name => "Long_Method";

        public double Confidence
        {
            get
            {
                _network.SetEvidenceForLoc(_loc.Value);
                _network.SetEvidenceForCyclo(_cyclo.Value);
                _network.SetEvidenceForMaxNesting(_maxnesting.Value);
                _network.SetEvidenceForNoav(_noav.Value);

                return Math.Round(_network.IsLongMethod() * 100, 2);
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
