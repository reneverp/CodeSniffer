using CodeSniffer.Interfaces;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace CodeSniffer.ViewModels.Utilities
{
    public class IOService : ApplicationInterfaces.IOService
    {
        public string OpenFolderDialog()
        {
            var dialog = new FolderBrowserDialog();
            dialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            var result = dialog.ShowDialog();

            if(result == DialogResult.OK)
            {
                return dialog.SelectedPath;
            }

            return "";
        }

        public string OpenFileDialog(bool multiSelect)
        {
            var dialog = new OpenFileDialog();
            dialog.InitialDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            dialog.Multiselect = multiSelect;

            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                return dialog.FileName;
            }

            return "";
        }

        public void WriteToFile(string filename, string content)
        {
            using (var sw = new StreamWriter(filename))
            {
                sw.Write(content);
            }
        }

        public string ReadContentFromFile(string filename)
        {
            string result = "";

            using (var sr = new StreamReader(filename))
            {
                result = sr.ReadToEnd();
            }

            return result;
        }
    }
}
