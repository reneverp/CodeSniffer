using CodeSniffer.ViewModels;
using System;
using System.Collections.Generic;
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

            var p = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            File.Delete(p + @"\ClassTest.csv");
            File.Delete(p + @"\MethodTest.csv");

            foreach (var frag in vm.CodeFragments)
            {
                if (frag.Model.Metrics.Where(x => x.Name == "LOC").FirstOrDefault()?.Value > 50 &&
                    frag.Model.Metrics.Where(x => x.Name == "ATFD").FirstOrDefault()?.Value > 1 &&
                    frag.Model.Metrics.Where(x => x.Name == "WMC").FirstOrDefault()?.Value > 2 &&
                    frag.Model.Metrics.Where(x => x.Name == "TCC").FirstOrDefault()?.Value > 0)
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
                    if (child.Model.Metrics.Where(x => x.Name == "ATFD").FirstOrDefault()?.Value > 5 &&
                        child.Model.Metrics.Where(x => x.Name == "LAA").FirstOrDefault()?.Value < 3 &&
                        child.Model.Metrics.Where(x => x.Name == "FDP").FirstOrDefault()?.Value > 3)
                    {
                        child.Model.CodeSmells.Where(x => x.Name == "Feature_Envy").FirstOrDefault().IsDetected = true;
                        Console.WriteLine("selected Feature_Envy codesmell");
                    }
                    else
                    {
                        child.Model.CodeSmells.Where(x => x.Name == "Feature_Envy").FirstOrDefault().IsDetected = false;
                    }

                    if (child.Model.Metrics.Where(x => x.Name == "LOC").FirstOrDefault()?.Value > 10 &&
                       child.Model.Metrics.Where(x => x.Name == "CYCLO").FirstOrDefault()?.Value > 3 &&
                       child.Model.Metrics.Where(x => x.Name == "MAXNESTING").FirstOrDefault()?.Value > 3 &&
                       child.Model.Metrics.Where(x => x.Name == "NOAV").FirstOrDefault()?.Value > 5)
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
