using CodeSniffer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.ViewModels
{
    class MainWindowViewModel
    {
        public string Html
        {
            get
            {
                Parser parser = new Parser();

                Project proj = new Project();
                parser.Parse(@"D:\svn\ganttproject-ganttproject-2.8.5\ganttproject-ganttproject-2.8.5\ganttproject\src\net\sourceforge\ganttproject\ChartTabContentPanel.java", proj);

                return StringToHtml(proj.CompilationUnits[0].Classes[0].Text);
            }
        }

        private string StringToHtml(string input)
        {
            string output = "<html>"
                            + "<head>"
                                + "<script src=\"https://cdn.rawgit.com/google/code-prettify/master/loader/run_prettify.js\"></script>"
                            + "</head>"
                            + "<pre class=\"prettyprint\">";

            output += input.Replace("\n", "<br/>");

            output += "</pre></html>";


            return output;
        }
    }
}
