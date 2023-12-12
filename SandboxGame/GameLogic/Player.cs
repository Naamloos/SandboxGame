using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.GameLogic
{
    public class Player
    {
        public Sprite Sprite { get => sprite; }
        private Sprite sprite;
        private InputHelper inputHelper;

        private int x = 0;
        private int y = 0;
        private float speed = 200;

        private bool movesRight = true;

        public Player(Sprite player, InputHelper input)
        {
            sprite = player;
            inputHelper = input;
        }

        public void Update(GameTime gameTime, bool active = true)
        {
            var frameTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            var distanceTraveled = (speed / 1000) * frameTime;

            DebugHelper.SetDebugValues("SPEED", distanceTraveled.ToString());

            if (active)
            {
                if (inputHelper.Left && !inputHelper.Right)
                {
                    x = (int)(x - distanceTraveled);
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
            }
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            sprite.Draw(spriteBatch, x, y, camera: camera, flip: !movesRight);
        }
    }
}
