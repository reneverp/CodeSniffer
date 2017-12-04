using CodeSniffer.BBN.Discretization;

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
            _network = new BayesianNetwork(@"Networks\FeatureEnvy_Network.xdsl");

        }

        public void SetEvidenceForAtfd(double value)
        {
            ATFD_METHOD atfd = AccessToForeignDataMethod.Discretize(value);
            _network.SetEvidence("ATFD", (int)atfd);
        }

        public void SetEvidenceForFdp(double value)
        {
            FDP fdp = ForeignDataProviders.Discretize(value);
            _network.SetEvidence("FDP", (int)fdp);
        }

        public void SetEvidenceForLaa(double value)
        {
            LAA laa = LocalityOfAttributeAccesses.Discretize(value);
            _network.SetEvidence("LAA", (int)laa);
        }

        public double IsFeatureEnvy()
        {
            return _network.GetOutcomeValue("Feature_Envy", (int)Feature_Envy.TRUE);
        }
    }
}
