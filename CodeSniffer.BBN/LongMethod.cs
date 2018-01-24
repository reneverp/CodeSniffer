using CodeSniffer.BBN.Discretization;
using System.IO;
using System.Reflection;

namespace CodeSniffer.BBN
{
    public enum Long_Method
    {
        TRUE,
        FALSE
    }

    public class LongMethod
    {
        private BayesianNetwork _network;

        public LongMethod()
        {
            string p = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _network = new BayesianNetwork(p + @"\Networks\LongMethod_network_naive.xdsl");
        }

        public void SetEvidenceForLoc(double value)
        {
            var loc = Discretizer.LOC.Discretize(value);
            _network.SetEvidence("loc", loc.ToString());
        }

        public void SetEvidenceForCyclo(double value)
        {
            var cyclo = Discretizer.CYCLO.Discretize(value);
            _network.SetEvidence("cyclo", cyclo.ToString());
        }

        public void SetEvidenceForMaxNesting(double value)
        {
            var maxnesting = Discretizer.MAXNESTING.Discretize(value);
            _network.SetEvidence("maxnesting", maxnesting.ToString());
        }

        public void SetEvidenceForNoav(double value)
        {
            var noav = Discretizer.NOAV.Discretize(value);
            _network.SetEvidence("noav", noav.ToString());
        }

        public double IsLongMethod()
        {
            //TODO: USE DESCRIPTIVE NAMING
            return _network.GetOutcomeValue("long_method", (int)Long_Method.TRUE);
        }
    }
}
