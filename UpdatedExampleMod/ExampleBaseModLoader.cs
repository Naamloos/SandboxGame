using SandboxGame.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdatedExampleMod
{
    /// <summary>
    /// Just so I can re-use this for the server and client implementation.
    /// If you want, you can separately implement this for server and client,
    /// so you can for example make it say "My Mod (Server)" and "My Mod (Client)".
    /// </summary>
    public abstract class ExampleBaseModLoader : IModLoader
    {
        public ModMetadata GetMetadata() => new ModMetadata()
        {
            Name = "UpdatedExampleMod",
            Author = "Naamloos",
            Version = "0.0.1",
            Description = "An updated example mod."
        };
    }
}
