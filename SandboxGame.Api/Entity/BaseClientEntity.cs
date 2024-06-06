using SandboxGame.Api.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Api.Entity
{
    public abstract class BaseClientEntity : BaseEntity
    {
        /// <summary>
        /// Defines on what layer this entity should be drawn.
        /// </summary>
        public virtual RenderLayer ClientRenderLayer => RenderLayer.World;

        /// <summary>
        /// Gets called on every client tick.
        /// </summary>
        public virtual void OnClientTick() { }

        /// <summary>
        /// Gets called on every client draw frame.
        /// </summary>
        public virtual void OnClientDraw() { }

        /// <summary>
        /// Gets called when this entity is clicked on.
        /// </summary>
        public virtual void OnClientClick() { }
    }
}
