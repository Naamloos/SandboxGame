using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Api
{
    public interface IClientModLoader : IModLoader
    {
        /// <summary>
        /// Gets called when the client is being loaded. Use this to load any client-side assets or data.
        /// </summary>
        public void OnClientLoad();

        /// <summary>
        /// Gets called when the client unloads. If anything of yours needs disposing, this is the place to do it.
        /// </summary>
        public void OnClientUnload();

        /// <summary>
        /// Gets called when the world is done loading.
        /// </summary>
        public void OnClientWorldLoad();

        /// <summary>
        /// Gets called on every world draw frame.
        /// </summary>
        public void OnClientWorldDraw();

        /// <summary>
        /// Gets called on every world tick.
        /// </summary>
        public void OnClientWorldTick();
    }
}
