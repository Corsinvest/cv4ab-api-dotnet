namespace Corsinvest.AllenBradley.PLC.Api
{
    /// <summary>
    /// Scanning mode
    /// </summary>
    public enum ScanMode
    {
        /// <summary>
        /// Read value from PLC
        /// </summary>
        Read,

        /// <summary>
        /// Write value from PLC
        /// </summary>
        Write,

        /// <summary>
        /// Read and Write value from PLC
        /// </summary>
        ReadAndWrite
    }
}