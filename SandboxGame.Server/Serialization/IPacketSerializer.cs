using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Server.Serialization
{
    public interface IPacketSerializer
    {
        public byte[] Serialize(object obj);

        public object Deserialize(Type type, byte[] data);
    }
}
