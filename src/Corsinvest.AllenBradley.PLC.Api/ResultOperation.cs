using System;
using System.Collections.Generic;
using System.Linq;

namespace Corsinvest.AllenBradley.PLC.Api
{
    /// <summary>
    /// Result operation
    /// </summary>
    public class ResultOperation
    {
        internal ResultOperation(ITag tag, DateTime timestamp, long executionTime, int statusCode)
        {
            Tag = tag;
            Timestamp = timestamp;
            ExecutionTime = executionTime;
            StatusCode = statusCode;
        }

        /// <summary>
        /// Tag
        /// </summary>
        /// <value></value>
        public ITag Tag { get; }

        /// <summary>
        /// Timestamp last operation
        /// </summary>
        /// <value></value>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Millisecond execution operatorion
        /// </summary>
        /// <value></value>
        public long ExecutionTime { get; }

        /// <summary>
        /// Returns the status code <see cref="StatusCodeOperation"/>
        /// STATUS_OK will be returned if the operation completed successfully.
        /// </summary>
        /// <value></value>
        public int StatusCode { get; }

        /// <summary>
        /// Reduce multimple result to one.
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public static ResultOperation Reduce(IEnumerable<ResultOperation> results)
        {
            return new ResultOperation(null,
                                       results.Min(a => a.Timestamp),
                                       results.Sum(a => a.ExecutionTime),
                                       results.Sum(a => a.StatusCode) != 0 ? results.Max(a => a.StatusCode) : 0);
        }
    }
}