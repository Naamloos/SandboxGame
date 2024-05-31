using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Api.Assets
{
    public struct SpriteMetadata
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public TimeSpan AnimationDuration { get; set; }

        public SpriteMetadata(int width, int height, TimeSpan? AnimationDuration = null) 
        {
            Width = width;
            Height = height;
            this.AnimationDuration = AnimationDuration ?? TimeSpan.FromSeconds(1);
        }
    }
}
