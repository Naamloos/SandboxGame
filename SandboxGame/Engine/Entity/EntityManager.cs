using Microsoft.Xna.Framework;
using SandboxGame.Api;
using SandboxGame.Api.Entity;
using SandboxGame.Api.Units;
using SandboxGame.Engine.Assets;
using SandboxGame.Engine.Cameras;
using SandboxGame.Engine.Input;
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

        private MouseHelper _mouseHelper;
        private Camera _camera;

        public EntityManager(IServiceProvider serviceProvider, MouseHelper mouseHelper, Camera _camera) 
        {
            this._serviceProvider = serviceProvider;
            this._mouseHelper = mouseHelper;
            this._camera = _camera;
        }

        public T SpawnEntity<T>() where T : IEntity
        {
            var args = typeof(T).GetConstructors().First().GetParameters().Select(x => _serviceProvider.GetService(x.ParameterType)).ToArray();

            var entity = (T)Activator.CreateInstance(typeof(T), args);

            entity.EntityManager = this;

            _loadedEntities.Add(entity);

            // re-order render layers
            _loadedEntities = _loadedEntities.OrderBy(x => x.RenderLayer).ToList();

            return entity;
        }

        public void UpdateEntities()
        {
            // ensure no race conditions
            var entities = new List<IEntity>();
            entities.AddRange(_loadedEntities);

            for (var i = 0; i < entities.Count; i++)
            {
                entities[i].Update();

                if (entities[i].Interactable)
                {
                    var isUI = entities[i].RenderLayer == RenderLayer.UserInterface;
                    var intersects = isUI ? _mouseHelper.ScreenPos.AsRectangle().Intersects(entities[i].Bounds)
                        : _mouseHelper.WorldPos.AsRectangle().Intersects(entities[i].Bounds);

                    if (intersects && _mouseHelper.LeftClick)
                    {
                        entities[i].OnClick();
                    }
                }
            }
        }

        public void DrawEntities()
        {
            // ensure no race conditions
            var entities = new List<IEntity>();
            entities.AddRange(_loadedEntities);

            for (var i = 0; i < entities.Count; i++)
            {
                if (entities[i].RenderLayer == RenderLayer.UserInterface)
                {
                    _camera.DrawToUI(() => entities[i].Draw());
                }
                else
                {
                    entities[i].Draw();
                }
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
