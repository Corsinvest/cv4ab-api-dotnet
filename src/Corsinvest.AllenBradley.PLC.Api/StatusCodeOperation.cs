using System.Runtime.InteropServices;

namespace Corsinvest.AllenBradley.PLC.Api
{
    /// <summary>
    /// Status code operation.
    /// </summary>
    public static class StatusCodeOperation
    {
        /// <summary>
        /// Operation in progress. Not an error.
        /// </summary>
        public const int STATUS_PENDING = 1;

        /// <summary>
        /// No error.
        /// </summary>
        public const int STATUS_OK = 0;

        /// <summary>
        /// A null pointer was found during processing. Often this is returned when an argument is null.
        /// </summary>
        public const int ERR_NULL_PTR = -1;

        /// <summary>
        /// An attempt to access a value outside of the allow limits was made. 
        /// Usually this is in conjunction with accessing a data word in a tag.
        /// </summary>
        public const int ERR_OUT_OF_BOUNDS = -2;

        /// <summary>
        /// Unable to allocate memory.
        /// </summary>
        public const int ERR_NO_MEM = -3;

        /// <summary>
        /// Unable to add to a linked list internally.
        /// </summary>
        public const int ERR_LL_ADD = -4;

        /// <summary>
        /// Illegal or unknown parameter value.
        /// </summary>
        public const int ERR_BAD_PARAM = -5;

        /// <summary>
        /// Error creating a tag or internal value.
        /// </summary>
        public const int ERR_CREATE = -6;

        /// <summary>
        /// 
        /// </summary>
        public const int ERR_NOT_EMPTY = -7;

        /// <summary>
        /// Error opening a socket or other OS-level item.
        /// </summary>
        public const int ERR_OPEN = -8;

        /// <summary>
        /// Error setting socket options or similar.
        /// </summary>
        public const int ERR_SET = -9;

        /// <summary>
        /// Error writing.
        /// </summary>
        public const int ERR_WRITE = -10;

        /// <summary>
        /// Operation did not complete in the time allowed.
        /// </summary>
        public const int ERR_TIMEOUT = -11;

        /// <summary>
        /// Did not receive an ACK in the time allowed.
        /// </summary>
        public const int ERR_TIMEOUT_ACK = -12;

        /// <summary>
        /// Exceeded allowed number of retries.
        /// </summary>
        public const int ERR_RETRIES = -13;

        /// <summary>
        /// Error reading.
        /// </summary>
        public const int ERR_READ = -14;

        /// <summary>
        /// Garbled or unexpected response from remote system.
        /// </summary>
        public const int ERR_BAD_DATA = -15;

        /// <summary>
        /// Unable to encode part of the transaction.
        /// </summary>
        public const int ERR_ENCODE = -16;

        /// <summary>
        /// Unable to decode part of the returned transaction.
        /// </summary>
        public const int ERR_DECODE = -17;

        /// <summary>
        /// Unsupported operation (i.e. tag type does not support the operation).
        /// </summary>
        public const int ERR_UNSUPPORTED = -18;

        /// <summary>
        /// Argument too long. Usually a string or name.
        /// </summary>
        public const int ERR_TOO_LONG = -19;

        /// <summary>
        /// Error closing a socket or similar OS construct.
        /// </summary>
        public const int ERR_CLOSE = -20;

        /// <summary>
        /// Operation not permitted.
        /// </summary>
        public const int ERR_NOT_ALLOWED = -21;

        /// <summary>
        /// Unable to set up background thread.
        /// </summary>
        public const int ERR_THREAD = -22;

        /// <summary>
        /// No data received.
        /// </summary>
        public const int ERR_NO_DATA = -23;

        /// <summary>
        /// Unable to join with thread.
        /// </summary>
        public const int ERR_THREAD_JOIN = -24;

        /// <summary>
        /// Unable to create thread.
        /// </summary>
        public const int ERR_THREAD_CREATE = -25;

        /// <summary>
        /// Error while attempting to destroy OS-level mutex.
        /// </summary>
        public const int ERR_MUTEX_DESTROY = -26;

        /// <summary>
        /// Error while attempting to unlock mutex.
        /// </summary>
        public const int ERR_MUTEX_UNLOCK = -27;

        /// <summary>
        /// Error while attempting to initialize mutex.
        /// </summary>
        public const int ERR_MUTEX_INIT = -28;

        /// <summary>
        /// Error while attempting to lock mutex.
        /// </summary>
        public const int ERR_MUTEX_LOCK = -29;

        /// <summary>
        /// Tag operation not implemented.
        /// </summary>
        public const int ERR_NOT_IMPLEMENTED = -30;

        /// <summary>
        /// Illegal or unknown value for device type (CPU).
        /// </summary>
        public const int ERR_BAD_DEVICE = -31;

        /// <summary>
        /// Garbled or unknown gateway IP.
        /// </summary>
        public const int ERR_BAD_GATEWAY = -32;

        /// <summary>
        /// Error reported from remote end.
        /// </summary>
        public const int ERR_REMOTE_ERR = -33;

        /// <summary>
        /// Operation failed due to target object not found.
        /// </summary>
        public const int ERR_NOT_FOUND = -34;

        /// <summary>
        /// Operation aborted.
        /// </summary>
        public const int ERR_ABORT = -35;

        /// <summary>
        /// (Windows only) Error initializing/terminating use of Windows sockets.
        /// </summary>
        public const int ERR_WINSOCK = -36;

        /// <summary>
        /// Check code in error
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool IsError(int code) { return (code != STATUS_PENDING && code != STATUS_OK); }

        /// <summary>
        /// Decode error.
        /// </summary>
        /// <param name="code">Error code</param>
        /// <returns></returns>
        public static string DecodeError(int code) { return Marshal.PtrToStringAnsi(NativeMethod.plc_tag_decode_error(code)); }
    }
}