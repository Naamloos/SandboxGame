using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SandboxGame.Engine.Storage.Serialization
{
    public class JsonDataSerializer : IDataSerializer
    {
        public string FileExtension => "json";
        private JsonSerializerOptions _options;

        public JsonDataSerializer() 
        {
            _options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
        }

        public T DeserializeDataFromStream<T>(Stream stream)
        {
            if(stream.Length < 1)
            {
                return Activator.CreateInstance<T>();
            }
            return JsonSerializer.Deserialize<T>(stream, _options);
        }

        public void SerializeDataToStream<T>(T data, Stream stream)
        {
            JsonSerializer.Serialize(stream, data, options: _options);
        }
    }
}
