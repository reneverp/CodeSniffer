using CodeSniffer.Interfaces;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CodeSniffer.Utilities
{
    class IOService : ApplicationInterfaces.IOService
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
    }
}
