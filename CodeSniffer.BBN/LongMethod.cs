using CodeSniffer.BBN.Discretization;
using CodeSniffer.BBN.ParameterEstimation;
using System.Collections.Generic;
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
        private static LongMethod _instance;

        public static LongMethod Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LongMethod();
                }

                return _instance;
            }
        }

        private LongMethod()
        {
            string p = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _network = new BayesianNetwork(p + @"\Networks\LongMethod_network_naive.xdsl");

            Learn();

            _network.SaveNetwork();
        }

        private void Learn()
        {
            IDictionary<string, DiscretizedData> map = GenerateBinMap();

            LaplaceEstimator.LaplaceEstimation(Discretizer.MethodDataset, _network, map, "Long_Method", 1);

            LaplaceEstimator.Adapt(Discretizer.ProcessAdditionalMethodCases(), Discretizer.MethodDataset, _network, map, "Long_Method", 1, 1);
        }

        private IDictionary<string, DiscretizedData> GenerateBinMap()
        {
            IDictionary<string, DiscretizedData> mapToReturn = new Dictionary<string, DiscretizedData>();

            mapToReturn.Add("LOC", Discretizer.LOC);
            mapToReturn.Add("CYCLO", Discretizer.CYCLO);
            mapToReturn.Add("MAXNESTING", Discretizer.MAXNESTING);
            mapToReturn.Add("NOAV", Discretizer.NOAV);

            return mapToReturn;
        }

        public void SetEvidenceForLoc(double value)
        {
            var loc = Discretizer.LOC.Discretize(value);
            _network.SetEvidence("LOC", loc.ToString());
        }

        public void SetEvidenceForCyclo(double value)
        {
            var cyclo = Discretizer.CYCLO.Discretize(value);
            _network.SetEvidence("CYCLO", cyclo.ToString());
        }

        public void SetEvidenceForMaxNesting(double value)
        {
            var maxnesting = Discretizer.MAXNESTING.Discretize(value);
            _network.SetEvidence("MAXNESTING", maxnesting.ToString());
        }

        public void SetEvidenceForNoav(double value)
        {
            var noav = Discretizer.NOAV.Discretize(value);
            _network.SetEvidence("NOAV", noav.ToString());
        }

        public double IsLongMethod()
        {
            //TODO: USE DESCRIPTIVE NAMING
            return _network.GetOutcomeValue("Long_Method", (int)Long_Method.TRUE);
        }
    }
}
