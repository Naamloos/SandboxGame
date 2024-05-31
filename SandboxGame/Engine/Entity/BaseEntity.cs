using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Api.Camera;
using SandboxGame.Api.Units;
using SandboxGame.Engine;
using SandboxGame.Engine.Cameras;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SandboxGame.Engine.Entity
{
    public abstract class BaseEntity : ICameraTarget
    {
        public abstract RectangleUnit Bounds { get; }

        public abstract Vector2 Position { get; set; }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);

        public EntityManager EntityManager { get
            {
                return _entityManager;
            }
            set
            {
                if(EntityManager != null)
                {
                    throw new InvalidOperationException("EntityManager was already set!");
                }

                _entityManager = value;
            }
        }
        private EntityManager _entityManager = null;

        public abstract bool IsWorldEntity { get; }

        public void SetPosition(Vector2 position)
        {
            Position = position;
        }

        public IEnumerable<BaseEntity> FindEntitiesNearby(float distance, Func<BaseEntity, bool> searchParams)
        {
            return EntityManager.FindEntitiesNearby(this, distance, searchParams);
        }
    }
}
