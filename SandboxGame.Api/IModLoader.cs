using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Api
{
    public interface IModLoader
    {
        public ModMetadata GetMetadata();
    }
}
