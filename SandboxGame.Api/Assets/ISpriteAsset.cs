using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Api.Assets
{
    public interface ISpriteAsset
    {
        /// <summary>
        /// Defines the amount of frames this sprite has.
        /// </summary>
        public int Frames { get; }

        /// <summary>
        /// Returns the frame at the given index.
        /// </summary>
        /// <param name="frame">Frame id, 0 to <see cref="Frames"/> - 1</param>
        /// <returns>Asset stream</returns>
        public Stream GetFrame(int frame);

        public SpriteMetadata GetMetadata();
    }
}
