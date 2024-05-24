using SandboxGame.Engine.Assets;
using SandboxGame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Engine.Entity
{
    public class EntityManager
    {
        private IServiceProvider _serviceProvider;

        public EntityManager(IServiceProvider serviceProvider) 
        {
            this._serviceProvider = serviceProvider;
        }

        public T SpawnEntity<T>() where T : BaseEntity
        {
            var args = typeof(T).GetConstructors().First().GetParameters().Select(x => _serviceProvider.GetService(x.ParameterType)).ToArray();

            return (T)Activator.CreateInstance(typeof(T), args);
        }
    }
}
