using CodeSniffer.BBN.Discretization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.BBN.ParameterEstimation
{
    class LaplaceEstimator
    {
        public static void Adapt(IList<DataRow> additionalCases, DataSet discretizedDataSet, BayesianNetwork network, IDictionary<string, DiscretizedData> discretizedDataMap, string classificationNodeName, int laplaceSmoothing, double fadingFactor)
        {
            //TODO: fading....!!

            if (discretizedDataSet.Tables == null || discretizedDataSet.Tables.Count == 0)
            {
                throw new Exception("Discretized dataset empty");
            }

            var rows = discretizedDataSet.Tables[0].Select().OrderBy(x => x.Field<string>(0));

            foreach (var row in additionalCases)
            {
                discretizedDataSet.Tables[0].Rows.Add(row);
            }

            var countTrueOnly = rows.Where(x => x.Field<string>(classificationNodeName) == "True").Count();
            var countFalseOnly = rows.Where(x => x.Field<string>(classificationNodeName) == "False").Count();

            var rowsAdapt = discretizedDataSet.Tables[0].Select().OrderBy(x => x.Field<string>(0));
            var adaptCountTrueOnly = rowsAdapt.Where(x => x.Field<string>(classificationNodeName) == "True").Count();
            var adaptCountFalseOnly = rowsAdapt.Where(x => x.Field<string>(classificationNodeName) == "False").Count();

            var adaptedCountTrueOnly = (countTrueOnly - adaptCountTrueOnly) * -1;
            var adaptedCountFalseOnly = (countFalseOnly - adaptCountFalseOnly) * -1;

            double countTrue = (countTrueOnly * fadingFactor + adaptedCountTrueOnly + laplaceSmoothing);
            double countFalse = (countFalseOnly * fadingFactor + adaptedCountFalseOnly + laplaceSmoothing);

            double probFalse = (double)(countFalse) / (((double)(rows.Count() * fadingFactor) + adaptedCountTrueOnly + adaptedCountFalseOnly + (laplaceSmoothing * 2)));
            double probTrue = (double)(countTrue) / (((double)(rows.Count() * fadingFactor) + adaptedCountTrueOnly + adaptedCountFalseOnly + (laplaceSmoothing * 2)) );

            network.SetProbabilities(classificationNodeName, new double[] { probTrue, probFalse });

            foreach (var kvp in discretizedDataMap)
            {
                List<double> falseProbs = new List<double>();
                List<double> trueProbs = new List<double>();

                foreach (var bin in kvp.Value.Bins)
                {
                    //Debug.WriteLine("|- " + bin.ToString() + " -|");

                    var rowsTrue = rows.Where(x => x.Field<string>(kvp.Key) == bin.ToString() && x.Field<string>(classificationNodeName) == "True");
                    var rowsFalse = rows.Where(x => x.Field<string>(kvp.Key) == bin.ToString() && x.Field<string>(classificationNodeName) == "False");

                    var adaptRowsTrue = (rowsTrue.Count() - rowsAdapt.Where(x => x.Field<string>(kvp.Key) == bin.ToString() && x.Field<string>(classificationNodeName) == "True").Count()) * -1;
                    var adaptRowsFalse = (rowsFalse.Count() - rowsAdapt.Where(x => x.Field<string>(kvp.Key) == bin.ToString() && x.Field<string>(classificationNodeName) == "False").Count()) * -1;

                    countTrue = (rowsTrue.Count() * fadingFactor + adaptRowsTrue + laplaceSmoothing);
                    countFalse = (rowsFalse.Count() * fadingFactor + adaptRowsFalse + laplaceSmoothing);

                    probFalse = (double)(countFalse) / ((double)(countFalseOnly * fadingFactor) + adaptedCountFalseOnly + ((laplaceSmoothing * kvp.Value.Bins.Count)) );
                    probTrue = (double)(countTrue) / ((double)(countTrueOnly * fadingFactor) + adaptedCountTrueOnly + ((laplaceSmoothing * kvp.Value.Bins.Count)) );

                    falseProbs.Add(probFalse);
                    trueProbs.Add(probTrue);
                }

                trueProbs.AddRange(falseProbs);

                network.SetProbabilities(kvp.Key, trueProbs.ToArray());
            }
        }

        public static void LaplaceEstimation(DataSet dataset, BayesianNetwork network, IDictionary<string, DiscretizedData> map, string classificationNodeName, int laplaceSmoothing)
        {
            Adapt(new List<DataRow>(), dataset, network, map, classificationNodeName, laplaceSmoothing, 1.0);
        }
    }
}
