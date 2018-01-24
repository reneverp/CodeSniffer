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

        public void SetEvidence(string nodeId, string outcomeId)
        {
            int handle = _network.GetNode(nodeId);
            _network.SetEvidence(handle, outcomeId);
        }

        public double GetOutcomeValue(string nodeId, int outcomeIndex)
        {
            _network.UpdateBeliefs();

            int handle = _network.GetNode(nodeId);
            var results = _network.GetNodeValue(handle);

            return results[outcomeIndex];
        }

        private void LoadNetworkFromFile(string networkFile, int retryCount = 0)
        {
            try
            {
                _network = new Network();
                _network.ReadFile(networkFile);
            }
            catch(SmileException e)
            {
                Thread.Sleep(1000);
                if (retryCount < 5)
                {
                    LoadNetworkFromFile(networkFile, ++retryCount);
                }
                else
                {
                    throw e;
                }
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
