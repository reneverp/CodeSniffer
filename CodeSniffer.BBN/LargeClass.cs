using CodeSniffer.BBN.Discretization;
using System.IO;
using System.Reflection;

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
            string p = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _network = new BayesianNetwork(p + @"\Networks\LargeClass_network_naive.xdsl");
        }

        public void SetEvidenceForLoc(double value)
        {
            var loc = Discretizer.LOCClass.Discretize(value);
            _network.SetEvidence("LOC", loc.ToString());
        }

        public void SetEvidenceForAtfd(double value)
        {
            var atfd = Discretizer.ATFDClass.Discretize(value);
            _network.SetEvidence("ATFD", atfd.ToString());
        }

        public void SetEvidenceForTcc(double value)
        {
            var tcc = Discretizer.TCC.Discretize(value);
            _network.SetEvidence("TCC", tcc.ToString());
        }

        public void SetEvidenceForWmc(double value)
        {
            var wmc = Discretizer.WMC.Discretize(value);
            _network.SetEvidence("WMC", wmc.ToString());
        }

        public double IsLargeClass()
        {
            return _network.GetOutcomeValue("Large_Class", (int)Large_Class.TRUE);
        }
    }
}
