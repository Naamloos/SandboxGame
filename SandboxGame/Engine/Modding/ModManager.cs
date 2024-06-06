using Microsoft.Extensions.Logging;
using SandboxGame.Api;
using SandboxGame.Engine.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Engine.Modding
{
    public class ModManager
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IStorageSupplier _storage;
        private readonly ILogger<ModManager> _logger;

        private List<Type> _modTypes = new List<Type>();
        private List<IClientModLoader> _loadedMods = new List<IClientModLoader>();

        public ModManager(IServiceProvider serviceProvider, IStorageSupplier storage, ILogger<ModManager> logger)
        {
            this._serviceProvider = serviceProvider;
            this._storage = storage;
            this._logger = logger;

            LoadMods();
            InitializeMods();
        }

        public void LoadMods()
        {
            _logger.LogInformation("Pre-Loading mods...");
            var mods = _storage.GetModStreams();
            foreach(var mod in mods)
            {
                using var ms = new MemoryStream();
                mod.CopyTo(ms);
                var asm = ms.GetBuffer();
                var modAssembly = Assembly.Load(asm); // Load the assembly from stream

                var availableMods = modAssembly.GetTypes().Where(t => typeof(IModLoader).IsAssignableFrom(t)).ToList();
                _modTypes.AddRange(availableMods);
            }
            _logger.LogInformation("Pre-loaded mods.");
        }

        public void InitializeMods()
        {
            _logger.LogInformation("Initializing mods...");
            foreach(var modType in _modTypes)
            {
                var parameters = modType.GetConstructors().First().GetParameters().Select(x => _serviceProvider.GetService(x.ParameterType)).ToArray();
                var mod = (IClientModLoader)Activator.CreateInstance(modType, parameters);
                mod.OnClientLoad();
                var metadata = mod.GetMetadata();
                _logger.LogInformation($"Loaded mod {metadata.Name} {metadata.Version} by {metadata.Author} - {metadata.Description}");
                _loadedMods.Add(mod);
            }
            _logger.LogInformation("Done mod initialization");
        }

        public void UnloadMods()
        {
            foreach(var mod in _loadedMods)
            {
                mod.OnClientUnload();
                var metadata = mod.GetMetadata();
                _logger.LogInformation($"Unloaded mod {metadata.Name}");
                _loadedMods.Remove(mod);
            }
        }

        public void WorldLoaded()
        {
            foreach(var mod in _loadedMods)
            {
                mod.OnClientWorldLoad();
            }
        }

        public void WorldDraw()
        {
            foreach(var mod in _loadedMods)
            {
                mod.OnClientWorldDraw();
            }
        }

        public void WorldUpdate()
        {
            foreach(var mod in _loadedMods)
            {
                mod.OnClientWorldTick();
            }
        }
    }
}
