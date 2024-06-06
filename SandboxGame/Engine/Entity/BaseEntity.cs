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
    public abstract class BaseEntity : BaseClientEntity
    {
        public abstract RectangleUnit Bounds { get; }

        public abstract PointUnit Position { get; set; }

        public abstract void OnClientTick();

        public abstract void OnClientDraw();

        public IClientEntityManager EntityManager 
        {
            get;
            set;
        }

        public abstract bool IsWorldEntity { get; }

        public abstract bool IsInteractable { get; }

        public virtual RenderLayer ClientRenderLayer => RenderLayer.Foreground;

        public void SetPosition(PointUnit position)
        {
            Position = position;
        }

        public IEnumerable<BaseClientEntity> FindEntitiesNearby(float distance, Func<BaseClientEntity, bool> searchParams)
        {
            return EntityManager.FindEntitiesNearby(this, distance, searchParams);
        }

        public virtual void OnClientClick()
        {
            
        }
    }
}
