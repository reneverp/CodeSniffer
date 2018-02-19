
namespace CodeSniffer.ViewModels.ApplicationInterfaces
{
    public interface IOService
    {
        string OpenFolderDialog();
        string OpenFileDialog(bool multiSelect);
        void WriteToFile(string filename, string content);
        string ReadContentFromFile(string filename);
    }
}
