using SandboxGame.Api.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Api.Entity
{
    public interface IEntity
    {
        public RectangleUnit Bounds { get; }

        public PointUnit Position { get; set; }

        public bool IsWorldEntity { get; }

        public void Update();

        public void Draw();

        public void SetPosition(PointUnit position);

        public IEntityManager EntityManager { set; }
    }
}
