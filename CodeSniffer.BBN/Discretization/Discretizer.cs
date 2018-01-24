using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.BBN.Discretization
{
    public static class Discretizer
    {
        private static EFDataSet _ef;

        public static DiscretizedData LOCClass { get; private set; }
        public static DiscretizedData TCC { get; private set; }
        public static DiscretizedData WMC { get; private set; }
        public static DiscretizedData ATFDClass { get; private set; }

        public static DiscretizedData LOC { get; private set; }
        public static DiscretizedData CYCLO { get; private set; }
        public static DiscretizedData ATFD { get; private set; }
        public static DiscretizedData FDP { get; private set; }
        public static DiscretizedData LAA { get; private set; }
        public static DiscretizedData MAXNESTING { get; private set; }
        public static DiscretizedData NOAV { get; private set; }

        public static DataSet ClassDataset { get; private set; }
        public static DataSet MethodDataset { get; private set; }


        static Discretizer()
        {
            _ef = new Discretization.EFDataSet();

            DiscretizeClassTrainingSet();
            DiscretizeMethodTrainingSet();
        }

        private static void DiscretizeClassTrainingSet()
        {
            _ef.Load(GetFullPath("ClassTrainingSet_1356_03122017_withoutOutlier.csv"));

            LOCClass = new DiscretizedData(_ef.Discretize<int>(0, 8));
            TCC = new DiscretizedData(_ef.Discretize<double>(2, 8));
            WMC = new DiscretizedData(_ef.Discretize<int>(3, 8));
            ATFDClass = new DiscretizedData(_ef.Discretize<int>(4, 8));

            //_ef.WriteToCsv(@"C:\Temp\outClass_test.csv");
        }

        private static void DiscretizeMethodTrainingSet()
        {
            _ef.Load(GetFullPath("MethodTrainingSet_2204_30112017_withoutOutlier.csv"));

            LOC = new DiscretizedData(_ef.Discretize<int>(0, 8));
            CYCLO = new DiscretizedData(_ef.Discretize<int>(1, 8));
            ATFD = new DiscretizedData(_ef.Discretize<int>(5, 8));
            FDP = new DiscretizedData(_ef.Discretize<int>(6, 8));
            LAA = new DiscretizedData(_ef.Discretize<double>(7, 8));
            MAXNESTING = new DiscretizedData(_ef.Discretize<int>(8, 8));
            NOAV = new DiscretizedData(_ef.Discretize<int>(9, 8));

            //_ef.WriteToCsv(@"C:\Temp\outMethod_test.csv");
        }

        private static string GetFullPath(string file)
        {
            string p = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            return p + @"\TrainingsData\" + file;
        }


    }
}

