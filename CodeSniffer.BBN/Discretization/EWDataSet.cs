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

        //The rows needed for parameter estimation
        public IList<DataRow> Rows { get; private set; }

        public override string ToString()
        {
            string upperBoundaryStr = UpperBoundary.ToString().Replace('.', '_');
            string lowerBoundaryStr = LowerBoundary.ToString().Replace('.', '_');

            if (UpperBoundary == double.MaxValue)
            {
                upperBoundaryStr = "andUp";
            }

            return "bin_" + lowerBoundaryStr + "_" + upperBoundaryStr;
        }

        public Bin(double lower, double upper)
        {
            LowerBoundary = lower;
            UpperBoundary = upper;

            Rows = new List<DataRow>();
        }
    }

    public class EWDataSet
    {
        private DataSet _dataset;
        private IDictionary<DataRow, DiscreteValue[]> _discreteVals;
        private IDictionary<int, IList<Bin>> _bins;
        private DataSet _discreteDataset;

        public EWDataSet()
        {
            _dataset = new DataSet();
            _discreteVals = new Dictionary<DataRow, DiscreteValue[]>();
            _bins = new Dictionary<int, IList<Bin>>();
        }

        public void Load(string csvFile)
        {
            _discreteDataset = null;
            _dataset = new DataSet();
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

        //Use x times the std deviation as max
        public IList<Bin> Discretize<T>(int colIndex, int amountOfBins, double upperLimit)
        {
            var rows = _dataset.Tables[0].Select().OrderBy(x => x.Field<T>(colIndex));

            double min = 0;//Convert.ToDouble(rows.First().Field<T>(rowIndex));
            double max = upperLimit; //Convert.ToDouble(rows.Last().Field<T>(rowIndex));
            double binsize = (max - min) / amountOfBins;

            for(int i =0; i < amountOfBins; i++)
            {
                double lower = min + (i * binsize);
                double upper = min + ((i + 1) * binsize);

                if (i == amountOfBins - 1)
                {
                    upper = double.MaxValue;
                }

                Bin currentBin = new Bin(lower, upper);
                _bins[colIndex].Add(currentBin);

                var selectedRows = rows.Where(x => lower <= Convert.ToDouble(x.Field<T>(colIndex)) && Convert.ToDouble(x.Field<T>(colIndex)) <= upper);

                foreach(var row in selectedRows)
                {
                    if(!_discreteVals.ContainsKey(row))
                    {
                        _discreteVals.Add(row, new DiscreteValue[row.ItemArray.Length]);
                    }

                    _discreteVals[row][colIndex] = new DiscreteValue(Convert.ToDouble(row.Field<T>(colIndex)), currentBin);
                    currentBin.Rows.Add(row);
                }
            }

            return _bins[colIndex];
        }

        public void WriteBinsToCsv(string filename)
        {
            IList<int> keys = _bins.Keys.ToList();

            foreach (var key in keys)
            {
                if (_bins[key].Count > 0)
                {
                    using (StreamWriter sw = new StreamWriter(filename.Replace(".csv", "_" + key + "_.csv")))
                    {
                        string toWrite = "";

                        foreach (var bin in _bins[key])
                        {
                            toWrite += bin.ToString() + ",";
                        }

                        sw.WriteLine(toWrite.Substring(0, toWrite.LastIndexOf(',')));

                        toWrite = "";
                        foreach (var bin in _bins[key])
                        {
                            toWrite += bin.Rows.Count() + ",";
                        }

                        sw.WriteLine(toWrite.Substring(0, toWrite.LastIndexOf(',')));
                    }
                }
            }
        }

        public DataSet GetDiscreteDataSet()
        {
            if (_discreteDataset == null)
            {
                _discreteDataset = new DataSet();
                _discreteDataset.Tables.Add();

                var rows = _dataset.Tables[0].Select();

                foreach (DataColumn col in _dataset.Tables[0].Columns)
                {
                    _discreteDataset.Tables[0].Columns.Add(col.ColumnName, typeof(string));
                }

                foreach (var row in rows)
                {
                    var currentRow = _discreteDataset.Tables[0].Rows.Add();

                    var values = _discreteVals[row];

                    int fieldCount = values.Count();

                    for (int i = 0; i < fieldCount; i++)
                    {
                        if (values[i] != null)
                        {
                            currentRow[i] = values[i].DiscreteBin.ToString();
                        }
                        else
                        {
                            currentRow[i] = row.ItemArray[i].ToString();
                        }
                    }
                }
            }

            return _discreteDataset;
        }

        public void WriteToCsv(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                var rows = _discreteDataset.Tables[0].Select();

                string toWrite = "";
                foreach (DataColumn col in _discreteDataset.Tables[0].Columns)
                {
                    toWrite += col.ColumnName + ",";
                }

                sw.WriteLine(toWrite.Substring(0, toWrite.LastIndexOf(',')));

                foreach (var row in rows)
                {
                    toWrite = "";

                    for (int i = 0; i < row.ItemArray.Count(); i++)
                    {
                        toWrite += row.ItemArray[i].ToString() + ",";
                    }

                    sw.WriteLine(toWrite.Substring(0, toWrite.LastIndexOf(',')));
                }
            }
        }
    }
}
