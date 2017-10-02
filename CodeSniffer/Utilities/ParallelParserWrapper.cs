using CodeSniffer.Interfaces;
using CodeSniffer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Utilities
{
    class AsyncParserWrapper
    {
        private IParser _concreteParser;
        private DirectoryUtil _directoryUtil;


        public AsyncParserWrapper(IParser concreteParser, DirectoryUtil directoryUtil)
        {
            _concreteParser = concreteParser;
            _directoryUtil = directoryUtil;
        }

        public async Task<IProject> ParseAsync(string directory)
        {
            IProject project = new Project();

            var filenames = _directoryUtil.GetFileNames(directory, "java");

            List<Task> taskList = new List<Task>();

            foreach (var filename in filenames)
            {
                taskList.Add(Task.Run(() => _concreteParser.Parse(filename, project)));
            }

            await Task.WhenAll(taskList);

            if (taskList.Any(x => x.IsFaulted))
            {
                project = null;
            }

            return project;
        }
    }
}
