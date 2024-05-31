using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Api
{
    public enum RenderLayer
    {
        /// <summary>
        /// Anything rendered behind everything else, even the world.
        /// </summary>
        Background,

        /// <summary>
        /// Render layer for the worlds entities, including features like trees, rocks, etc.
        /// </summary>
        World,

        /// <summary>
        /// Render layer for the player and other entities that are rendered in front of the world.
        /// </summary>
        Foreground,

        /// <summary>
        /// Render layer for effects, such as particles.
        /// </summary>
        Effects,

        /// <summary>
        /// User interface render layer.
        /// </summary>
        UserInterface
    }
}
