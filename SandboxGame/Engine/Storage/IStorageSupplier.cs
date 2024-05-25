using SandboxGame.Engine.Storage.Serialization;
using SandboxGame.WorldGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Engine.Storage
{
    public interface IStorageSupplier
    {
        public void StoreConfigFile<T>(string name, T data);

        public T ReadConfigFile<T>(string name);

        public Chunk ReadChunk(string world, int x, int y);

        public void WriteChunkFile(string world, int x, int y, Chunk chunk);

        public WorldInfo ReadWorldMetadata(string world);

        public void WriteWorldMetadata(string world, WorldInfo worldInfo);

        public void DeleteWorld(string world);
    }
}
