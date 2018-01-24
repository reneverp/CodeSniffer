using CodeSniffer.BBN.Discretization;
using System.IO;
using System.Reflection;

namespace CodeSniffer.BBN
{
    public enum Feature_Envy
    {
        TRUE,
        FALSE
    }

    public class FeatureEnvy
    {
        private BayesianNetwork _network;

        public FeatureEnvy()
        {
            string p = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            _network = new BayesianNetwork(p + @"\Networks\FeatureEnvy_network_naive.xdsl");
        }

        public void SetEvidenceForAtfd(double value)
        {
            var atfd = Discretizer.ATFD.Discretize(value);
            _network.SetEvidence("ATFD", atfd.ToString());
        }

        public void SetEvidenceForFdp(double value)
        {
            var fdp = Discretizer.FDP.Discretize(value);
            _network.SetEvidence("FDP", fdp.ToString());
        }

        public void SetEvidenceForLaa(double value)
        {
            var laa = Discretizer.LAA.Discretize(value);
            _network.SetEvidence("LAA", laa.ToString());
        }

        public double IsFeatureEnvy()
        {
            return _network.GetOutcomeValue("Feature_Envy", (int)Feature_Envy.TRUE);
        }
    }
}
