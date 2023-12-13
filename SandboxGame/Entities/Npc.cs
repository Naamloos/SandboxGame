using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Entities
{
    public class Npc : BaseEntity
    {
        public override Rectangle Bounds
        {
            get
            {
                return new Rectangle(Position.ToPoint(), new Point(32, 32));
            }
        }

        public override Vector2 Position { get; set; }

        private Camera camera;
        private MouseHelper mouseHelper;
        private Sprite sprite;
        private bool hovering;
        private ICameraTarget oldTarget;

        public Npc(Sprite sprite, Camera camera, Vector2 position, MouseHelper mouseHelper)
        {
            Position = position;
            this.camera = camera;
            this.sprite = sprite;
            this.mouseHelper = mouseHelper;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, (int)Position.X, (int)Position.Y, hovering, camera: camera);
        }

        public override void Update(GameTime gameTime)
        {
            hovering = Bounds.Intersects(new Rectangle(mouseHelper.WorldPos.ToPoint(), new Point(1, 1)));

            if(hovering && mouseHelper.LeftClick)
            {
                if(oldTarget is null)
                {
                    oldTarget = camera.Target;
                    camera.Follow(this, true);
                }
                else
                {
                    camera.Follow(oldTarget, true);
                    oldTarget = null;
                }
            }
        }
    }
}
