using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeSniffer.BBN.UnitTests
{
    [TestFixture]
    public class DiscretizationTest
    {
        [Test]
        public void Test()
        {
            Discretization.EFDataSet a = new Discretization.EFDataSet();

            a.Load(@"C:\Temp\ClassTrainingSet_1356_03122017_withoutOutlier.csv");

            var loc = a.Discretize<int>(0, 8);
            var tcc = a.Discretize<double>(2, 8);
            var wmc = a.Discretize<int>(3, 8);
            var atfd = a.Discretize<int>(4, 8);


            a.WriteToCsv(@"C:\Temp\out_test.csv");

        }
    }
}
