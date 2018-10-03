using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace Corsinvest.AllenBradley.PLC.Api
{
    /// <summary>
    /// Controller
    /// </summary>
    public class Controller : IDisposable
    {
        private List<TagGroup> _tagGroups = new List<TagGroup>();

        private bool _disposed;

        /// <summary>
        /// Controller definition
        /// </summary>
        /// <param name="ipAddress">IP address of the gateway for this protocol.
        /// Could be the IP address of the PLC you want to access.</param>
        /// <param name="path">Required for LGX, Optional for PLC/SLC/MLGX IOI path to access the PLC from the gateway.
        /// <para></para>Communication Port Type: 1- Backplane, 2- Control Net/Ethernet, DH+ Channel A, DH+ Channel B, 3- Serial
        /// <para></para>Slot number where cpu is installed: 0,1.. </param>
        /// <param name="cpuType">AB CPU models</param>
        public Controller(string ipAddress, string path, CPUType cpuType)
        {
            if (cpuType == CPUType.LGX && string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("PortType and Slot must be specified for Controllogix / Compactlogix processors");
            }

            IPAddress = ipAddress;
            Path = path;
            CPUType = cpuType;
        }

        /// <summary>
        /// Raise Exception on failed operation
        /// </summary>
        /// <value></value>
        public bool FailOperationRaiseException { get; set; } = false;

        /// <summary>
        /// Communication timeout
        /// </summary>
        /// <value></value>
        public int Timeout { get; set; } = 5000;

        /// <summary>
        /// Groups
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<TagGroup> Groups { get { return _tagGroups.AsReadOnly(); } }

        /// <summary>
        /// All Tags
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<ITag> Tags { get { return _tagGroups.SelectMany(a => a.Tags).Distinct().ToList().AsReadOnly(); } }

        /// <summary>
        /// IP address of the gateway for this protocol. Could be the IP address of the PLC you want to access.
        /// </summary>
        public string IPAddress { get; }

        /// <summary>
        /// AB CPU models
        /// </summary>
        public CPUType CPUType { get; }

        /// <summary>
        /// Required for LGX, Optional for PLC/SLC/MLGX IOI path to access the PLC from the gateway.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Ping controller.
        /// </summary>
        /// <param name="echo">True echo result to standard output</param>
        /// <returns></returns>
        public bool Ping(bool echo = false)
        {
            using (var ping = new Ping())
            {
                var reply = ping.Send(IPAddress);
                if (echo)
                {
                    Console.WriteLine($"Address: {reply.Address}");
                    Console.WriteLine($"RoundTrip time: {reply.RoundtripTime}");
                    Console.WriteLine($"Time to live: {reply.Options?.Ttl}");
                    Console.WriteLine($"Don't fragment: {reply.Options?.DontFragment}");
                    Console.WriteLine($"Buffer size: {reply.Buffer?.Length}");
                    Console.WriteLine($"Status: {reply.Status}");
                }

                return reply.Status == IPStatus.Success;
            }
        }

        /// <summary>
        /// Creates new TagGroup 
        /// </summary>
        /// <returns></returns>
        public TagGroup CreateGroup()
        {
            var group = new TagGroup(this);
            _tagGroups.Add(group);
            return group;
        }

        #region IDisposable Support
        void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    foreach (var group in _tagGroups) { group.Dispose(); }
                    _tagGroups.Clear();
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// Destructor
        /// </summary>
        /// <returns></returns>
        ~Controller() { Dispose(false); }

        /// <summary>
        /// Dispose object
        /// </summary>
        public void Dispose() { Dispose(true); }
        #endregion
    }
}