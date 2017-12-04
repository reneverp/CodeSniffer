using CodeSniffer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Models.CodeSmells
{
    class FeatureEnvy : ICodeSmell
    {
        private static BBN.FeatureEnvy _bbn;
        private bool _isDetected;
        private IMetric _atfd;
        private IMetric _laa;
        private IMetric _fdp;

        public FeatureEnvy(IMetric atfd, IMetric laa, IMetric fdp)
        {
            _bbn = new BBN.FeatureEnvy();

            _atfd = atfd;
            _laa = laa;
            _fdp = fdp;
        }

        public string Name => "Feature Envy";

        public double Confidence
        {
            get
            {
                _bbn.SetEvidenceForAtfd(_atfd.Value);
                _bbn.SetEvidenceForLaa(_laa.Value);
                _bbn.SetEvidenceForFdp(_fdp.Value);

                return Math.Round(_bbn.IsFeatureEnvy() * 100, 2);
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
