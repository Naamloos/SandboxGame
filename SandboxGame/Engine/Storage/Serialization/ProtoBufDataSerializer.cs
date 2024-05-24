using ProtoBuf;
using System;
using System.IO;

namespace SandboxGame.Engine.Storage.Serialization
{
    public class ProtoBufDataSerializer : IDataSerializer
    {
        public string FileExtension => "bin";

        public T DeserializeDataFromStream<T>(Stream stream)
        {
            if (stream.Length < 1)
            {
                return Activator.CreateInstance<T>();
            }
            return Serializer.Deserialize<T>(stream);
        }

        public void SerializeDataToStream<T>(T data, Stream stream)
        {
            Serializer.Serialize<T>(stream, data);
        }
    }
}
