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
    public class DataSetHelper
    {
        public static DataSet GetDataSetForCSV(string csv)
        {
            //TODO:: MOVE TO OTHER CLASS
            DataSet dataset = new DataSet();

            OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OleDb.4.0; Data Source = " + Path.GetDirectoryName(csv) + "; Extended Properties = \"Text;HDR=YES;FMT=Delimited\"");
            conn.Open();

            OleDbDataAdapter adapter = new OleDbDataAdapter
                    ("SELECT * FROM " + Path.GetFileName(csv), conn);

            adapter.Fill(dataset);

            conn.Close();

            return dataset;
        }
    }
}
