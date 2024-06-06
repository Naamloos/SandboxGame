using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("SandboxGame.Server")]
[assembly: InternalsVisibleTo("SandboxGame.Client")]
namespace SandboxGame.Api
{
    public struct ModMetadata
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
    }
}
