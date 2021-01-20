using System;
using System.ComponentModel;
using System.Threading;
using log4net;

namespace DotNetToolBox.Extensions
{
    public static class BackgroundWorkerExtensions
    {

        private static readonly ILog log = LogManager.GetLogger(nameof(BackgroundWorkerExtensions));

        /// <summary>
        /// Executes the <see cref="BackgroundWorker.CancelAsync"/> function of the <see cref="worker"/>
        /// and waits until the <see cref="BackgroundWorker.IsBusy"/> is <see langword="false"/>. The <see cref="waitInterval"/> in
        /// milliseconds determines the time between the <see cref="BackgroundWorker.IsBusy"/> checks and the <see cref="maxTimeout"/>
        /// is the maximum time to wait for completion.
        /// </summary>
        /// <param name="worker">The <see cref="BackgroundWorker"/> being cancelled.</param>
        /// <param name="waitInterval">The time (in ms) to wait between status checks after cancelling to <see cref="BackgroundWorker"/>.
        /// The <see cref="waitInterval"/> <c>must</c> be between 100 and 2500 ms, or .1 and 2.5 seconds.</param>
        /// <param name="maxTimeout">The maximum allotted time (in ms) to wait after cancelling to <see cref="BackgroundWorker"/>.
        /// If set, the <see cref="maxTimeout"/> <c>must</c> be greater than 100 ms (.1 seconds).</param>
        public static void CancelAndWait(this BackgroundWorker worker, int waitInterval = 250, int? maxTimeout = null)
        {
            if (worker == null) throw new ArgumentNullException(nameof(worker));
            if (!worker.IsBusy) return;

            //  if we enabled cancellation (before starting the thread) and there's
            //  no cancellation currently pending, cancel the worker asynchronously
            if (worker.WorkerSupportsCancellation && !worker.CancellationPending)
            {
                worker.CancelAsync();
            }

            //  ensure the wait interval isn't less than 100 ms (.1 seconds)
            if (waitInterval < 100)
            {
                throw new ArgumentOutOfRangeException(nameof(waitInterval), waitInterval, $"The {nameof(waitInterval)} cannot be less than 100 ms (.1 seconds).");
            }

            //  ensure the wait interval isn't greater than 2500 ms (2.5 seconds)
            if (waitInterval > 2500)
            {
                throw new ArgumentOutOfRangeException(nameof(waitInterval), waitInterval, $"The {nameof(waitInterval)} cannot be greater than 2500 ms (2.5 seconds).");
            }

            if (maxTimeout.HasValue)
            {
                //  ensure the timeout isn't less than 100 ms (.1 seconds)
                if (maxTimeout < 100)
                {
                    throw new ArgumentOutOfRangeException(nameof(maxTimeout), maxTimeout, $"The {nameof(maxTimeout)} cannot be less than 100 ms (.1 seconds).");
                }
            }

            var startedOn = DateTime.Now;
            var getElapsedTime = new Func<TimeSpan>(() => DateTime.Now - startedOn);
            var isMaxTimeoutReached = new Func<bool>(() =>
            {
                if (maxTimeout == null) return false;
                var max = (int) maxTimeout;
                var elapsed = getElapsedTime.Invoke();
                var elapsedMs = (decimal) elapsed.TotalMilliseconds;
                return elapsedMs >= max;
            });

            while (worker.IsBusy)
            {
                var elapsed = getElapsedTime.Invoke();
                var message = $"{nameof(BackgroundWorker)} is still pending cancellation after {elapsed:ss.ff seconds}.";
                log.Debug(message);
                
                var isMaxTimeout = isMaxTimeoutReached.Invoke();
                if (isMaxTimeout)
                {
                    var failedMessage = $"Cancellation of the {nameof(BackgroundWorker)} failed because the maximum timeout of {maxTimeout} ms elapsed.";
                    throw new TimeoutException(failedMessage);
                }

                Thread.Sleep(waitInterval);
            }

            var totalElapsed = getElapsedTime.Invoke();
            var completionMessage = $"{nameof(BackgroundWorker)} is cancellation completed after {totalElapsed:ss.ff seconds}.";
            log.Debug(completionMessage);
        }

    }
}
