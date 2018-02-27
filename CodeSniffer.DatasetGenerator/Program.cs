using CodeSniffer.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.DatasetGenerator
{
    class Program
    {

        static void Main(string[] args)
        {
            var parser = new Parser();
            var asyncParser = new ViewModels.Utilities.AsyncParserWrapper(parser, new ViewModels.Utilities.DirectoryUtil());
            var vm = new MainWindowViewModel(asyncParser, new ViewModels.Utilities.IOService());

            vm.RefreshAsync().Wait();

            double largeClass_LOC = double.Parse(ConfigurationManager.AppSettings["LargeClass_LOC"]);
            double largeClass_ATFD = double.Parse(ConfigurationManager.AppSettings["LargeClass_ATFD"]);
            double largeClass_WMC = double.Parse(ConfigurationManager.AppSettings["LargeClass_WMC"]);
            double largeClass_TCC = double.Parse(ConfigurationManager.AppSettings["LargeClass_TCC"]);

            double longMethod_LOC = double.Parse(ConfigurationManager.AppSettings["LongMethod_LOC"]);
            double longMethod_CYCLO = double.Parse(ConfigurationManager.AppSettings["LongMethod_CYCLO"]);
            double longMethod_MAXNESTING = double.Parse(ConfigurationManager.AppSettings["LongMethod_MAXNESTING"]);
            double longMethod_NOAV = double.Parse(ConfigurationManager.AppSettings["LongMethod_NOAV"]);

            double featureEnvy_ATFD = double.Parse(ConfigurationManager.AppSettings["FeatureEnvy_ATFD"]);
            double featureEnvy_LAA = double.Parse(ConfigurationManager.AppSettings["FeatureEnvy_LAA"]);
            double featureEnvy_FDP = double.Parse(ConfigurationManager.AppSettings["FeatureEnvy_FDP"]);

            var p = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            File.Delete(p + @"\ClassTest.csv");
            File.Delete(p + @"\MethodTest.csv");

            foreach (var frag in vm.CodeFragments)
            {
                if (frag.Model.Metrics.Where(x => x.Name == "LOC").FirstOrDefault()?.Value > largeClass_LOC &&
                    frag.Model.Metrics.Where(x => x.Name == "ATFD").FirstOrDefault()?.Value > largeClass_ATFD &&
                    frag.Model.Metrics.Where(x => x.Name == "WMC").FirstOrDefault()?.Value > largeClass_WMC &&
                    frag.Model.Metrics.Where(x => x.Name == "TCC").FirstOrDefault()?.Value > largeClass_TCC)
                {
                    frag.Model.CodeSmells.Where(x => x.Name == "Large_Class").FirstOrDefault().IsDetected = true;
                    Console.WriteLine("selected Large_Class codesmell");
                }
                else
                {
                    frag.Model.CodeSmells.Where(x => x.Name == "Large_Class").FirstOrDefault().IsDetected = false;
                }

                foreach (var child in frag.Children)
                {
                    if (child.Model.Metrics.Where(x => x.Name == "ATFD").FirstOrDefault()?.Value > featureEnvy_ATFD &&
                        child.Model.Metrics.Where(x => x.Name == "LAA").FirstOrDefault()?.Value < featureEnvy_LAA &&
                        child.Model.Metrics.Where(x => x.Name == "FDP").FirstOrDefault()?.Value > featureEnvy_FDP)
                    {
                        child.Model.CodeSmells.Where(x => x.Name == "Feature_Envy").FirstOrDefault().IsDetected = true;
                        Console.WriteLine("selected Feature_Envy codesmell");
                    }
                    else
                    {
                        child.Model.CodeSmells.Where(x => x.Name == "Feature_Envy").FirstOrDefault().IsDetected = false;
                    }

                    if (child.Model.Metrics.Where(x => x.Name == "LOC").FirstOrDefault()?.Value > longMethod_LOC &&
                       child.Model.Metrics.Where(x => x.Name == "CYCLO").FirstOrDefault()?.Value > longMethod_CYCLO &&
                       child.Model.Metrics.Where(x => x.Name == "MAXNESTING").FirstOrDefault()?.Value > longMethod_MAXNESTING &&
                       child.Model.Metrics.Where(x => x.Name == "NOAV").FirstOrDefault()?.Value > longMethod_NOAV)
                    {
                        child.Model.CodeSmells.Where(x => x.Name == "Long_Method").FirstOrDefault().IsDetected = true;
                        Console.WriteLine("selected Long_Method codesmell");
                    }
                    else
                    {
                        child.Model.CodeSmells.Where(x => x.Name == "Long_Method").FirstOrDefault().IsDetected = false;
                    }
                }
            }



            vm.GenerateDataset("test.csv");

        }
    }
}
