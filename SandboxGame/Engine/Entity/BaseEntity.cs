using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Api;
using SandboxGame.Api.Camera;
using SandboxGame.Api.Entity;
using SandboxGame.Api.Units;
using SandboxGame.Engine;
using SandboxGame.Engine.Cameras;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SandboxGame.Engine.Entity
{
    public abstract class BaseEntity : IEntity
    {
        public abstract RectangleUnit Bounds { get; }

        public abstract PointUnit Position { get; set; }

        public abstract void Update();

        public abstract void Draw();

        public IEntityManager EntityManager 
        {
            get;
            set;
        }

        public abstract bool IsWorldEntity { get; }

        public void SetPosition(PointUnit position)
        {
            Position = position;
        }

        public IEnumerable<IEntity> FindEntitiesNearby(float distance, Func<IEntity, bool> searchParams)
        {
            return EntityManager.FindEntitiesNearby(this, distance, searchParams);
        }
    }
}
