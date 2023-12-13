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

        private Sprite sprite;
        private InputHelper inputHelper;
        private Camera camera;

        private float speed = 200;

        private bool movesRight = true;

        public Player(Sprite player, InputHelper input, Camera camera)
        {
            sprite = player.Copy();
            inputHelper = input;
            this.camera = camera;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, (int)Position.X, (int)Position.Y, camera: camera, flip: !movesRight);
        }

        public override void Update(GameTime gameTime)
        {
            var frameTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            var distanceTraveled = (speed / 1000) * frameTime;

            DebugHelper.SetDebugValues("SPEED", distanceTraveled.ToString());

            if (camera.Target == this)
            {
                float x = Position.X;
                float y = Position.Y;

                if (inputHelper.Left && !inputHelper.Right)
                {
                    x = x - distanceTraveled;
                    movesRight = true;
                }

                if (inputHelper.Right && !inputHelper.Left)
                {
                    x = (int)(x + distanceTraveled);
                    movesRight = false;
                }

                if (inputHelper.Up && !inputHelper.Down)
                {
                    y = (int)(y - distanceTraveled);
                }

                if (inputHelper.Down && !inputHelper.Up)
                {
                    y = (int)(y + distanceTraveled);
                }

                Position = new Vector2(x, y);
            }
        }
    }
}
