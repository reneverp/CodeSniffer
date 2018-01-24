using CodeSniffer.BBN.Discretization;
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

            var locClass = new DiscretizedData(a.Discretize<int>(0, 8));
            var tcc = new DiscretizedData(a.Discretize<double>(2, 8));
            var wmc = new DiscretizedData(a.Discretize<int>(3, 8));
            var atfdClass = new DiscretizedData(a.Discretize<int>(4, 8));

            a.WriteToCsv(@"C:\Temp\outClass_test.csv");

            a.Load(@"C:\Temp\MethodTrainingSet_2204_30112017_withoutOutlier.csv");

            var loc = new DiscretizedData(a.Discretize<int>(0, 8));
            var cyclo = new DiscretizedData(a.Discretize<int>(1, 8));
            var atfd = new DiscretizedData(a.Discretize<int>(5, 8));
            var fdp = new DiscretizedData(a.Discretize<int>(6, 8));
            var laa = new DiscretizedData(a.Discretize<double>(7, 8));
            var maxnesting = new DiscretizedData(a.Discretize<int>(8, 8));
            var noav = new DiscretizedData(a.Discretize<int>(9, 8));


            a.WriteToCsv(@"C:\Temp\outMethod_test.csv");

        }
    }
}
