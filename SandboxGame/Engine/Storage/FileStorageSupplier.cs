using SandboxGame.Engine.Storage.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Engine.Storage
{
    public class FileStorageSupplier : IStorageSupplier
    {
        const string WORLDS_DIR = "worlds";
        const string ASSETS_DIR = "assets";
        const string CONFIG_DIR = "config";
        const string MODS_DIR = "mods";

        private string _dataPath;
        private string _worldsPath;
        private string _assetsPath;
        private string _configsPath;
        private string _modsPath;

        private IDataSerializer _configSerializer;
        private IDataSerializer _dataSerializer;

        public FileStorageSupplier(string dataPath = null) 
        {
            _configSerializer = new JsonDataSerializer();
            _dataSerializer = new ProtoBufDataSerializer();

            var currentAssemblyPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "data");
            var fullPath = Path.GetFullPath(dataPath ?? currentAssemblyPath);

            _dataPath = fullPath;
            _worldsPath = Path.Combine(_dataPath, WORLDS_DIR);
            _assetsPath = Path.Combine(_dataPath, ASSETS_DIR);
            _configsPath = Path.Combine(_dataPath, CONFIG_DIR);
            _modsPath = Path.Combine(_dataPath, MODS_DIR);

            // check if dir exists, else create it
            if(!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            // create game data directories if they don't exist yet
            if(!Directory.Exists(_worldsPath))
                Directory.CreateDirectory(_worldsPath);
            if (!Directory.Exists(_assetsPath))
                Directory.CreateDirectory(_assetsPath);
            if(!Directory.Exists(_configsPath))
                Directory.CreateDirectory(_configsPath);
            if(!Directory.Exists (_modsPath))
                Directory.CreateDirectory(_modsPath);
        }

        public void StoreConfigFile<T>(string name, T data)
        {
            var fullFilePath = Path.Combine(_configsPath, name + "." + _configSerializer.FileExtension);
            using var stream = getFileStream(fullFilePath);
            _configSerializer.SerializeDataToStream(data, stream);
        }

        public T ReadConfigFile<T>(string name)
        {
            var fullFilePath = Path.Combine(_configsPath, name + "." + _configSerializer.FileExtension);
            using var stream = getFileStream(fullFilePath, true);
            return _configSerializer.DeserializeDataFromStream<T>(stream);
        }

        private Stream getFileStream(string filePath, bool read = false) 
            => File.Exists(filePath) ? new FileStream(filePath, read? FileMode.Open : FileMode.Truncate, read? FileAccess.Read : FileAccess.Write) : File.Create(filePath);
    }
}
