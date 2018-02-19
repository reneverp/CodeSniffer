using CodeSniffer.Interfaces;
using CodeSniffer.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CodeSniffer.ViewModels.Utilities;

namespace CodeSniffer.ViewModels.Utilities
{
    public class AsyncParserWrapper
    {
        private IParser _concreteParser;
        private DirectoryUtil _directoryUtil;

        private ConcurrentQueue<Action> _taskQueue;

        public AsyncParserWrapper(IParser concreteParser, DirectoryUtil directoryUtil)
        {
            _concreteParser = concreteParser;
            _directoryUtil = directoryUtil;

            _taskQueue = new ConcurrentQueue<Action>();
        }

        public async Task<IProject> ParseAsync(string directory)
        {
            IProject project = new Project();

            var filenames = _directoryUtil.GetFileNames(directory, "java");
            
            List<Task> taskList = new List<Task>();

            foreach (var filename in filenames)
            {
                _taskQueue.Enqueue(() => _concreteParser.Parse(filename, project));
            }

            for(int i = 0; i < Environment.ProcessorCount; i++)
            {
                taskList.Add(Task.Run(() =>
                                    {
                                        Action task = null;

                                        while (_taskQueue.TryDequeue(out task))
                                        {
                                            task.Invoke();
                                        }
                                    }
                ));
            }

            await Task.WhenAll(taskList);

            if (taskList.Any(x => x.IsFaulted))
            {
                project = null;
            }
            else
            {
                project.FindClassRelations();
            }

            return project;
        }
    }
}
