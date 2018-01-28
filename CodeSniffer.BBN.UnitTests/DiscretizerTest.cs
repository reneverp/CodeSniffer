using CodeSniffer.BBN.Discretization;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.BBN.UnitTests
{
    [TestFixture]
    public class DiscretizerTest
    {
        [Test]
        public void DiscretizationTest()
        {
            Discretizer.DiscretizeTrainingSets();

            Discretizer.ProcessAdditionalMethodCases();

            Discretizer.ProcessAdditionalClassCases();

        }
    }
}
