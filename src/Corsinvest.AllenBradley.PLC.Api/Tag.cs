using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Corsinvest.AllenBradley.PLC.Api
{
    /// <summary>
    /// Tag base definition
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    public sealed class Tag<TType> : ITag<TType>, IDisposable
    {
        private bool _disposed = false;

        /// <summary>
        /// Event changed value
        /// </summary>
        public event EventHandlerOperation Changed;

        private Tag() { }

        /// <summary>
        /// Creates a tag. If the CPU type is LGX, the port type and slot has to be specified.
        /// </summary>
        /// <param name="controller">Controller reference</param>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol.
        /// E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        /// <param name="size">The size of an element in bytes. The tag is assumed to be composed of elements of the same size.
        /// For structure tags, use the total size of the structure.</param>
        /// <param name="length">elements count: 1- single, n-array.</param>
        /// <param name="debugLevel"></param>
        internal Tag(Controller controller, string name, int size, int length = 1, int debugLevel = 0)
        {
            Controller = controller;
            Name = name;
            Size = size;
            Length = length;
            ValueManager = new TagValueManager(this);

            var url = $"protocol=ab_eip&gateway={controller.IPAddress}";
            if (!string.IsNullOrEmpty(controller.Path)) { url += $"&path={controller.Path}"; }
            url += $"&cpu={controller.CPUType}&elem_size={Size}&elem_count={Length}&name={Name}";
            if (debugLevel > 0) { url = $"&debug={debugLevel}"; }

            Value = TagHelper.CreateObject<TType>(Length);

            //create reference
            Handle = NativeMethod.plc_tag_create(url);
        }

        /// <summary>
        /// Handle creation Tag
        /// </summary>
        /// <value></value>
        public IntPtr Handle { get; }

        /// <summary>
        /// Controller reference.
        /// </summary>
        /// <value></value>
        public Controller Controller { get; }

        /// <summary>
        /// The textual name of the tag to access. The name is anything allowed by the protocol. 
        /// E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The size of an element in bytes. The tag is assumed to be composed of elements of the same size.For structure tags, 
        /// use the total size of the structure.
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// elements length: 1- single, n-array.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Value manager
        /// </summary>
        /// <value></value>
        public TagValueManager ValueManager { get; }

        /// <summary>
        /// Old value tag.
        /// </summary>
        /// <value></value>
        public object OldValue { get; private set; }

        private TType _value;
        /// <summary>
        /// Value tag.
        /// </summary>
        /// <value></value>
        public TType Value
        {
            get => (TType)ValueManager.Get(_value, 0);
            set
            {
                _value = value;
                ValueManager.Set(value, 0);
            }
        }

        object ITag.Value
        {
            get => Value;
            set => Value = (TType)value;
        }

        /// <summary>
        /// Indicates whether or not a value must be read from the PLC.
        /// </summary>
        /// <value></value>
        public bool IsRead { get; private set; } = false;

        /// <summary>
        /// Indicates whether or not a value must be write to the PLC.
        /// </summary>
        public bool IsWrite { get; private set; } = false;

        /// <summary>
        /// Indicate if Value changed OldValue 
        /// </summary>
        /// <value></value>
        public bool IsChangedValue
        {
            get
            {
                using (var streamOldValue = new MemoryStream())
                using (var streamValue = new MemoryStream())
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(streamOldValue, OldValue);
                    streamOldValue.Seek(0, SeekOrigin.Begin);

                    formatter.Serialize(streamValue, Value);
                    streamValue.Seek(0, SeekOrigin.Begin);

                    return GetHashCode(streamOldValue.ToArray()) != GetHashCode(streamValue.ToArray());
                }
            }
        }

        /// <summary>
        /// Peforms read of Tag
        /// </summary>
        /// <returns></returns>
        public ResultOperation Read()
        {
            //save old value
            OldValue = DeepClone(Value);

            var timestamp = DateTime.Now;
            var watch = Stopwatch.StartNew();
            var statusCode = NativeMethod.plc_tag_read(Handle, Controller.Timeout);
        
            watch.Stop();
            IsRead = true;

            var result = new ResultOperation(this, timestamp, watch.ElapsedMilliseconds, statusCode);

            //check raise exception
            if (Controller.FailOperationRaiseException && StatusCodeOperation.IsError(statusCode))
            {
                throw new TagOperationException(result);
            }

            //event change value
            if (IsChangedValue) { Changed?.Invoke(result); }

            return result;
        }

        private static int GetHashCode(byte[] data)
        {
            if (data == null) { return 0; }

            var i = data.Length;
            var hc = i + 1;

            while (--i >= 0)
            {
                hc *= 257;
                hc ^= data[i];
            }

            return hc;
        }

        private static T DeepClone<T>(T obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// Peforms write of Tag 
        /// </summary>
        /// <returns></returns>
        public ResultOperation Write()
        {
            var timestamp = DateTime.Now;
            var watch = Stopwatch.StartNew();
            var statusCode = NativeMethod.plc_tag_write(Handle, Controller.Timeout);
            watch.Stop();
            IsWrite = true;

            var result = new ResultOperation(this, timestamp, watch.ElapsedMilliseconds, statusCode);

            //check raise exception
            if (Controller.FailOperationRaiseException && StatusCodeOperation.IsError(statusCode))
            {
                throw new TagOperationException(result);
            }

            return result;
        }

        /// <summary>
        /// Get size tag read from PLC.
        /// </summary>
        /// <returns></returns>
        public int GetSize() { return NativeMethod.plc_tag_get_size(Handle); }

        /// <summary>
        /// Get status operation. <see cref="StatusCodeOperation"/>
        /// </summary>
        /// <returns></returns>
        public int GetStatus() { return NativeMethod.plc_tag_status(Handle); }

        /// <summary>
        /// Lock for multitrading. <see cref="StatusCodeOperation"/>
        /// </summary>
        /// <returns></returns>
        public int Lock() { return NativeMethod.plc_tag_lock(Handle); }

        /// <summary>
        /// Unlock for multitrading <see cref="StatusCodeOperation"/>
        /// </summary>
        /// <returns></returns>
        public int Unlock() { return NativeMethod.plc_tag_unlock(Handle); }

        #region IDisposable Support
        void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing) { NativeMethod.plc_tag_destroy(Handle); }
                _disposed = true;
            }
        }

        /// <summary>
        /// Destructor
        /// </summary>
        /// <returns></returns>
        ~Tag() { Dispose(false); }

        /// <summary>
        /// Dispose object
        /// </summary>
        public void Dispose() { Dispose(true); }
        #endregion
    }
}