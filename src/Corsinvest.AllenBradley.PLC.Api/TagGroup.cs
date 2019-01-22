using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Corsinvest.AllenBradley.PLC.Api
{
    /// <summary>
    /// Group of Tags
    /// </summary>
    public class TagGroup : IDisposable
    {
        private bool _disposed;
        private Timer _timer;
        private List<ITag> _tags = new List<ITag>();
        private object _lockScan = new object();

        /// <summary>
        /// Event changed value
        /// </summary>
        public event EventHandlerOperations Changed;

        /// <summary>
        /// Event on timed scan.
        /// </summary>
        public event EventHandler OnTimedScan;

        private TagGroup() { }

        internal TagGroup(Controller controller)
        {
            Controller = controller;
            _timer = new Timer();
            _timer.Elapsed += OnTimedEvent;
        }

        #region Collection
        /// <summary>
        /// Tags
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<ITag> Tags { get { return _tags.AsReadOnly(); } }

        /// <summary>
        /// Add tag
        /// </summary>
        /// <param name="tag"></param>
        public void AddTag(ITag tag)
        {
            if (Tags.Contains(tag)) { throw new ArgumentException("Tag already exists in this collection!"); }
            if (!Controller.Tags.Contains(tag)) { throw new ArgumentException("Tag not in this controller"); }

            _tags.Add(tag);
        }

        /// <summary>
        /// Remove tag
        /// </summary>
        /// <param name="tag"></param>
        public void RemoveTag(ITag tag)
        {
            if (!Tags.Contains(tag)) { throw new ArgumentException("Tag not exists in this collection!"); }
            _tags.Remove(tag);
            CheckDisposeTag(tag);
        }

        /// <summary>
        /// Clears all Tags from the group
        /// </summary>
        public void ClearTags() { _tags.Clear(); }
        #endregion

        #region Create Tags
        /// <summary>
        /// Create Tag Int64
        /// </summary>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol.
        /// E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        /// <returns></returns>
        public Tag<long> CreateTagInt64(string name) { return CreateTagType<long>(name); }

        /// <summary>
        /// Create Tag UInt64
        /// </summary>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol.
        /// E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        /// <returns></returns>
        public Tag<ulong> CreateTagUInt64(string name) { return CreateTagType<ulong>(name); }

        /// <summary>
        /// Create Tag Int32
        /// </summary>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol.
        /// E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        /// <returns></returns>
        public Tag<int> CreateTagInt32(string name) { return CreateTagType<int>(name); }

        /// <summary>
        /// Create Tag UInt32
        /// </summary>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol.
        /// E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        /// <returns></returns>
        public Tag<uint> CreateTagUInt32(string name) { return CreateTagType<uint>(name); }

        /// <summary>
        /// Create Tag Int16
        /// </summary>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol.
        /// E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        /// <returns></returns>
        public Tag<short> CreateTagInt16(string name) { return CreateTagType<short>(name); }

        /// <summary>
        /// Create Tag UInt16
        /// </summary>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol.
        /// E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        /// <returns></returns>
        public Tag<ushort> CreateTagUInt16(string name) { return CreateTagType<ushort>(name); }

        /// <summary>
        /// Create Tag Int8
        /// </summary>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol.
        /// E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        /// <returns></returns>
        public Tag<sbyte> CreateTagInt8(string name) { return CreateTagType<sbyte>(name); }

        /// <summary>
        /// Create Tag UInt8
        /// </summary>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol.
        /// E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        /// <returns></returns>
        public Tag<byte> CreateTagUInt8(string name) { return CreateTagType<byte>(name); }

        /// <summary>
        /// Create Tag String
        /// </summary>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol.
        /// E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        /// <returns></returns>
        public Tag<string> CreateTagString(string name) { return CreateTagType<string>(name); }

        /// <summary>
        /// Create Tag Float32
        /// </summary>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol.
        /// E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        /// <returns></returns>
        public Tag<float> CreateTagFloat32(string name) { return CreateTagType<float>(name); }

        /// <summary>
        /// Create Tag Float64
        /// </summary>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol.
        /// E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        /// <returns></returns>
        public Tag<double> CreateTagFloat64(string name) { return CreateTagType<double>(name); }

        /// <summary>
        /// Create Tag custom Type Class
        /// </summary>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol.
        /// E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        /// <typeparam name="TCustomType">Class to create</typeparam>
        /// <returns></returns>
        public Tag<TCustomType> CreateTagType<TCustomType>(string name)
        {
            return CreateTagType<TCustomType>(name, TagSize.GetSizeObject(TagHelper.CreateObject<TCustomType>(1)));
        }

        /// <summary>
        /// Create Tag using free definition
        /// </summary>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol.
        /// E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        /// <param name="size">The size of an element in bytes. The tag is assumed to be composed of elements of the same size.
        /// For structure tags, use the total size of the structure.</param>
        /// <param name="length">elements count: 1- single, n-array.</param>
        /// <returns></returns>
        public Tag<TCustomType> CreateTagType<TCustomType>(string name, int size, int length = 1)
        {
            var tag = new Tag<TCustomType>(Controller, name, size, length);
            _tags.Add(tag);
            return tag;
        }

        /// <summary>
        /// Create Tag array.
        /// </summary>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol.
        /// E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        /// <param name="length">elements count: 1- single, n-array.</param>
        /// <typeparam name="TCustomType">Type to create</typeparam>
        /// <returns></returns>
        public Tag<TCustomType> CreateTagArray<TCustomType>(string name, int length)
            where TCustomType : IList
        {
            var type = typeof(TCustomType);
            if (!type.IsArray) { throw new ArgumentException("Is not array!"); }
            if (length <= 0) { throw new ArgumentException("Length > 0!"); }

            var obj = TagHelper.CreateObject<TCustomType>(length);
            return CreateTagType<TCustomType>(name, TagSize.GetSizeObject(obj[0]), length);
        }
        #endregion

        /// <summary>
        /// Enabled status.
        /// </summary>
        /// <value></value>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Controller
        /// </summary>
        /// <value></value>
        public Controller Controller { get; }

        /// <summary>
        /// Performs read of Group of Tags
        /// </summary>
        public IEnumerable<ResultOperation> Read(bool onlyChanged = false)
        {
            var results = Tags.Select(a => a.Read()).ToArray();
            var resultsOnlyChanged = results.Where(a => a.Tag.IsChangedValue);
            if (resultsOnlyChanged.Count() > 0) { Changed?.Invoke(resultsOnlyChanged); }

            return onlyChanged ? resultsOnlyChanged : results;
        }

        /// <summary>
        /// Performs write of Group of Tags
        /// </summary>
        public IEnumerable<ResultOperation> Write() { return this.Tags.Select(a => a.Write()).ToArray(); }

        /// <summary>
        /// Scan operation behavior of Tags
        /// </summary>
        /// <value></value>
        public ScanMode ScanMode { get; set; } = ScanMode.Read;

        /// <summary>
        /// Scanning update (refresh) interval in milliseconds
        /// </summary>
        /// <value></value>
        public double ScanInterval
        {
            get => _timer.Interval;
            set => _timer.Interval = value;
        }

        /// <summary>
        /// Begins background scanning of Tags
        /// </summary>
        public void ScanStart()
        {
            if (Enabled) { throw new Exception("Enabled group disabled!"); }
            _timer.Start();
        }

        /// <summary>
        /// Stops scanning from previously called ScanStart.  
        /// Terminates scan thread and frees any allocated resources.
        /// </summary>
        public void ScanStop() { _timer.Stop(); }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            lock (_lockScan)
            {
                switch (ScanMode)
                {
                    case ScanMode.Read: Read(); break;
                    case ScanMode.Write: Write(); break;
                    case ScanMode.ReadAndWrite: break;
                    default: break;
                }

                OnTimedScan?.Invoke(this, EventArgs.Empty);
            }
        }

        #region IDisposable Support
        private void CheckDisposeTag(ITag tag)
        {
            //if not in connroller dispose
            if (!Controller.Tags.Contains(tag)) { tag.Dispose(); }
        }

        void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    foreach (var tag in _tags.ToArray())
                    {
                        _tags.Remove(tag);
                        CheckDisposeTag(tag);
                    }
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// Destructor
        /// </summary>
        /// <returns></returns>
        ~TagGroup() { Dispose(false); }

        /// <summary>
        /// Dispose object
        /// </summary>
        public void Dispose() { Dispose(true); }
        #endregion
    }
}