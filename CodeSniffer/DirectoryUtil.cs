using System.Collections.Generic;
using System.IO;

namespace CodeSniffer
{
    //TODO, duplication (this functionality also exisstss in the console app
    class DirectoryUtil
    {
        public IList<string> GetFileNames(string directory, string extension, List<string> files = null)
        {
            if (files == null)
            {
                files = new List<string>();
            }

            files.AddRange(Directory.GetFiles(directory, "*." + extension));

            foreach (var dir in Directory.GetDirectories(directory))
            {
                GetFileNames(dir, extension, files);
            }

            return files;
        }
    }
}
