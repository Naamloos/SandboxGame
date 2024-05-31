using Microsoft.Xna.Framework;
using SandboxGame.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Engine
{
    public class GameTimeHelper : IGameTimeHelper
    {
        public TimeSpan ElapsedGameTime => gameTime.ElapsedGameTime;

        public TimeSpan TotalGameTime => gameTime.TotalGameTime;

        public bool IsRunningSlowly => gameTime.IsRunningSlowly;

        private GameTime gameTime;

        public void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;
        }
    }
}
