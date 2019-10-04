using System;

namespace Corsinvest.AllenBradley.PLC.Api
{
    /// <summary>
    /// Tag operation exception
    /// </summary>
    [System.Serializable]
    public class TagOperationException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public TagOperationException(ResultOperation result) : base("Error execute operation!")
        {
            Result = result;
        }

        /// <summary>
        /// Result operation
        /// </summary>
        /// <value></value>
        public ResultOperation Result { get; }
    }
}