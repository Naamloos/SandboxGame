using SandboxGame.Common.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Common
{
    public class PacketSerializer
    {
        public PacketSerializer() 
        { 
        
        }

        /// <summary>
        /// Tries to read a packet from a stream.
        /// </summary>
        /// <typeparam name="T">Type of packet to read</typeparam>
        /// <param name="stream">Stream to read from</param>
        /// <param name="packet">Packet that was read</param>
        /// <returns>Whether reading was succesful.</returns>
        public bool TryReadFromStream<T>(Stream stream, out T packet) where T : BasePacket
        {
            packet = default;
            return false; // TODO
        }

        /// <summary>
        /// Tries to write a packet to a stream.
        /// </summary>
        /// <typeparam name="T">Type of packet to write</typeparam>
        /// <param name="packet">Packet that is written</param>
        /// <param name="stream">Stream to write packet to</param>
        /// <returns>Whether writing was succesful.</returns>
        public bool TryWriteToStream<T>(T packet, Stream stream) where T : BasePacket
        {
            return false; // TODO
        }
    }
}
