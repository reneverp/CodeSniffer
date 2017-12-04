using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeSniffer.BBN.Discretization;

namespace CodeSniffer.BBN
{
    public enum Large_Class
    {
        TRUE,
        FALSE
    }

    public class LargeClass
    {
        private BayesianNetwork _network;

        public LargeClass()
        {
            _network = new BayesianNetwork(@"Networks\LargeClass_Network.xdsl");

        }

        public void SetEvidenceForLoc(double value)
        {
            LOC loc = LinesOfCodeclass.Discretize(value);
            _network.SetEvidence("LOC", (int)loc);
        }

        public void SetEvidenceForAtfd(double value)
        {
            ATFD atfd = AccessToForeignDataClass.Discretize(value);
            _network.SetEvidence("ATFD", (int)atfd);
        }

        public void SetEvidenceForTcc(double value)
        {
            TCC tcc = TightClassCohesion.Discretize(value);
            _network.SetEvidence("TCC", (int)tcc);
        }

        public void SetEvidenceForWmc(double value)
        {
            WMC wmc = WeightedMethodCount.Discretize(value);
            _network.SetEvidence("WMC", (int)wmc);
        }

        public double IsLargeClass()
        {
            return _network.GetOutcomeValue("Large_Class", (int)Large_Class.TRUE);
        }
    }
}
