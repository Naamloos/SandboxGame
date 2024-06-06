using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Api
{
    public interface IServerModLoader : IModLoader
    {
        /// <summary>
        /// Gets called when the server is being loaded. Use this to load any server-side assets or data.
        /// </summary>
        public void OnServerLoad();

        /// <summary>
        /// Gets called when the server unloads. If anything of yours needs disposing, this is the place to do it.
        /// </summary>
        public void OnServerUnload();

        /// <summary>
        /// Gets called when the world is done loading.
        /// </summary>
        public void OnServerWorldLoad();

        /// <summary>
        /// Gets called on every world tick.
        /// </summary>
        public void OnServerWorldTick();
    }
}
