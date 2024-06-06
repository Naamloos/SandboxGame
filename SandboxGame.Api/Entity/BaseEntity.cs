using SandboxGame.Api.Attributes;
using SandboxGame.Api.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Api.Entity
{
    /// <summary>
    /// Base implementation for all entities in the game.
    /// Properties in this class are synchronized by the server to all clients.
    /// </summary>
    public abstract class BaseEntity
    {
        internal delegate void SyncEntityDataHandler(BaseEntity entity);
        internal event SyncEntityDataHandler? SynchronizeEntityValue;

        /// <summary>
        /// The unique identifier of the entity.
        /// this is used by the game implementation to identify the entity.
        /// </summary>
        public ulong EntityId => _entityId;
        internal ulong _entityId;

        /// <summary>
        /// Identifies which client owns this entity and thus has control over it.
        /// Empty if the server owns the entity.
        /// In most cases, a player has it's own client id as owner id, and everything else is owned by the server.
        /// If you don't own the entity, you can't change it's synchronized properties. 
        /// These are <see cref="Bounds"/> and <see cref="IsInteractable"/>. (More later)
        /// </summary>
        public string OwnerId => _ownerId;
        internal string _ownerId;

        /// <summary>
        /// Position of this entity in the world. Also used for e.g. collision detection.
        /// </summary>
        public RectangleUnit Bounds
        {

            get => _bounds;
            
            set
            {
                _bounds = value;
                SynchronizeEntityValue?.Invoke(this);
            }
        }
        internal RectangleUnit _bounds;

        /// <summary>
        /// Whether this entity may be interacted with or not.
        /// </summary>
        public bool IsInteractable
        {

            get => _isInteractable;

            set
            {
                _isInteractable = value;
                SynchronizeEntityValue?.Invoke(this);
            }
        }
        internal bool _isInteractable;

        public void SetPosition(float x, float y)
        {
            // This to ensure that the bounds are updated and the server/client is notified of the change.
            this.Bounds = new RectangleUnit(x, y, this.Bounds.Width, this.Bounds.Height);
        }
    }
}
