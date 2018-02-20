using CodeSniffer.BBN.Discretization;
using CodeSniffer.BBN.ParameterEstimation;
using System.Collections.Generic;
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
        private static FeatureEnvy _instance;

        private static object _lockObj = new object();

        public static FeatureEnvy Instance
        {
            get
            {
                lock (_lockObj)
                {
                    if (_instance == null)
                    {
                        _instance = new FeatureEnvy();
                    }

                    return _instance;
                }
            }
        }

        private FeatureEnvy()
        {
            string p = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            _network = new BayesianNetwork(p + @"\Networks\FeatureEnvy_network_naive.xdsl");
            Learn();

            _network.SaveNetwork();
        }

        private void Learn()
        {
            IDictionary<string, DiscretizedData> map = GenerateBinMap();

            SetOutcomeIds();

            //learn from original data
            LaplaceEstimator.LaplaceEstimation(Discretizer.MethodDataset, _network, map, "Feature_Envy", 1);

            //Adapt to additional data
            var cases = Discretizer.ProcessAdditionalMethodCases();
            if (cases.Count > 0)
            {
                //calculate the fading factor according the samplesize of the trainingset:
                double originalCount = Discretizer.MethodDataset.Tables[0].Rows.Count;
                double q = (originalCount - cases.Count) / originalCount;

                LaplaceEstimator.Adapt(cases, Discretizer.MethodDataset, _network, map, "Feature_Envy", 1, q);
            }

        }

        private void SetOutcomeIds()
        {
            _network.SetOutcomeIds("ATFD", Discretizer.ATFD.Bins);
            _network.SetOutcomeIds("LAA", Discretizer.LAA.Bins);
            _network.SetOutcomeIds("FDP", Discretizer.FDP.Bins);
        }

        private IDictionary<string, DiscretizedData> GenerateBinMap()
        {
            IDictionary<string, DiscretizedData> mapToReturn = new Dictionary<string, DiscretizedData>();

            mapToReturn.Add("ATFD", Discretizer.ATFD);
            mapToReturn.Add("LAA", Discretizer.LAA);
            mapToReturn.Add("FDP", Discretizer.FDP);

            return mapToReturn;
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
