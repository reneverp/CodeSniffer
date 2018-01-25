using CodeSniffer.BBN.Discretization;
using System.IO;
using System.Reflection;
using System;
using CodeSniffer.BBN.ParameterEstimation;
using System.Collections.Generic;

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
        private static LargeClass _instance;

        public static LargeClass Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LargeClass();
                }

                return _instance;
            }
        }

        private LargeClass()
        {
            string p = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _network = new BayesianNetwork(p + @"\Networks\LargeClass_network_naive.xdsl");

            Learn();

            _network.SaveNetwork();
        }

        private void Learn()
        {        
            IDictionary<string, DiscretizedData> map = GenerateBinMap();

            LaplaceEstimator.LaplaceEstimation(Discretizer.ClassDataset, _network, map, "Large_Class", 1);
        }

        private IDictionary<string, DiscretizedData> GenerateBinMap()
        {
            IDictionary<string, DiscretizedData> mapToReturn = new Dictionary<string, DiscretizedData>();

            mapToReturn.Add("LOC", Discretizer.LOCClass);
            mapToReturn.Add("ATFD", Discretizer.ATFDClass);
            mapToReturn.Add("TCC", Discretizer.TCC);
            mapToReturn.Add("WMC", Discretizer.WMC);

            return mapToReturn;
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
