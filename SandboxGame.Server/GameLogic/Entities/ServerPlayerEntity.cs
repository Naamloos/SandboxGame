using SandboxGame.Api.Attributes;
using SandboxGame.Api.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Server.GameLogic.Entities
{
    [EntityImplementation("player", 32, 32)]
    public class ServerPlayerEntity : BaseServerEntity
    {
    }
}
