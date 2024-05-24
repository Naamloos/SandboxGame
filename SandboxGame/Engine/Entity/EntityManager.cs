using Microsoft.Xna.Framework;
using SandboxGame.Engine.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Engine.Entity
{
    public class EntityManager
    {
        private IServiceProvider _serviceProvider;

        private List<BaseEntity> _loadedEntities = new List<BaseEntity>();

        public EntityManager(IServiceProvider serviceProvider) 
        {
            this._serviceProvider = serviceProvider;
        }

        public T SpawnEntity<T>() where T : BaseEntity
        {
            var args = typeof(T).GetConstructors().First().GetParameters().Select(x => _serviceProvider.GetService(x.ParameterType)).ToArray();

            var entity = (T)Activator.CreateInstance(typeof(T), args);

            entity.EntityManager = this;

            _loadedEntities.Add(entity);

            return entity;
        }

        public void UnloadEntity(BaseEntity entity)
        {
            _loadedEntities.Remove(entity);
        }

        /// <summary>
        /// Be wary: This method only unloads them from the entity manager. It's up to you to remove them from your scene!
        /// </summary>
        public void UnloadAllEntities() 
        {
            _loadedEntities.Clear();
        }

        public IEnumerable<BaseEntity> FindEntitiesNearby(BaseEntity entity, float distance, Func<BaseEntity, bool> searchParams)
            => _loadedEntities.Where(x => x.IsWorldEntity && x != entity && searchParams(x) && Vector2.Distance(entity.Position, x.Position) < distance);
    }
}
