using SandboxGame.Engine.Storage.Serialization;
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
    }
}
