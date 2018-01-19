using Smile;
using Smile.Learning;
using System.Threading;

namespace CodeSniffer.BBN
{
    class BayesianNetwork
    {
        private Network _network;

        public BayesianNetwork(string networkFile)
        {
            SmileLicense.ActivateLicense();
            LoadNetworkFromFile(networkFile);
        }

        public void SetEvidence(string nodeId, int outcomeIndex)
        {
            int handle = _network.GetNode(nodeId);
            _network.SetEvidence(handle, outcomeIndex);
        }

        public double GetOutcomeValue(string nodeId, int outcomeIndex)
        {
            _network.UpdateBeliefs();

            int handle = _network.GetNode(nodeId);
            var results = _network.GetNodeValue(handle);

            return results[outcomeIndex];
        }

        private void LoadNetworkFromFile(string networkFile)
        {
            try
            {
                _network = new Network();
                _network.ReadFile(networkFile);
            }
            catch(SmileException)
            {
                Thread.Sleep(1000);
                LoadNetworkFromFile(networkFile);
            }
        }

        private void Learn()
        {
            Smile.Learning.EM a = new Smile.Learning.EM();
            ///a.Learn(data, _network, match);
            ///

             
            
        }
    }
}
