using Microsoft.Xna.Framework;
using SandboxGame.Api.Entity;
using SandboxGame.Api.Units;
using SandboxGame.Engine.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Engine.Entity
{
    public class EntityManager : IEntityManager
    {
        private IServiceProvider _serviceProvider;

        private List<IEntity> _loadedEntities = new List<IEntity>();

        public EntityManager(IServiceProvider serviceProvider) 
        {
            this._serviceProvider = serviceProvider;
        }

        public T SpawnEntity<T>() where T : IEntity
        {
            var args = typeof(T).GetConstructors().First().GetParameters().Select(x => _serviceProvider.GetService(x.ParameterType)).ToArray();

            var entity = (T)Activator.CreateInstance(typeof(T), args);

            entity.EntityManager = this;

            _loadedEntities.Add(entity);

            return entity;
        }

        public void UpdateEntities()
        {
            for (var i = 0; i < _loadedEntities.Count; i++)
            {
                _loadedEntities[i].Update();
            }
        }

        public void DrawEntities()
        {
            for (var i = 0; i < _loadedEntities.Count; i++)
            {
                _loadedEntities[i].Draw();
            }
        }

        public IEnumerable<IEntity> FindEntities(Func<IEntity, bool> predicate) => _loadedEntities.Where(predicate);

        public IEnumerable<T> FindEntityOfType<T>() where T : IEntity => _loadedEntities.Where(x => x.GetType() == typeof(T)).Cast<T>();

        public void UnloadEntity(IEntity entity)
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

        public IEnumerable<IEntity> FindEntitiesNearby(IEntity entity, float distance, Func<IEntity, bool> searchParams)
            => _loadedEntities.Where(x => x.IsWorldEntity && x != entity && searchParams(x) && PointUnit.Distance(entity.Position, x.Position) < distance);
    }
}
