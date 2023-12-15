using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Engine;
using SandboxGame.Engine.Assets;
using SandboxGame.Engine.Cameras;
using SandboxGame.Engine.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Entities
{
    public class Player : BaseEntity
    {
        public override Rectangle Bounds
        {
            get
            {
                return new Rectangle(Position.ToPoint(), new Point(48, 48));
            }
        }

        public override Vector2 Position { get; set; } = Vector2.Zero;

        private float speed = 200;

        private bool movesRight = true;

        private Sprite headSprite;
        private Sprite bodySprite;
        private Sprite handSprite;
        private Sprite leftFootSprite;
        private Sprite rightFootSprite;

        public Player(GameContext gameContext) : base(gameContext)
        {
            headSprite = GameContext.AssetManager.GetSprite("player_head").Copy();
            bodySprite = GameContext.AssetManager.GetSprite("player_body").Copy();
            handSprite = GameContext.AssetManager.GetSprite("player_hand").Copy();
            leftFootSprite = GameContext.AssetManager.GetSprite("player_foot_l").Copy();
            rightFootSprite = GameContext.AssetManager.GetSprite("player_foot_r").Copy();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Point center = Bounds.Center;

            Point bodyPos = new Point(center.X - bodySprite.Width / 2, center.Y - bodySprite.Height / 2);
            Point headPos = new Point(center.X - headSprite.Width / 2, (bodyPos.Y + 1) - (headSprite.Height));
            Point leftHand = new Point((center.X - (bodySprite.Width / 2)) - (handSprite.Width - 1), bodyPos.Y);
            Point rightHand = new Point((center.X + (bodySprite.Width / 2)) - 1, bodyPos.Y);
            Point leftFoot = new Point(center.X - (bodySprite.Width / 2), center.Y + (bodySprite.Height / 4));
            Point rightFoot = new Point((center.X + (bodySprite.Width / 2)) - leftFootSprite.Width, center.Y + (bodySprite.Height / 4));

            bodySprite.Draw(spriteBatch, bodyPos.X, bodyPos.Y, flip: !movesRight);
            headSprite.Draw(spriteBatch, headPos.X, headPos.Y, flip: !movesRight);

            handSprite.Draw(spriteBatch, leftHand.X, leftHand.Y);
            handSprite.Draw(spriteBatch, rightHand.X, rightHand.Y);
            rightFootSprite.Draw(spriteBatch, leftFoot.X, leftFoot.Y);
            leftFootSprite.Draw(spriteBatch, rightFoot.X, rightFoot.Y);
        }

        public override void Update(GameTime gameTime)
        {
            headSprite.Update(gameTime);
            bodySprite.Update(gameTime);
            handSprite.Update(gameTime);
            leftFootSprite.Update(gameTime);
            rightFootSprite.Update(gameTime);

            var frameTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            var distanceTraveled = (speed / 1000) * frameTime;

            DebugHelper.SetDebugValues("SPEED", distanceTraveled.ToString());

            if (GameContext.Camera.Target == this)
            {
                float x = Position.X;
                float y = Position.Y;

                if (GameContext.InputHelper.Left && !GameContext.InputHelper.Right)
                {
                    x = x - distanceTraveled;
                    movesRight = true;
                }

                if (GameContext.InputHelper.Right && !GameContext.InputHelper.Left)
                {
                    x = (int)(x + distanceTraveled);
                    movesRight = false;
                }

                if (GameContext.InputHelper.Up && !GameContext.InputHelper.Down)
                {
                    y = (int)(y - distanceTraveled);
                }

                if (GameContext.InputHelper.Down && !GameContext.InputHelper.Up)
                {
                    y = (int)(y + distanceTraveled);
                }

                Position = new Vector2(x, y);
            }
        }
    }
}
