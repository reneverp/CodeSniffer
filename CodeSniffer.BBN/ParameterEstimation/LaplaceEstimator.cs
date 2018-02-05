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
            var adaptCountTrueOnly = additionalCases.Where(x => x.Field<string>(classificationNodeName) == "True").Count();
            var adaptCountFalseOnly = additionalCases.Where(x => x.Field<string>(classificationNodeName) == "False").Count();

            var adaptedCountTrueOnly = (countTrueOnly + adaptCountTrueOnly);
            var adaptedCountFalseOnly = (countFalseOnly + adaptCountFalseOnly);

            double countTrue = (countTrueOnly * fadingFactor + adaptCountTrueOnly + laplaceSmoothing);
            double countFalse = (countFalseOnly * fadingFactor + adaptCountFalseOnly + laplaceSmoothing);

            double probFalse = (double)(countFalse) / (((double)(rows.Count() * fadingFactor) + adaptCountTrueOnly + adaptCountFalseOnly + (laplaceSmoothing * 2)));
            double probTrue = (double)(countTrue) / (((double)(rows.Count() * fadingFactor) + adaptCountTrueOnly + adaptCountFalseOnly + (laplaceSmoothing * 2)) );

            network.SetProbabilities(classificationNodeName, new double[] { probTrue, probFalse });

            foreach (var kvp in discretizedDataMap)
            {
                List<double> falseProbs = new List<double>();
                List<double> trueProbs = new List<double>();

                foreach (var bin in kvp.Value.Bins)
                {
                    var rowsTrue = rows.Where(x => x.Field<string>(kvp.Key) == bin.ToString() && x.Field<string>(classificationNodeName) == "True");
                    var rowsFalse = rows.Where(x => x.Field<string>(kvp.Key) == bin.ToString() && x.Field<string>(classificationNodeName) == "False");

                    var adaptRowsTrue = (additionalCases.Where(x => x.Field<string>(kvp.Key) == bin.ToString() && x.Field<string>(classificationNodeName) == "True").Count());
                    var adaptRowsFalse = (additionalCases.Where(x => x.Field<string>(kvp.Key) == bin.ToString() && x.Field<string>(classificationNodeName) == "False").Count());

                    countTrue = (rowsTrue.Count() * fadingFactor + adaptRowsTrue + laplaceSmoothing);
                    countFalse = (rowsFalse.Count() * fadingFactor + adaptRowsFalse + laplaceSmoothing);

                    probFalse = (double)(countFalse) / ((double)(countFalseOnly * fadingFactor) + adaptCountFalseOnly + ((laplaceSmoothing * kvp.Value.Bins.Count)) );
                    probTrue = (double)(countTrue) / ((double)(countTrueOnly * fadingFactor) + adaptCountTrueOnly + ((laplaceSmoothing * kvp.Value.Bins.Count)) );

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
