using SandboxGame.Api.Attributes;
using SandboxGame.Api.Entity;
using SandboxGame.Api.Units;
using SandboxGame.Server.Packets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Server.EntityManager
{
    public class ServerEntityManager : IServerEntityManager
    {
        private Dictionary<Type, EntityImplementationAttribute> registeredEntities = new Dictionary<Type, EntityImplementationAttribute>();
        private Dictionary<ulong, BaseServerEntity> loadedEntities = new Dictionary<ulong, BaseServerEntity>();
        private GameServer server;

        public ServerEntityManager(GameServer server) 
        { 
            this.server = server;
        }

        public void RegisterEntity<T>() where T : BaseServerEntity
        {
            var type = typeof(T);
            var implementation = type.GetCustomAttribute<EntityImplementationAttribute>();
            if(implementation == null)
            {
                throw new Exception("Entity must have EntityImplementationAttribute!");
            }

            if(registeredEntities.ContainsKey(type))
            {
                throw new Exception("Entity already registered!");
            }

            registeredEntities.Add(type, implementation);
        }

        public void UnregisterEntity<T>() where T : BaseServerEntity
        {
            if(registeredEntities.ContainsKey(typeof(T)))
                registeredEntities.Remove(typeof(T));
        }

        public BaseServerEntity SpawnEntity<T>(PointUnit Position) where T : BaseServerEntity
            => SpawnEntity<T>(new RectangleUnit(Position.X, Position.Y, registeredEntities[typeof(T)].Width, registeredEntities[typeof(T)].Height));

        public BaseServerEntity SpawnEntity<T>(RectangleUnit Bounds) where T : BaseServerEntity
        {
            var newEntity = (BaseServerEntity)Activator.CreateInstance(typeof(T))!;
            newEntity._bounds = Bounds;
            newEntity._ownerId = "";
            newEntity._entityId = getNextEntityId();
            // Notify clients of entity spawn, and then send entity data packet afterwards

            broadcastEntitySpawn(newEntity);
            broadcastEntityData(newEntity);

            return newEntity;
        }

        private ulong getNextEntityId()
        {
            ulong id = 0;
            while(loadedEntities.ContainsKey(id))
            {
                id++;
            }
            return id;
        }

        public void SetEntityOwner(ulong entityId, string ownerId = "")
        {
            if(loadedEntities.ContainsKey(entityId))
            {
                loadedEntities[entityId]._ownerId = ownerId;
            }

            // Notify clients of entity owner change
            var ownerPacket = new SetEntityOwnerPacket()
            {
                EntityId = entityId,
                Owner = ownerId
            };
        }

        public void DespawnEntity(BaseServerEntity Entity)
        {
            loadedEntities.Remove(Entity._entityId);
            // Notify clients of entity despawn
        }

        public void DespawnEntity(ulong EntityId)
        {
            DespawnEntity(loadedEntities[EntityId]);
        }

        public void Tick()
        {
            foreach (var entity in loadedEntities.Values)
            {
                entity.OnServerTick();
            }
        }

        private void broadcastEntitySpawn(BaseServerEntity entity)
        {
            var spawnPacket = new EntitySpawnPacket()
            {
                EntityId = entity._entityId,
                EntityType = entity.GetType().Name
            };
            server.SendPacketToAllClients(spawnPacket);
        }

        private void broadcastEntityData(BaseServerEntity entity)
        {
            var dataPacket = new EntityDataPacket(entity);
            server.SendPacketToAllClients(dataPacket);
        }
    }
}
