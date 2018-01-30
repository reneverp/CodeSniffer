using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.BBN.Discretization
{
    public static class Discretizer
    {
        private static EWDataSet _ew;

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

        public static bool IsDiscretized { get; private set; }

        private static object _lockObj;

        static Discretizer()
        {
            _ew = new Discretization.EWDataSet();
            _lockObj = new object();
        }

        public static void DiscretizeTrainingSets()
        {
            lock (_lockObj)
            {
                DiscretizeClassTrainingSet();
                DiscretizeMethodTrainingSet();

                IsDiscretized = true;
            }
        }

        private static void DiscretizeClassTrainingSet()
        {
            _ew.Load(GetFullPath("ClassTrainingSet_1356_03122017_withoutOutlier.csv"));

            LOCClass = new DiscretizedData(_ew.Discretize<int>(0, 8));
            TCC = new DiscretizedData(_ew.Discretize<double>(2, 8));
            WMC = new DiscretizedData(_ew.Discretize<int>(3, 8));
            ATFDClass = new DiscretizedData(_ew.Discretize<int>(4, 8));

            ClassDataset = _ew.GetDiscreteDataSet();

            _ew.WriteToCsv(GetFullPath("ClassTrainingSet_1356_03122017_withoutOutlier_discretized.csv"));
        }

        private static void DiscretizeMethodTrainingSet()
        {
            _ew.Load(GetFullPath("MethodTrainingSet_2204_30112017_withoutOutlier.csv"));

            LOC = new DiscretizedData(_ew.Discretize<int>(0, 8));
            CYCLO = new DiscretizedData(_ew.Discretize<int>(1, 8));
            ATFD = new DiscretizedData(_ew.Discretize<int>(5, 8));
            FDP = new DiscretizedData(_ew.Discretize<int>(6, 8));
            LAA = new DiscretizedData(_ew.Discretize<double>(7, 8));
            MAXNESTING = new DiscretizedData(_ew.Discretize<int>(8, 8));
            NOAV = new DiscretizedData(_ew.Discretize<int>(9, 8));

            MethodDataset = _ew.GetDiscreteDataSet();

            _ew.WriteToCsv(GetFullPath("MethodTrainingSet_2204_30112017_withoutOutlier_discretized.csv"));
        }

        public static IList<DataRow> ProcessAdditionalMethodCases()
        {
            return ProcessAdditionalMethodCases(GetFullPath("AdditionalMethodData.csv"));
        }

        public static IList<DataRow> ProcessAdditionalMethodCases(string filename)
        {
            IList<DataRow> rowsToReturn = new List<DataRow>();

            if (File.Exists(filename))
            {
                //TODO:: MOVE TO OTHER CLASS
                var dataset = DataSetHelper.GetDataSetForCSV(filename);

                foreach (var row in dataset.Tables[0].Select())
                {
                    Bin loc = LOC.Discretize(row.Field<int>("LOC"));
                    Bin cyclo = CYCLO.Discretize(row.Field<int>("CYCLO"));
                    Bin atfd = ATFD.Discretize(row.Field<int>("ATFD"));
                    Bin fdp = FDP.Discretize(row.Field<int>("FDP"));

                    Bin laa = null;
                    try
                    {
                        laa = LAA.Discretize(row.Field<double>("LAA"));
                    }
                    catch
                    {
                        laa = LAA.Discretize(row.Field<int>("LAA"));
                    }
                    Bin maxnesting = MAXNESTING.Discretize(row.Field<int>("MAXNESTING"));
                    Bin noav = NOAV.Discretize(row.Field<int>("NOAV"));

                    string featureEnvy = row.Field<string>("Feature_Envy");
                    string longMethod = row.Field<string>("Long_Method");

                    DataRow newRow = MethodDataset.Tables[0].NewRow();

                    newRow.SetField<string>("LOC", loc.ToString());
                    newRow.SetField<string>("CYCLO", cyclo.ToString());
                    newRow.SetField<string>("ATFD", atfd.ToString());
                    newRow.SetField<string>("FDP", fdp.ToString());
                    newRow.SetField<string>("LAA", laa.ToString());
                    newRow.SetField<string>("MAXNESTING", maxnesting.ToString());
                    newRow.SetField<string>("NOAV", noav.ToString());

                    newRow.SetField<string>("Feature_Envy", featureEnvy);
                    newRow.SetField<string>("Long_Method", longMethod);

                    rowsToReturn.Add(newRow);
                }
            }

            return rowsToReturn;
        }

        public static IList<DataRow> ProcessAdditionalClassCases()
        {
            return ProcessAdditionalClassCases(GetFullPath("AdditionalClassData.csv"));
        }

        public static IList<DataRow> ProcessAdditionalClassCases(string filename)
        {
            IList<DataRow> rowsToReturn = new List<DataRow>();

            if (File.Exists(filename))
            {
                //TODO:: MOVE TO OTHER CLASS
                var dataset = DataSetHelper.GetDataSetForCSV(filename);

                foreach (var row in dataset.Tables[0].Select())
                {
                    Bin loc = LOCClass.Discretize(row.Field<int>("LOC"));
                    Bin tcc = TCC.Discretize(row.Field<double>("TCC"));
                    Bin wmc = WMC.Discretize(row.Field<int>("WMC"));
                    Bin atfd = ATFDClass.Discretize(row.Field<int>("ATFD"));

                    string largeClass = row.Field<string>("Large_Class");

                    DataRow newRow = ClassDataset.Tables[0].NewRow();

                    newRow.SetField<string>("LOC", loc.ToString());
                    newRow.SetField<string>("TCC", tcc.ToString());
                    newRow.SetField<string>("WMC", wmc.ToString());
                    newRow.SetField<string>("ATFD", atfd.ToString());

                    newRow.SetField<string>("Large_Class", largeClass);

                    rowsToReturn.Add(newRow);
                }
            }

            return rowsToReturn;
        }

        private static string GetFullPath(string file)
        {
            string p = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            return p + @"\TrainingsData\" + file;
        }


    }
}

