using CodeSniffer.Interfaces;
using CodeSniffer.Models.Metrics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeSniffer.BBN;

namespace CodeSniffer.Models.CodeSmells
{
    class LargeClass : ICodeSmell
    {

        private bool _isDetected;
        private IMetric _tcc;
        private IMetric _wmc;
        private IMetric _atfd;
        private IMetric _loc;
        private bool _overridden;
        private BBN.LargeClass _network;

        public event Action Updated;

        public LargeClass(IMetric tcc, IMetric wmc, IMetric atfd, IMetric loc )
        {
            _tcc = tcc;
            _wmc = wmc;
            _atfd = atfd;
            _loc = loc;

            _network = BBN.LargeClass.Instance;
        }

        public string Name => "Large_Class";

        public double Confidence
        {
            get
            {
                _network.SetEvidenceForAtfd(_atfd.Value);
                _network.SetEvidenceForLoc(_loc.Value);
                _network.SetEvidenceForTcc(_tcc.Value);
                _network.SetEvidenceForWmc(_wmc.Value);

                return Math.Round(_network.IsLargeClass() * 100, 2);
            }
            set
            {
            }
        }

        public bool IsDetected {
            get
            {
                if (!_overridden)
                {
                    _isDetected = DetectOnConfidence();
                }

                return _isDetected;
            }
            set
            {
                if(value != IsDetected)
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
