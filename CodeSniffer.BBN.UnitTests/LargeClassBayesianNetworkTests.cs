using NUnit.Framework;

namespace CodeSniffer.BBN.UnitTests
{
    [TestFixture]
    public class LargeClassBayesianNetworkTests
    {
        [Test]
        public void TestBayesian()
        {
            var lm = BBN.LongMethod.Instance;
            var fe = BBN.FeatureEnvy.Instance;

            var nw = BBN.LargeClass.Instance;

            //above 350 lines should end up in > 85% score according to initial learning
            nw.SetEvidenceForLoc(350);

            Assert.GreaterOrEqual(nw.IsLargeClass(), 0.85);
        }
    }
}
