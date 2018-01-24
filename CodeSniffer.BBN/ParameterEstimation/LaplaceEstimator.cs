using CodeSniffer.BBN.Discretization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.BBN.ParameterEstimation
{
    class LaplaceEstimator
    {
        public static void LaplaceEstimation(DataSet discretizedDataSet, BayesianNetwork network, IDictionary<string, DiscretizedData> discretizedDataMap, string classificationNodeName)
        {
            if(discretizedDataSet.Tables == null || discretizedDataSet.Tables.Count == 0)
            {
                throw new Exception("Discretized dataset empty");
            }

            var rows = discretizedDataSet.Tables[0].Select().OrderBy(x => x.Field<string>(0));

            ////LAPLACE ESTIMATION SMOOTHING EXAMPLE
            var countTrueOnly = rows.Where(x => x.Field<string>(classificationNodeName) == "True").Count();
            var countFalseOnly = rows.Where(x => x.Field<string>(classificationNodeName) == "False").Count();

            int countTrue = countTrueOnly + 1;
            int countFalse = countTrueOnly + 1;

            double probFalse = (double)(countFalse) / ((double)(countFalseOnly) + (1 * 2));
            double probTrue = (double)(countTrue) / ((double)(countTrueOnly) + (1 * 2));

            network.SetProbabilities(classificationNodeName, new double[] { probTrue, probFalse });

            //network.SetEvidence()

            //foreach (var bin in LOCClass)
            //{
            //    var rowsTrue = rows.Where(x => x.Field<string>("Lines of Code") == bin.ToString() && x.Field<string>("Large Class") == "True");
            //    var rowsFalse = rows.Where(x => x.Field<string>("Lines of Code") == bin.ToString() && x.Field<string>("Large Class") == "False");

            //    countTrue = rowsTrue.Count() + 1;
            //    countFalse = rowsFalse.Count() + 1;

            //    double probFalse = (double)(countFalse) / ((double)(countFalseOnly) + (1 * LOCClass.Count));
            //    double probTrue = (double)(countTrue) / ((double)(countTrueOnly) + (1 * LOCClass.Count));

            //    Debug.WriteLine(probFalse + "         ,            " + probTrue);
            //}

        }
    }
}
