using CodeSniffer.BBN.Discretization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace CodeSniffer.Discretizer
{
    class Program
    {
        private static bool _isClass;
        private static bool _isMethod;
        private static string _input;
        private static string _output;
        private static IList<DataRow> _rows;

        static void Main(string[] args)
        {
            if(args.Count() != 3)
            {
                Console.WriteLine("usage: CodeSniffer.Discretizer.exe <c | m> <input csv file> <output csv file>");
                Environment.Exit(-1);
            }

            if(args[0].ToLower().Trim() == "c")
            {
                _isClass = true;
            }
            if(args[0].ToLower().Trim() == "m")
            {
                _isMethod = true;
            }

            _input = args[1].Trim();
            _output = args[2].Trim();


            BBN.Discretization.Discretizer.DiscretizeTrainingSets();

            if(_isMethod)
            {
                _rows = BBN.Discretization.Discretizer.ProcessAdditionalMethodCases(_input);
            }

            if (_isClass)
            {
                _rows = BBN.Discretization.Discretizer.ProcessAdditionalClassCases(_input);
            }

            using (StreamWriter sw = new StreamWriter(_output))
            {
                var cols = _rows[0].Table.Columns;

                string toWrite = "";

                for (int i =0; i < cols.Count; i++)
                {
                    toWrite += cols[i].ColumnName;

                    if(i != cols.Count -1)
                    {
                        toWrite += ",";
                    }
                }
                sw.WriteLine(toWrite);

                foreach(var row in _rows)
                {
                    toWrite = "";
                    for (int i = 0; i < row.ItemArray.Count(); i++)
                    {
                        toWrite += row.ItemArray[i].ToString();

                        if (i != row.ItemArray.Count() - 1)
                        {
                            toWrite += ",";
                        }
                    }

                    sw.WriteLine(toWrite);
                }
            }
        }
    }
}
