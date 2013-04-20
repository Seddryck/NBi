using System;

namespace NBi.Core.Query
{
    /// <summary>
    /// This class manage the result of a Performance result
    /// </summary>
    public class PerformanceResult
    {
        /// <summary>
        /// Specify the time elapsed (in seconds) during execution of the query
        /// </summary>
        public TimeSpan TimeElapsed { get; private set; }

        /// <summary>
        /// Specify the time elapsed (in seconds) after which the TimeOut has occured
        /// </summary>
        public TimeSpan TimeOut { get; private set; }

        /// <summary>
        /// Specify if the query has timeout.
        /// </summary>
        public bool IsTimeOut { get; private set; }

        /// <summary>
        /// Constructor for the performance result
        /// </summary>
        /// <param name="timeElapsed">Time elapsed in seconds</param>
        public PerformanceResult(TimeSpan timeElapsed)
        {
            TimeElapsed = timeElapsed;
        }

        /// <summary>
        /// Constructor for the performance result
        /// </summary>
        internal PerformanceResult()
        {
        }

        internal static PerformanceResult Timeout(int timeout)
        {
            return new PerformanceResult() {IsTimeOut=true, TimeOut = new TimeSpan(timeout*1000*10)};
        }
    }
}
