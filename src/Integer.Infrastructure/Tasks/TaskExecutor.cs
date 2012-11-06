using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Raven.Client;
using System.Threading.Tasks;

namespace Integer.Infrastructure.Tasks
{
    public static class TaskExecutor
    {
        private static readonly ThreadLocal<List<BackgroundTask>> tasksToExecute =
            new ThreadLocal<List<BackgroundTask>>(() => new List<BackgroundTask>());

        public static IDocumentStore DocumentStore
        {
            get { return documentStore; }
            set
            {
                if (documentStore == null)
                    documentStore = value;
            }
        }
        private static IDocumentStore documentStore;

        public static Action<Exception> ExceptionHandler { get; set; }

        public static void ExcuteLater(BackgroundTask task)
        {
            tasksToExecute.Value.Add(task);
        }

        public static void Discard()
        {
            tasksToExecute.Value.Clear();
        }

        public static void StartExecuting()
        {
            var value = tasksToExecute.Value;
            var copy = value.ToArray();
            value.Clear();

            if (copy.Length > 0)
            {
                Task.Factory.StartNew(() =>
                {
                    foreach (var backgroundTask in copy)
                    {
                        ExecuteTask(backgroundTask);
                    }
                }, TaskCreationOptions.LongRunning)
                    .ContinueWith(task =>
                    {
                        if (ExceptionHandler != null) ExceptionHandler(task.Exception);
                    }, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        public static void ExecuteTask(BackgroundTask task)
        {
            for (var i = 0; i < 10; i++)
            {
                using (var session = documentStore.OpenSession())
                {
                    switch (task.Run(session))
                    {
                        case true:
                        case false:
                            return;
                        case null:
                            break;
                    }
                }
            }
        }
    }
}
