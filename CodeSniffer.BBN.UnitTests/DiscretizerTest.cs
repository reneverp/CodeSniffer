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
        //    EFDataSet ef = new EFDataSet(); 
        //    ef.Load(GetFullPath("ClassTrainingSet_1356_03122017_withoutOutlier.csv"));

        //    var LOCClass = ef.Discretize<int>(0, 8);
        //    var TCC = new DiscretizedData(ef.Discretize<double>(2, 8));
        //    var WMC = new DiscretizedData(ef.Discretize<int>(3, 8));
        //    var ATFDClass = new DiscretizedData(ef.Discretize<int>(4, 8));

        //    var dis = ef.GetDiscreteDataSet();

        //    foreach(DataRow row in dis.Tables[0].Rows)
        //    {
        //        string rowToWrite = "";
        //        for (int i = 0; i < row.ItemArray.Count(); i++)
        //        {
        //            rowToWrite += row.ItemArray[i].ToString() + "    |    ";
        //        }

        //        Debug.WriteLine(rowToWrite);
        //    }

        //    var rows = dis.Tables[0].Select().OrderBy(x => x.Field<string>(0));

        //    //LAPLACE ESTIMATION SMOOTHING EXAMPLE
        //    var countTrueOnly = rows.Where(x => x.Field<string>("Large_Class") == "True").Count();
        //    var countFalseOnly = rows.Where(x => x.Field<string>("Large_Class") == "False").Count();

        //    int countTrue = countTrueOnly + 1;
        //    int countFalse = countFalseOnly + 1;

        //    double probFalse = (double)(countFalse) / ((double)(rows.Count()) + (1 * 2));
        //    double probTrue = (double)(countTrue) / ((double)(rows.Count()) + (1 * 2));

        //    foreach (var bin in LOCClass)
        //    {
        //        var rowsTrue = rows.Where(x => x.Field<string>("LOC") == bin.ToString() && x.Field<string>("Large_Class") == "True");
        //        var rowsFalse = rows.Where(x => x.Field<string>("LOC") == bin.ToString() && x.Field<string>("Large_Class") == "False");

        //        countTrue = rowsTrue.Count() + 1;
        //        countFalse = rowsFalse.Count() + 1;

        //        probFalse = (double)(countFalse) / ((double)(countFalseOnly) + (1 * LOCClass.Count));
        //        probTrue = (double)(countTrue) / ((double)(countTrueOnly) + (1 * LOCClass.Count));

        //        Debug.WriteLine(probFalse + "         ,            " + probTrue);
        //    }

        //    Assert.That(true == true);
        //}

        //private string GetFullPath(string file)
        //{
        //    string p = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        //    return p + @"\TrainingsData\" + file;
        }
    }
}
