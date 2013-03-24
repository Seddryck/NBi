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
        /// Constructor for the performance result
        /// </summary>
        /// <param name="timeElapsed">Time elapsed in seconds</param>
        public PerformanceResult(TimeSpan timeElapsed)
        {
            TimeElapsed = timeElapsed;
        }
    }
}
