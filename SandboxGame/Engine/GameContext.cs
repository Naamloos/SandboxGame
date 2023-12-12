﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Engine
{
    public class GameContext
    {
        public SceneManager SceneManager { get; set; }
        public AssetManager AssetManager { get; set; }
        public Camera Camera { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public GameWindow GameWindow { get; set; }
        public MouseHelper MouseHelper { get; set; }
        public InputHelper InputHelper { get; set; }

        public GameContext()
        {
            // No initialization.
        }
    }
}
