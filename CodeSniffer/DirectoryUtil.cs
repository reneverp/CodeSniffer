using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Console
{
    class DirectoryUtil
    {
        public void DeleteLogFile()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filesToDelete = Directory.GetFiles(path, "test*.log");

            foreach (var file in filesToDelete)
            {
                File.Delete(file);
            }
        }

        public IList<string> GetFileNames(string directory, string extension, List<string> files = null)
        {
            if (files == null)
            {
                files = new List<string>();
            }

            files.AddRange(Directory.GetFiles(directory, "*." + extension));

            foreach(var dir in Directory.GetDirectories(directory))
            {
                GetFileNames(dir, extension, files);
            }

            return files;
        }
    }
}
