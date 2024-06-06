using SandboxGame.Api.Camera;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Api.Entity
{
    public interface IClientEntityManager
    {
        public T SpawnEntity<T>() where T : BaseClientEntity;

        public BaseClientEntity SpawnDialog(string name, string content, ICameraTarget entity, Action whenDone = null);

        public IEnumerable<BaseClientEntity> FindEntities(Func<BaseClientEntity, bool> predicate);

        public IEnumerable<T> FindEntityOfType<T>() where T : BaseClientEntity;

        public void UnloadEntity(BaseClientEntity entity);

        public void UnloadAllEntities();

        public IEnumerable<BaseClientEntity> FindEntitiesNearby(BaseClientEntity entity, float distance, Func<BaseClientEntity, bool> searchParams);
    }
}
