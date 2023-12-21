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

        private Sprite sprite;
        private bool hovering;
        private SpriteFont dialogFont;
        private Sprite dialogTicker;

        private const string NPC_NAME = "Markiplier";
        private const string NPC_DIALOG = "Hello everybody, my name is Markiplier I am just chileng on the bech\nAnd welcome to Five Nights At Freddy's\nHar Har HarHar Har";

        public Npc(GameContext gameContext, string spriteName, Vector2 position) : base(gameContext)
        {
            Position = position;

            this.sprite = GameContext.AssetManager.GetSprite(spriteName);
            this.dialogFont = GameContext.AssetManager.GetFont("main");
            this.dialogTicker = GameContext.AssetManager.GetSprite("dialog");
        }

        private Dialog dialog = null;

        public override void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, (int)Position.X, (int)Position.Y, hovering, camera: GameContext.Camera);

            if(dialog != null)
            {
                dialog.Draw(spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            hovering = Bounds.Intersects(new Rectangle(GameContext.MouseHelper.WorldPos.ToPoint(), new Point(1, 1)));

            if(hovering && GameContext.MouseHelper.LeftClick && dialog == null)
            {
                GameContext.Camera.Follow(this);

                dialog = new Dialog(NPC_NAME, NPC_DIALOG, GameContext, this,
                () =>
                {
                    dialog = null;
                    GameContext.Camera.StopFollowing();
                });
            }

            if (dialog != null)
            {
                dialog.Update(gameTime);
            }
        }
    }
}
