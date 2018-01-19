using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.BBN.Discretization
{
    public class DiscreteValue
    {
        public Bin DiscreteBin { get; set; }
        public double Value { get; private set; }

        public DiscreteValue(double value, Bin bin)
        {
            DiscreteBin = bin;
            Value = value;
        }
    }

    public class Bin
    {
        public double LowerBoundary { get; private set; }
        public double UpperBoundary { get; private set; }

        public Bin(double lower, double upper)
        {
            LowerBoundary = lower;
            UpperBoundary = upper;
        }
    }

    public class EFDataSet
    {
        private DataSet _dataset;
        private IDictionary<DataRow, DiscreteValue[]> _discreteVals;
        private IDictionary<int, IList<Bin>> _bins;

        public EFDataSet()
        {
            _dataset = new DataSet();
            _discreteVals = new Dictionary<DataRow, DiscreteValue[]>();
            _bins = new Dictionary<int, IList<Bin>>();
        }

        public void Load(string csvFile)
        {
            _dataset.Clear();
            _bins.Clear();
            _discreteVals.Clear();

            string FileName = csvFile;

            OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OleDb.4.0; Data Source = " + Path.GetDirectoryName(FileName) +"; Extended Properties = \"Text;HDR=YES;FMT=Delimited\"");
            conn.Open();

            OleDbDataAdapter adapter = new OleDbDataAdapter
                    ("SELECT * FROM " + Path.GetFileName(FileName), conn);

            adapter.Fill(_dataset);

            for(int i =0; i < _dataset.Tables[0].Columns.Count; i++)
            {
                _bins.Add(i, new List<Bin>());
            }

            conn.Close();
        }

        public IDictionary<DataRow, DiscreteValue[]> Discretize<T>(int rowIndex, int amountOfBins)
        {
            var rows = _dataset.Tables[0].Select().OrderBy(x => x.Field<T>(rowIndex));

            double min = Convert.ToDouble(rows.First().Field<T>(rowIndex));
            double max = Convert.ToDouble(rows.Last().Field<T>(rowIndex));
            double binsize = (max - min) / amountOfBins;


            IList<double> locs = new List<double>();
            for(int i =0; i < amountOfBins; i++)
            {
                double lower = min + (i * binsize);
                double upper = min + ((i + 1) * binsize);

                if (i == amountOfBins - 1)
                {
                    upper = double.MaxValue;
                }

                Bin currentBin = new Bin(lower, upper);
                _bins[rowIndex].Add(currentBin);

                var selectedRows = rows.Where(x => lower <= Convert.ToDouble(x.Field<T>(rowIndex)) && Convert.ToDouble(x.Field<T>(rowIndex)) <= upper);

                foreach(var row in selectedRows)
                {
                    if(!_discreteVals.ContainsKey(row))
                    {
                        _discreteVals.Add(row, new DiscreteValue[row.ItemArray.Length]);
                    }

                    _discreteVals[row][rowIndex] = new DiscreteValue(Convert.ToDouble(row.Field<T>(rowIndex)), currentBin);
                }
            }

            return _discreteVals;
        }

        public void WriteToCsv(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                var rows = _dataset.Tables[0].Select();

                foreach(var row in rows)
                {
                    string rowToWrite = "";
                    var values = _discreteVals[row];

                    int fieldCount = values.Count();

                    for(int i = 0; i < fieldCount; i++)
                    {
                        string tmp = "";

                        if (values[i] != null)
                        {
                            tmp = "b_" + values[i].DiscreteBin.LowerBoundary.ToString() + "_" + values[i].DiscreteBin.UpperBoundary.ToString();
                        }
                        else
                        {
                            tmp = row.ItemArray[i].ToString();
                        }

                        if (i != fieldCount - 1)
                        {
                            rowToWrite += tmp + ",";
                        }
                        else
                        {
                            rowToWrite += tmp;
                        }
                    }

                    sw.WriteLine(rowToWrite.Replace('.', '_'));
                }
            }

            WriteBins(filename);
        }

        private void WriteBins(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename.Substring(0, filename.LastIndexOf('.')) + "_bins.csv"))
            {
                for(int i = 0; i < 8; i++)
                {
                    string toWrite  = "b_" + _bins[0][i].LowerBoundary.ToString() + "_" + _bins[0][i].UpperBoundary.ToString() + ",";
                           toWrite += "b_" + _bins[2][i].LowerBoundary.ToString() + "_" + _bins[2][i].UpperBoundary.ToString() + ",";
                           toWrite += "b_" + _bins[3][i].LowerBoundary.ToString() + "_" + _bins[3][i].UpperBoundary.ToString();
                           toWrite += "b_" + _bins[4][i].LowerBoundary.ToString() + "_" + _bins[3][i].UpperBoundary.ToString();

                    sw.WriteLine(toWrite.Replace('.', '_'));
                }
            }
        }
    }
}
