using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Engine;
using SandboxGame.Engine.Assets;
using SandboxGame.Engine.Cameras;
using SandboxGame.Engine.Entity;
using SandboxGame.Engine.Input;
using System.Linq;

namespace SandboxGame.Entities
{
    public class Player : BaseEntity
    {
        public override Rectangle Bounds
        {
            get
            {
                return new Rectangle(Position.ToPoint(), new Point(32, 32));
            }
        }

        public override Vector2 Position { get; set; } = Vector2.Zero;

        public override bool IsWorldEntity => true;

        private float speed = 345;

        private bool movesRight = true;

        private Sprite headSprite;
        private Sprite bodySprite;
        private Sprite handSprite;
        private Sprite leftFootSprite;
        private Sprite rightFootSprite;

        private Sprite player;
        private Sprite debugBox;

        SpriteBatch spriteBatch;
        Camera camera;
        InputHelper inputHelper;

        public Player(AssetManager assetManager, SpriteBatch spriteBatch, Camera camera, InputHelper inputHelper) : base()
        {
            this.spriteBatch = spriteBatch;
            this.camera = camera;
            this.inputHelper = inputHelper;
            //headSprite = GameContext.AssetManager.GetSprite("player_head");
            //bodySprite = GameContext.AssetManager.GetSprite("player_body");
            //handSprite = GameContext.AssetManager.GetSprite("player_hand");
            //leftFootSprite = GameContext.AssetManager.GetSprite("player_foot_l");
            //rightFootSprite = GameContext.AssetManager.GetSprite("player_foot_r");

            player = assetManager.GetSprite("player");

            debugBox = assetManager.GetSprite("debug");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Point center = Bounds.Center;

            //Point bodyPos = new Point(center.X - bodySprite.Width / 2, center.Y - bodySprite.Height / 2);
            //Point headPos = new Point(center.X - headSprite.Width / 2, (bodyPos.Y + 1) - (headSprite.Height));
            //Point leftHand = new Point((center.X - (bodySprite.Width / 2)) - (handSprite.Width - 1), bodyPos.Y);
            //Point rightHand = new Point((center.X + (bodySprite.Width / 2)) - 1, bodyPos.Y);
            //Point leftFoot = new Point(center.X - (bodySprite.Width / 2), center.Y + (bodySprite.Height / 4));
            //Point rightFoot = new Point((center.X + (bodySprite.Width / 2)) - leftFootSprite.Width, center.Y + (bodySprite.Height / 4));

            //bodySprite.Draw(spriteBatch, bodyPos.X, bodyPos.Y, flip: !movesRight);
            //headSprite.Draw(spriteBatch, headPos.X, headPos.Y, flip: !movesRight);

            //handSprite.Draw(spriteBatch, leftHand.X, leftHand.Y);
            //handSprite.Draw(spriteBatch, rightHand.X, rightHand.Y);
            //rightFootSprite.Draw(spriteBatch, leftFoot.X, leftFoot.Y);
            //leftFootSprite.Draw(spriteBatch, rightFoot.X, rightFoot.Y);

            player.Draw(spriteBatch, (int)Position.X, (int)Position.Y, flip: movesRight, rotation: walking? hop : 0f);
            debugBox.Draw(spriteBatch, (int)center.X, (int)center.Y);
        }

        float hop = 0f;
        bool hopDir = false;
        bool walking = false;

        public override void Update(GameTime gameTime)
        {
            //headSprite.Update(gameTime);
            //bodySprite.Update(gameTime);
            //handSprite.Update(gameTime);
            //leftFootSprite.Update(gameTime);
            //rightFootSprite.Update(gameTime);

            player.Update(gameTime);

            var frameTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            var distanceTraveled = (speed / 1000) * frameTime;

            DebugHelper.SetDebugValues("SPEED", distanceTraveled.ToString());

            var chatOpened = EntityManager.FindEntityOfType<ChatBox>().Any(x => x.IsActive);

            if (camera.Target == this && !chatOpened)
            {
                float x = Position.X;
                float y = Position.Y;

                walking = false;

                if (inputHelper.GetKeyDown("left") && inputHelper.GetKeyUp("right"))
                {
                    x = x - distanceTraveled;
                    movesRight = false;
                    walking = true;
                }

                if (inputHelper.GetKeyDown("right") && inputHelper.GetKeyUp("left"))
                {
                    x = x + distanceTraveled;
                    movesRight = true;
                    walking = true;
                }

                if (inputHelper.GetKeyDown("up") && inputHelper.GetKeyUp("down"))
                {
                    y = y - distanceTraveled;
                    walking = true;
                }

                if (inputHelper.GetKeyDown("down") && inputHelper.GetKeyUp("up"))
                {
                    y = y + distanceTraveled;
                    walking = true;
                }

                if(walking)
                {
                    if (hopDir)
                        hop += 0.05f;
                    else
                        hop -= 0.05f;

                    if (hop < -0.2f || hop > 0.2f)
                        hopDir = !hopDir;
                }

                // TODO normalize traveled distance to not have diagonal movement feel wrong.
                Position = new Vector2(x, y);
            }
        }
    }
}
