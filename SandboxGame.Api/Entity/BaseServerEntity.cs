using SandboxGame.Api.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Api.Entity
{
    public abstract class BaseServerEntity : BaseEntity
    {
        /// <summary>
        /// This method gets called on every server tick.
        /// </summary>
        public virtual void OnServerTick()
        {

        }
    }
}
