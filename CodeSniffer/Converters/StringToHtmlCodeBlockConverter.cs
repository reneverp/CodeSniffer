using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Data;

namespace CodeSniffer.Converters
{
    class StringToHtmlCodeBlockConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
                return StringToHtml(value as string);
            else
                return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private string StringToHtml(string input)
        {
            string cssFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Css\desert.css");

           
            string output = "<html>"
                            + "<head>"
                                + "<link rel=\"stylesheet\" type=\"text/css\" href=\""+ cssFilePath +"\">"
                                  + "<script src=\"https://cdn.rawgit.com/google/code-prettify/master/loader/run_prettify.js\"></script>"
                                  + "<style>"
                                    + "pre.prettyprint {"
                                                + "border: none !important;"
                                                + "padding: 0;"
                                                + "margin: 0;"
                                        + "}"
                                + "</style>"
                            + "</head>"
                            + "<body>"
                            + "<pre class=\"prettyprint\">";

            output += input.Replace("\n", Environment.NewLine);

            output += "</body></pre></html>";


            return output;
        }
    }
}
