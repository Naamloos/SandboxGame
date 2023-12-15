using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SandboxGame.Engine;
using SandboxGame.Engine.Assets;
using SandboxGame.Engine.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Scenes
{
    internal class MenuScene : BaseScene
    {
        private Sprite _grassTest;
        private SpriteFont _font;

        public override void Initialize()
        {
            _grassTest = GameContext.AssetManager.GetSprite("tile");
            _font = GameContext.AssetManager.GetFont("main");
            GameContext.Camera.SetSpeed(200f);
        }

        private bool mouseTouchesSprite = false;

        private int posX = 500;
        private bool goesLeft = false;

        public override void Update(GameTime gameTime)
        {
            _grassTest.Update(gameTime);

            mouseTouchesSprite = _grassTest.Bounds.Intersects(new Rectangle(GameContext.Camera.ScreenToWorld(Mouse.GetState(GameContext.GameWindow).Position.ToVector2()).ToPoint(),
                new Point(1, 1)));

            if (GameContext.MouseHelper.LeftClick && mouseTouchesSprite)
            {
                if(GameContext.Camera.IsFollowing)
                {
                    GameContext.Camera.StopFollowing(false);
                }
                else
                {
                    GameContext.Camera.Follow(_grassTest, smooth: false);
                }
            }

            if(posX > 700)
            {
                goesLeft = true;
            }
            else if( posX < 300)
            {
                goesLeft = false;
            }

            posX += goesLeft ? -5 : 5;
        }

        public override void Draw(GameTime gameTime)
        {
            GameContext.SpriteBatch.GraphicsDevice.SetRenderTarget(null);
            GameContext.SpriteBatch.GraphicsDevice.Clear(Color.SlateBlue);
            _grassTest.Draw(GameContext.SpriteBatch, posX, 200,
                camera: GameContext.Camera, bloom: mouseTouchesSprite);
            GameContext.SpriteBatch.DrawString(_font, "Drawn on Game layer", GameContext.Camera.ScreenCenter, Color.Yellow);

            GameContext.Camera.DrawToUI(() =>
            {
                var mouse = Mouse.GetState(GameContext.GameWindow);
                GameContext.SpriteBatch.DrawString(_font, "Drawn on UI layer (Camera Moving)", new Vector2(15, GameContext.Camera.ScreenCenter.Y),
                    mouse.LeftButton == ButtonState.Pressed ? Color.Red : Color.Beige);
            });
        }

        public override void Dispose()
        {

        }
    }
}
