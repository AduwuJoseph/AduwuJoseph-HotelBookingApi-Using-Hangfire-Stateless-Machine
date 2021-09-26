using Hangfire.Common;
using Hangfire.Logging;
using Hangfire.States;
using Hangfire.Storage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.Api.Helpers
{
    public sealed class FileLogProvider : ILog, ILogProvider, IDisposable
    {
        private readonly Task _writeTask;
        private readonly BlockingCollection<string> _writeQueue;

        public FileLogProvider()
        {
            _writeQueue = new BlockingCollection<string>();
            _writeTask = Task.Factory.StartNew(() =>
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "hangfire.log");

                foreach (var contents in _writeQueue.GetConsumingEnumerable())
                {
                    try
                    {
                        File.AppendAllText(path, contents);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        public bool Log(LogLevel logLevel, Func<string> messageFunc, Exception exception = null)
        {
            var message = messageFunc?.Invoke();
            if (string.IsNullOrEmpty(message))
            {
                return true;
            }
            var text = $"{DateTime.Now:hh:mm:ss:fff} - [{logLevel}] {message}\r\n";
            Console.Write(text);
            _writeQueue.Add(text);
            return true;
        }

        public ILog GetLogger(string name)
        {
            return this;
        }

        public void Dispose()
        {
            _writeQueue.CompleteAdding();
            _writeQueue?.Dispose();
            _writeTask.Wait(2000);
        }
    }
    public class LogFailureAttribute : JobFilterAttribute, IApplyStateFilter
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            var failedState = context.NewState as FailedState;
            if (failedState != null)
            {
                Logger.ErrorException(
                    String.Format("Background job #{0} was failed with an exception.", context.JobId),
                    failedState.Exception);
            }
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
        }
    }
}
