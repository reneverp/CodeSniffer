//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CodeSniffer.BBN.Discretization
//{
//    public class Bin
//    {
//        public double LowerBoundary { get; set; }
//        public double UpperBoundary { get; set; }
//        public IList<double> Values { get; private set; }

//        public Bin(double lowerBoundary, double upperBoundary)
//        {
//            LowerBoundary = lowerBoundary;
//            UpperBoundary = upperBoundary;
//            Values = new List<double>();
//        }
//    }

//    public class DiscreteValues
//    {       
//        public IList<Bin> Bins { get; private set; }

//        public DiscreteValues()
//        {
//            Bins = new List<Bin>();
//        }
//    }

//    public class EF
//    {
//        private IList<IList<double>> table;

//        public EF()
//        {
//            table = new List<IList<double>>();
//        }

//        public IList<DiscreteValues> Discretize()
//        {
//            var val = Load(@"C:\Temp\ClassTrainingSet_1356_03122017_withoutOutlier.csv");

//            IList<DiscreteValues> discretizedTable = new List<DiscreteValues>();

//            foreach (var col in val)
//            {
//                var ar = col.ToArray();

//                Array.Sort(ar);

//                double min = ar.First();
//                double max = ar.Last();
//                double binsize = (max - min) / 8;

//                DiscreteValues discretized = new DiscreteValues();

                

//                //double boundary = binsize;
//                double prevBoundary = min;

//                Bin bin = new Bin(prevBoundary, prevBoundary + binsize);

//                for (int i = 0; i < ar.Count(); i++)
//                {
//                    if (ar[i] <= prevBoundary + binsize)
//                    {
//                        bin.Values.Add(ar[i]);
//                    }
//                    else
//                    {
//                        discretized.Bins.Add(bin);
//                        prevBoundary = prevBoundary + binsize;

//                        while (ar[i] > prevBoundary + binsize)
//                        {
//                            bin = new Bin(prevBoundary, prevBoundary + binsize);
//                            discretized.Bins.Add(bin);
//                            prevBoundary = prevBoundary + binsize;
//                        }

//                        bin = new Bin(prevBoundary, prevBoundary + binsize);
//                        bin.Values.Add(ar[i]);
//                    }
//                }

//                discretized.Bins.Add(bin);

//                discretizedTable.Add(discretized);
//            }

//            GenerateNewCsv(discretizedTable, @"C:\Temp\ClassTrainingSet_1356_03122017_withoutOutlier.csv");

//            return discretizedTable;
//        }

//        public IList<IList<double>> Load(string filename)
//        {
//            table.Clear();

//            var lines = ReadLinesFromCsv(filename);
       
//            foreach(string line in lines)
//            {
//                var cells = line.Split(',');

//                for(int i =0; i < cells.Count(); i++)
//                {
//                    if(table.Count <= i)
//                    {
//                        table.Add(new List<double>());
//                    }

//                    double result = 0;
//                    double.TryParse(cells[i], out result);

//                    table[i].Add(result);
//                }
//            }

//            return table;
//        }

//        private IList<string> ReadLinesFromCsv(string filename)
//        {
//            IList<string> lines = new List<string>();

//            bool first = true;
//            using (StreamReader sr = new StreamReader(filename))
//            {
//                while (!sr.EndOfStream)
//                {
//                    string line = sr.ReadLine();

//                    if (!first)
//                    {
//                        lines.Add(line);
//                    }
//                    else
//                    {
//                        first = false;
//                    }
//                }
//            }

//            return lines;
//        }

//        private void GenerateNewCsv(IList<DiscreteValues> discreteTable, string from)
//        {
//            IList<string> lines = ReadLinesFromCsv(from);

//            IList<string> linesToWrite = new List<string>();

//            foreach (string line in lines)
//            {
//                var cells = line.Split(',');

//                string towrite = "";
//                for (int i = 0; i < cells.Count(); i++)
//                {
//                    string cellStrValue = cells[i].ToLower().Trim();

//                    if (cellStrValue != "false" && cellStrValue != "true")
//                    {
//                        double result = 0;
//                        double.TryParse(cellStrValue, out result);

//                        var bin = discreteTable[i].Bins.Where(x => x.LowerBoundary <= result && result <= x.UpperBoundary).FirstOrDefault();

//                        if (towrite == "")
//                        {
//                            towrite += bin.LowerBoundary.ToString() + "_" + bin.UpperBoundary.ToString();
//                        }
//                        else
//                        {
//                            towrite += "," + bin.LowerBoundary.ToString() + "_" + bin.UpperBoundary.ToString();
//                        }
//                    }
//                    else
//                    {
//                        towrite += "," + cellStrValue;
//                    }
//                }

//                linesToWrite.Add(towrite);
//            }

//            using (StreamWriter sw = new StreamWriter(@"C:\temp\out.csv"))
//            {
//                sw.AutoFlush = true;

//                foreach(var line in linesToWrite)
//                {
//                    sw.WriteLine(line);
//                }
//            }
//        }


//    }
//}
