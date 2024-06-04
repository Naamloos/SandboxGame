using ProtoBuf;

namespace SandboxGame.Server.Serialization
{
    public class ProtoBufPacketSerializer : IPacketSerializer
    {
        public object Deserialize(Type type, byte[] data)
        {
            var ms = new MemoryStream(data);
            ms.Position = 0;
            ms.SetLength(data.Length);
            return Serializer.Deserialize(type, new MemoryStream(data));
        }

        public byte[] Serialize(object obj)
        {
            using var ms = new MemoryStream();
            Serializer.NonGeneric.Serialize(ms, obj);
            ms.Position = 0;
            return ms.ToArray();
        }
    }
}
