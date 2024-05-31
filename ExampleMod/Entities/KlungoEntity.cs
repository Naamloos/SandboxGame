using ExampleMod.Sprites;
using SandboxGame.Api;
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
    public class KlungoEntity : IEntity
    {
        public RectangleUnit Bounds => new RectangleUnit(Position.X, Position.Y, klungoSprite.Width, klungoSprite.Height);

        public PointUnit Position { get; set; } = new PointUnit(-100, -100);

        public bool IsWorldEntity => true;

        public IEntityManager EntityManager { get; set; }

        public bool Interactable => true;

        public RenderLayer RenderLayer => RenderLayer.Foreground;

        private ILoadedSprite klungoSprite;

        public KlungoEntity(IAssetManager assetManager) 
        {
            this.klungoSprite = assetManager.GetSprite<KlungoSprite>();
        }

        public void Draw()
        {
            klungoSprite.Draw((int)Position.X, (int)Position.Y, interactable: this.Interactable); // TODO accept float values
        }

        public void SetPosition(PointUnit position)
        {
            this.Position = position;
        }

        public void Update()
        {
            // Update logic here
            // Randomize position from -100, -100 to 100, 100
            Position = new PointUnit(new Random().Next(-100, 100), new Random().Next(-100, 100));
        }

        public void OnClick()
        {

        }
    }
}
