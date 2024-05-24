using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Engine.Storage.Serialization
{
    public interface IDataSerializer
    {
        public void SerializeDataToStream<T>(T data, Stream stream);


        public T DeserializeDataFromStream<T>(Stream stream);

        public string FileExtension { get; }
    }
}
