using SandboxGame.Api.Units;

namespace SandboxGame.Api.Entity
{
    public interface IServerEntityManager
    {
        public void RegisterEntity<T>() where T : BaseServerEntity;

        public void UnregisterEntity<T>() where T : BaseServerEntity;

        public BaseServerEntity SpawnEntity<T>(RectangleUnit Bounds) where T : BaseServerEntity;
        
        public BaseServerEntity SpawnEntity<T>(PointUnit Position) where T : BaseServerEntity;

        public void DespawnEntity(BaseServerEntity Entity);

        public void DespawnEntity(ulong EntityId);
    }
}
