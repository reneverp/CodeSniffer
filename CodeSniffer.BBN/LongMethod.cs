using CodeSniffer.BBN.Discretization;

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

        public LongMethod()
        {
            _network = new BayesianNetwork(@"Networks\LongMethod_Network_naive.xdsl");

        }

        public void SetEvidenceForLoc(double value)
        {
            LOC_METHOD loc = LinesOfCodeMethod.Discretize(value);
            _network.SetEvidence("loc", (int)loc);
        }

        public void SetEvidenceForCyclo(double value)
        {
            CYCLO cyclo = CyclometicComplexity.Discretize(value);
            _network.SetEvidence("cyclo", (int)cyclo);
        }

        public void SetEvidenceForMaxNesting(double value)
        {
            MAXNESTING maxnesting = MaximumNesting.Discretize(value);
            _network.SetEvidence("maxnesting", (int)maxnesting);
        }

        public void SetEvidenceForNoav(double value)
        {
            NOAV noav = NumberOfAccessedVariables.Discretize(value);
            _network.SetEvidence("noav", (int)noav);
        }

        public double IsLongMethod()
        {
            //TODO: USE DESCRIPTIVE NAMING
            return _network.GetOutcomeValue("long_method", (int)Long_Method.TRUE);
        }
    }
}
