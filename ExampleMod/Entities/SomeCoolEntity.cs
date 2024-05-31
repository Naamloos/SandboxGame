using ExampleMod.Assets;
using SandboxGame.Api.Assets;
using SandboxGame.Api.Entity;
using SandboxGame.Api.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleMod.Entities
{
    public class SomeCoolEntity : IEntity
    {
        public RectangleUnit Bounds => new RectangleUnit(Position.X, Position.Y, someCoolLoadedSprite.Width, someCoolLoadedSprite.Height);

        public PointUnit Position { get; set; } = new PointUnit(-100, -100);

        public bool IsWorldEntity => true;

        public IEntityManager EntityManager { get; set; }

        private ILoadedSprite someCoolLoadedSprite;

        public SomeCoolEntity(IAssetManager assetManager) 
        {
            this.someCoolLoadedSprite = assetManager.GetSprite<SomeCoolSprite>();
        }

        public void Draw()
        {
            someCoolLoadedSprite.Draw((int)Position.X, (int)Position.Y); // TODO accept float values
        }

        public void SetPosition(PointUnit position)
        {
            this.Position = position;
        }

        public void Update()
        {
            // Update logic here
        }
    }
}
