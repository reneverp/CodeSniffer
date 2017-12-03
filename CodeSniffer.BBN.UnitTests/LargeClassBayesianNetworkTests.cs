using NUnit.Framework;

namespace CodeSniffer.BBN.UnitTests
{
    [TestFixture]
    public class LargeClassBayesianNetworkTests
    {
        [Test]
        public void Test()
        { 
            var nw = new LargeClass();

            //above 350 lines should end up in > 85% score according to initial learning
            nw.SetEvidenceForLoc(350);

            Assert.GreaterOrEqual(nw.IsLargeClass(), 0.85);
        }
    }
}
