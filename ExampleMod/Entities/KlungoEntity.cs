using ExampleMod.Sprites;
using SandboxGame.Api;
using SandboxGame.Api.Assets;
using SandboxGame.Api.Camera;
using SandboxGame.Api.Entity;
using SandboxGame.Api.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleMod.Entities
{
    public class KlungoEntity : IEntity, ICameraTarget
    {
        public RectangleUnit Bounds => new RectangleUnit(Position.X, Position.Y, klungoSprite.Width, klungoSprite.Height);

        public PointUnit Position { get; set; } = new PointUnit(-100, -100);

        public bool IsWorldEntity => true;

        public IEntityManager EntityManager { get; set; }

        public bool Interactable => true;

        public RenderLayer RenderLayer => RenderLayer.Foreground;

        private ILoadedSprite klungoSprite;
        private ICamera _camera;

        public KlungoEntity(IAssetManager assetManager, ICamera camera) 
        {
            this.klungoSprite = assetManager.GetSprite<KlungoSprite>();
            this._camera = camera;
        }

        public void Draw()
        {
            klungoSprite.Draw((int)_spritePosition.X, (int)_spritePosition.Y, camera: _camera, interactable: this.Interactable); // TODO accept float values
        }

        public void SetPosition(PointUnit position)
        {
            this.Position = position;
        }

        PointUnit _spritePosition = new PointUnit(0, 0);
        public void Update()
        {
            // Update logic here
            // Randomize position from -100, -100 to 100, 100
            _spritePosition = new PointUnit(Position.X - new Random().Next(0, 3), Position.Y - new Random().Next(0, 3));
            klungoSprite.Update();
        }

        private bool dialog = false;
        public void OnClick()
        {
            if(!dialog)
            {
                dialog = true;
                EntityManager.SpawnDialog("Klungo", "Hello\nI am Klungo\nNice to meet you!\nI was added via a mod...\nBtw, why am I shaking like this?", this, () =>
                {
                    dialog = false;
                });
            }
        }
    }
}
