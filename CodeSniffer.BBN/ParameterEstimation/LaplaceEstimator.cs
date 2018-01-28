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
        public static void LaplaceEstimation(DataSet discretizedDataSet, BayesianNetwork network, IDictionary<string, DiscretizedData> discretizedDataMap, string classificationNodeName, int laplaceSmoothing)
        {
            if(discretizedDataSet.Tables == null || discretizedDataSet.Tables.Count == 0)
            {
                throw new Exception("Discretized dataset empty");
            }

            var rows = discretizedDataSet.Tables[0].Select().OrderBy(x => x.Field<string>(0));

            var countTrueOnly = rows.Where(x => x.Field<string>(classificationNodeName) == "True").Count();
            var countFalseOnly = rows.Where(x => x.Field<string>(classificationNodeName) == "False").Count();

            int countTrue = countTrueOnly + laplaceSmoothing;
            int countFalse = countFalseOnly + laplaceSmoothing;

            double probFalse = (double)(countFalse) / ((double)(rows.Count()) + (laplaceSmoothing * 2));
            double probTrue = (double)(countTrue) / ((double)(rows.Count()) + (laplaceSmoothing * 2));

            network.SetProbabilities(classificationNodeName, new double[] { probTrue, probFalse });

            foreach (var kvp in discretizedDataMap)
            {
                List<double> falseProbs = new List<double>();
                List<double> trueProbs = new List<double>();

                foreach (var bin in kvp.Value.Bins)
                {
                    Debug.WriteLine("|- " + bin.ToString() + " -|");

                    var rowsTrue = rows.Where(x => x.Field<string>(kvp.Key) == bin.ToString() && x.Field<string>(classificationNodeName) == "True");
                    var rowsFalse = rows.Where(x => x.Field<string>(kvp.Key) == bin.ToString() && x.Field<string>(classificationNodeName) == "False");

                    countTrue = rowsTrue.Count() + laplaceSmoothing;
                    countFalse = rowsFalse.Count() + laplaceSmoothing;

                    probFalse = (double)(countFalse) / ((double)(countFalseOnly) + (laplaceSmoothing * kvp.Value.Bins.Count));
                    probTrue = (double)(countTrue) / ((double)(countTrueOnly) + (laplaceSmoothing * kvp.Value.Bins.Count));

                    falseProbs.Add(probFalse);
                    trueProbs.Add(probTrue);
                }

                trueProbs.AddRange(falseProbs);

                network.SetProbabilities(kvp.Key, trueProbs.ToArray());
            }

        }

        public static void Adapt(IList<DataRow> additionalMethodCases, DataSet discretizedDataSet, BayesianNetwork network, IDictionary<string, DiscretizedData> discretizedDataMap, string classificationNodeName, int laplaceSmoothing, double fadingFactor)
        {
            //TODO: fading....!!

            if (discretizedDataSet.Tables == null || discretizedDataSet.Tables.Count == 0)
            {
                throw new Exception("Discretized dataset empty");
            }

            var rows = discretizedDataSet.Tables[0].Select().OrderBy(x => x.Field<string>(0));

            foreach (var row in additionalMethodCases)
            {
                discretizedDataSet.Tables[0].Rows.Add(row);
            }

            var countTrueOnly = rows.Where(x => x.Field<string>(classificationNodeName) == "True").Count();
            var countFalseOnly = rows.Where(x => x.Field<string>(classificationNodeName) == "False").Count();

            var rowsAdapt = discretizedDataSet.Tables[0].Select().OrderBy(x => x.Field<string>(0));
            var adaptCountTrueOnly = rowsAdapt.Where(x => x.Field<string>(classificationNodeName) == "True").Count();
            var adaptCountFalseOnly = rowsAdapt.Where(x => x.Field<string>(classificationNodeName) == "False").Count();

            var adaptedCountTrueOnly = adaptCountTrueOnly - countTrueOnly;
            var adaptedCountFalseOnly = adaptCountFalseOnly - countFalseOnly;

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
                    Debug.WriteLine("|- " + bin.ToString() + " -|");

                    var rowsTrue = rows.Where(x => x.Field<string>(kvp.Key) == bin.ToString() && x.Field<string>(classificationNodeName) == "True");
                    var rowsFalse = rows.Where(x => x.Field<string>(kvp.Key) == bin.ToString() && x.Field<string>(classificationNodeName) == "False");

                    var adaptRowsTrue = rowsAdapt.Where(x => x.Field<string>(kvp.Key) == bin.ToString() && x.Field<string>(classificationNodeName) == "True").Count() - rowsTrue.Count();
                    var adaptRowsFalse = rowsAdapt.Where(x => x.Field<string>(kvp.Key) == bin.ToString() && x.Field<string>(classificationNodeName) == "False").Count() - rowsFalse.Count();

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
    }
}
