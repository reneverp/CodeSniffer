using Smile;

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
            _network = new Network();
            _network.ReadFile(networkFile);
        }
    }
}
