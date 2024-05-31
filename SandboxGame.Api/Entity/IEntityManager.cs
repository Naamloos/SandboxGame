using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Api.Entity
{
    public interface IEntityManager
    {
        public T SpawnEntity<T>() where T : IEntity;

        public IEnumerable<IEntity> FindEntities(Func<IEntity, bool> predicate);

        public IEnumerable<T> FindEntityOfType<T>() where T : IEntity;

        public void UnloadEntity(IEntity entity);

        public void UnloadAllEntities();

        public IEnumerable<IEntity> FindEntitiesNearby(IEntity entity, float distance, Func<IEntity, bool> searchParams);
    }
}
