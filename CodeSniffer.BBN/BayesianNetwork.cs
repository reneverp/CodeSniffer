using CodeSniffer.BBN.Discretization;
using Smile;
using Smile.Learning;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace CodeSniffer.BBN
{
    public class BayesianNetwork
    {
        private Network _network;
        private string _networkFile;

        public BayesianNetwork(string networkFile)
        {

            SmileLicense.ActivateLicense();
            LoadNetworkFromFile(networkFile);

            _networkFile = networkFile;

            if (!Discretizer.IsDiscretized)
            {
                Discretizer.DiscretizeTrainingSets();
            }
        }

        ~BayesianNetwork()
        {
            _network = null;
        }

        public double[] GetProbabilities(string nodeId)
        {
            int handle = _network.GetNode(nodeId);
            return _network.GetNodeDefinition(handle);
        }

        public void SetProbabilities(string nodeId, double[] definition)
        {
            int handle = _network.GetNode(nodeId);
            _network.SetNodeDefinition(handle, definition);
        }

        public void SetEvidence(string nodeId, string outcomeId)
        {
            int handle = _network.GetNode(nodeId);
            _network.SetEvidence(handle, outcomeId);
        }

        public void SetOutcomeIds(string nodeId, IList<Bin> descriptions)
        {
            for(int i = 0; i < descriptions.Count; i++)
            {
                _network.SetOutcomeId(nodeId, i, descriptions[i].ToString());
            }
        }

        public double GetOutcomeValue(string nodeId, int outcomeIndex)
        {
            if (System.Configuration.ConfigurationManager.AppSettings["TrainingMode"] == "True")
                return 0.0;

            _network.UpdateBeliefs();

            int handle = _network.GetNode(nodeId);
            var results = _network.GetNodeValue(handle);

            return results[outcomeIndex];
        }

        private void LoadNetworkFromFile(string networkFile, int retryCount = 0)
        {
            _network = new Network();
            _network.ReadFile(networkFile);
        }

        public void SaveNetwork()
        {
            _network.WriteFile(_networkFile);
        }

        public IDictionary<string, IList<double>> GetSampleCounts()
        {
            var counts = new Dictionary<string, IList<double>>();

            var jsonFile = Path.GetDirectoryName(_networkFile) + "\\" + Path.GetFileNameWithoutExtension(_networkFile) + ".json";

            if(File.Exists(jsonFile))
            {
                var json = File.ReadAllText(jsonFile);
                counts = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, IList<double>>>(json);
            }

            return counts;
        }

        public void SetSampleCounts(IDictionary<string, IList<double>> counts)
        {
            var jsonFile = Path.GetDirectoryName(_networkFile) + "\\" + Path.GetFileNameWithoutExtension(_networkFile) + ".json";

            using (var sw = new StreamWriter(jsonFile))
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(counts);
                sw.Write(json);
            }
        }
    }
}
