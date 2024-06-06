using SandboxGame.Api.Attributes;
using SandboxGame.Api.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdatedExampleMod.Entities
{
    [EntityImplementation("example", 32, 32)]
    public class ExampleServerEntity : BaseServerEntity
    {
        public override void OnServerTick()
        {
            // We randomize this entity's position every server tick.
            this.SetPosition((float)Random.Shared.Next(0, 25), (float)Random.Shared.Next(0, 25));
        }
    }
}
