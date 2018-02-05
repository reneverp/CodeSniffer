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
        private bool _overridden;

        public event Action Updated;

        public FeatureEnvy(IMetric atfd, IMetric laa, IMetric fdp)
        {
            _atfd = atfd;
            _laa = laa;
            _fdp = fdp;
        }

        public string Name => "Feature_Envy";

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
