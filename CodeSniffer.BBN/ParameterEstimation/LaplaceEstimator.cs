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

            IDictionary<string, IList<double>> sampleCounts = network.GetSampleCounts();

            double countTrueOnly = 0;
            double countFalseOnly = 0;

            if (!sampleCounts.ContainsKey(classificationNodeName))
            {
                countTrueOnly = rows.Where(x => x.Field<string>(classificationNodeName) == "True").Count();
                countFalseOnly = rows.Where(x => x.Field<string>(classificationNodeName) == "False").Count();
            }
            else
            {
                //get previous sample counts
                countTrueOnly = sampleCounts[classificationNodeName][0];
                countFalseOnly = sampleCounts[classificationNodeName][1];


                Console.WriteLine(classificationNodeName + ":: " + countTrueOnly);
                Console.WriteLine(classificationNodeName + ":: " + countFalseOnly);
            }

            var adaptCountTrueOnly = additionalCases.Where(x => x.Field<string>(classificationNodeName) == "True").Count();
            var adaptCountFalseOnly = additionalCases.Where(x => x.Field<string>(classificationNodeName) == "False").Count();

            var adaptedCountTrueOnly = (countTrueOnly + adaptCountTrueOnly);
            var adaptedCountFalseOnly = (countFalseOnly + adaptCountFalseOnly);

            double fadedCountTrue = countTrueOnly * fadingFactor + adaptCountTrueOnly;
            double fadedCountFalse = countFalseOnly * fadingFactor + adaptCountFalseOnly;

            //set new counts for next adaptation round
            AddSampleCount(classificationNodeName, ref sampleCounts, new[] { fadedCountTrue, fadedCountFalse });

            double countTrue = (fadedCountTrue   + laplaceSmoothing);
            double countFalse = (fadedCountFalse + laplaceSmoothing);

            double fadedCount = (countTrueOnly + countFalseOnly) * fadingFactor;
            double totalCount = (((double)(fadedCount) + adaptCountTrueOnly + adaptCountFalseOnly + (laplaceSmoothing * 2)));

            double probFalse = (double)(countFalse) / totalCount;
            double probTrue = (double)(countTrue) / totalCount;

            network.SetProbabilities(classificationNodeName, new double[] { probTrue, probFalse });

            foreach (var kvp in discretizedDataMap)
            {
                List<double> falseProbs = new List<double>();
                List<double> trueProbs = new List<double>();

                foreach (var bin in kvp.Value.Bins)
                {
                    double rowsTrue = 0;
                    double rowsFalse = 0;

                    if (!sampleCounts.ContainsKey(kvp.Key + bin.ToString()))
                    {
                        rowsTrue = rows.Where(x => x.Field<string>(kvp.Key) == bin.ToString() && x.Field<string>(classificationNodeName) == "True").Count();
                        rowsFalse = rows.Where(x => x.Field<string>(kvp.Key) == bin.ToString() && x.Field<string>(classificationNodeName) == "False").Count();
                    }
                    else
                    {
                        rowsTrue = sampleCounts[kvp.Key + bin.ToString()][0];
                        rowsFalse = sampleCounts[kvp.Key + bin.ToString()][1];
                    }

                    var adaptRowsTrue = (additionalCases.Where(x => x.Field<string>(kvp.Key) == bin.ToString() && x.Field<string>(classificationNodeName) == "True").Count());
                    var adaptRowsFalse = (additionalCases.Where(x => x.Field<string>(kvp.Key) == bin.ToString() && x.Field<string>(classificationNodeName) == "False").Count());

                    fadedCountTrue = rowsTrue * fadingFactor + adaptRowsTrue;
                    fadedCountFalse = rowsFalse * fadingFactor + adaptRowsFalse;

                    //set new counts for next adaptation round
                    AddSampleCount(kvp.Key + bin.ToString(), ref sampleCounts, new[] { fadedCountTrue, fadedCountFalse });

                    countTrue = (fadedCountTrue  + laplaceSmoothing);
                    countFalse = (fadedCountFalse + laplaceSmoothing);

                    probFalse = (double)(countFalse) / ((double)(countFalseOnly * fadingFactor) + adaptCountFalseOnly + ((laplaceSmoothing * kvp.Value.Bins.Count)));
                    probTrue = (double)(countTrue) / ((double)(countTrueOnly * fadingFactor) + adaptCountTrueOnly + ((laplaceSmoothing * kvp.Value.Bins.Count)));

                    falseProbs.Add(probFalse);
                    trueProbs.Add(probTrue);
                }

                trueProbs.AddRange(falseProbs);

                network.SetProbabilities(kvp.Key, trueProbs.ToArray());
            }

            network.SetSampleCounts(sampleCounts);
        }

        private static void AddSampleCount(string classificationNodeName, ref IDictionary<string, IList<double>> sampleCounts, IList<double> counts)
        {
            if (sampleCounts.ContainsKey(classificationNodeName))
            {
                sampleCounts[classificationNodeName] = counts;
            }
            else
            {
                sampleCounts.Add(classificationNodeName, counts);
            }
        }

        public static void LaplaceEstimation(DataSet dataset, BayesianNetwork network, IDictionary<string, DiscretizedData> map, string classificationNodeName, int laplaceSmoothing)
        {
            Adapt(new List<DataRow>(), dataset, network, map, classificationNodeName, laplaceSmoothing, 1.0);
        }
    }
}
