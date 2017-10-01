using System;
using System.Globalization;
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
            string output = "<html>"
                            + "<head>"
                                + "<script src=\"https://cdn.rawgit.com/google/code-prettify/master/loader/run_prettify.js\"></script>"
                                + "<style>"
                                    + "pre.prettyprint {"
                                        + "border: none !important;"
                                    + "}"
                                + "</style>"
                            + "</head>"
                            + "<pre class=\"prettyprint\">";

            output += input.Replace("\n", "<br/>");

            output += "</pre></html>";


            return output;
        }
    }
}
