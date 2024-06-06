using SandboxGame.Api;
using SandboxGame.Api.Entity;
using SandboxGame.Api.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdatedExampleMod.Entities;

namespace UpdatedExampleMod
{
    public class ExampleServerModLoader : ExampleBaseModLoader, IServerModLoader
    {
        private IServerEntityManager _entityManager;
        public ExampleServerModLoader(IServerEntityManager entityManager)
        {
            _entityManager = entityManager;
        }

        public void OnServerLoad()
        {
            // Register our server-side stuff
            _entityManager.RegisterEntity<ExampleServerEntity>();
        }

        public void OnServerUnload()
        {

        }

        public void OnServerWorldLoad()
        {
            // We want to spawn an entity any time the world loads.
            _entityManager.SpawnEntity<ExampleServerEntity>(PointUnit.Zero);
            // I do not recommend spawning persistent entities like this, but it's just an example.
        }

        public void OnServerWorldTick()
        {

        }
    }
}
