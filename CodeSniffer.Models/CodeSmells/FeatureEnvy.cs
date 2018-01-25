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
        private bool _isDetected;
        private IMetric _atfd;
        private IMetric _laa;
        private IMetric _fdp;

        public FeatureEnvy(IMetric atfd, IMetric laa, IMetric fdp)
        {
            _atfd = atfd;
            _laa = laa;
            _fdp = fdp;
        }

        public string Name => "Feature Envy";

        public double Confidence
        {
            get
            {
                BBN.FeatureEnvy.Instance.SetEvidenceForAtfd(_atfd.Value);
                BBN.FeatureEnvy.Instance.SetEvidenceForLaa(_laa.Value);
                BBN.FeatureEnvy.Instance.SetEvidenceForFdp(_fdp.Value);

                return Math.Round(BBN.FeatureEnvy.Instance.IsFeatureEnvy() * 100, 2);
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
