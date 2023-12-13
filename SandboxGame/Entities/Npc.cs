using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Engine;
using SandboxGame.Engine.Assets;
using SandboxGame.Engine.Cameras;
using SandboxGame.Engine.Input;
using SandboxGame.UI;
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
        private InputHelper inputHelper;
        private Sprite sprite;
        private bool hovering;
        private SpriteFont dialogFont;
        private Sprite dialogTicker;

        public Npc(Sprite sprite, Camera camera, Vector2 position, MouseHelper mouseHelper, InputHelper inputHelper, SpriteFont dialogFont, Sprite dialogTicker)
        {
            Position = position;
            this.camera = camera;
            this.sprite = sprite;
            this.mouseHelper = mouseHelper;
            this.inputHelper = inputHelper;
            this.dialogFont = dialogFont;
            this.dialogTicker = dialogTicker;
        }

        private Dialog dialog = null;

        public override void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, (int)Position.X, (int)Position.Y, hovering, camera: camera);

            if(dialog != null)
            {
                dialog.Draw(spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            hovering = Bounds.Intersects(new Rectangle(mouseHelper.WorldPos.ToPoint(), new Point(1, 1)));

            if(hovering && mouseHelper.LeftClick && dialog == null)
            {
                camera.Follow(this);
                dialog = new Dialog("Bob the NPC", "I do not think you're supposed to be here.\n" +
                    "However, I'll ask you this one question...\n" +
                    "Do you like my test room? :3", 
                    inputHelper, mouseHelper, dialogFont, dialogTicker, this, camera, () =>
                {
                    dialog = null;
                    camera.StopFollowing();
                });
            }

            if (dialog != null)
            {
                dialog.Update(gameTime);
            }
        }
    }
}
