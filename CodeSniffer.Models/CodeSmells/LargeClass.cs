using CodeSniffer.Interfaces;
using CodeSniffer.Models.Metrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Models.CodeSmells
{
    class LargeClass : ICodeSmell
    {

        private static BBN.LargeClass _bbn;
        private bool _isDetected;
        private IMetric _tcc;
        private IMetric _wmc;
        private IMetric _atfd;
        private IMetric _loc;

        public LargeClass(IMetric tcc, IMetric wmc, IMetric atfd, IMetric loc )
        {
            _bbn = new BBN.LargeClass();

            _tcc = tcc;
            _wmc = wmc;
            _atfd = atfd;
            _loc = loc;
        }

        public string Name => "Large Class";

        public double Confidence
        {
            get
            {
                _bbn.SetEvidenceForAtfd(_atfd.Value);
                _bbn.SetEvidenceForLoc(_loc.Value);
                _bbn.SetEvidenceForTcc(_tcc.Value);
                _bbn.SetEvidenceForWmc(_wmc.Value);

                return Math.Round(_bbn.IsLargeClass() * 100, 2);
            }
            set
            {
            }
        }

        public bool IsDetected {
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
