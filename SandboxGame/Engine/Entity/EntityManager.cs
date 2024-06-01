using Microsoft.Xna.Framework;
using SandboxGame.Api;
using SandboxGame.Api.Camera;
using SandboxGame.Api.Entity;
using SandboxGame.Api.Units;
using SandboxGame.Engine.Assets;
using SandboxGame.Engine.Cameras;
using SandboxGame.Engine.Input;
using SandboxGame.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

        private List<IEntity> _queuedSpawns = new List<IEntity>();
        private List<IEntity> _queuedDespawns = new List<IEntity>();

        public T SpawnEntity<T>() where T : IEntity
        {
            var args = typeof(T).GetConstructors().First().GetParameters().Select(x => _serviceProvider.GetService(x.ParameterType)).ToArray();

            var entity = (T)Activator.CreateInstance(typeof(T), args);

            entity.EntityManager = this;
            _queuedSpawns.Add(entity);

            return entity;
        }

        public IEntity SpawnDialog(string name, string content, ICameraTarget entity, Action whenDone = null)
        {
            var dialog = SpawnEntity<Dialog>();
            dialog.SetData(name, content, entity, whenDone);
            return dialog;
        }

        // To ensure we don't modify the collection while iterating.
        private void flushSpawns()
        {
            _loadedEntities.AddRange(_queuedSpawns);
            _loadedEntities.RemoveAll(x => _queuedDespawns.Contains(x));
            _queuedSpawns.Clear();
            _queuedDespawns.Clear();
            // re-order render layers
            _loadedEntities = _loadedEntities.OrderBy(x => (int)x.RenderLayer).ToList();
        }

        public void UpdateEntities()
        {
            var count = _loadedEntities.Count;
            for (var i = 0; i < count; i++)
            {
                _loadedEntities[i].Update();

                if (_loadedEntities[i].Interactable)
                {
                    var isUI = _loadedEntities[i].RenderLayer == RenderLayer.UserInterface;
                    var intersects = isUI ? _mouseHelper.ScreenPos.AsRectangle().Intersects(_loadedEntities[i].Bounds)
                        : _mouseHelper.WorldPos.AsRectangle().Intersects(_loadedEntities[i].Bounds);

                    if (intersects && _mouseHelper.LeftClick)
                    {
                        _loadedEntities[i].OnClick();
                    }
                }
            }

            flushSpawns();
        }

        public void DrawEntities()
        {
            var entities = _loadedEntities.ToImmutableArray();
            for (var i = 0; i < entities.Length; i++)
            {
                var entity = entities[i];
                if (entity.RenderLayer == RenderLayer.UserInterface)
                {
                    _camera.DrawToUI(() => entity.Draw());
                }
                else
                {
                    entity.Draw();
                }
            }
        }

        public IEnumerable<IEntity> FindEntities(Func<IEntity, bool> predicate) => _loadedEntities.Where(predicate);

        public IEnumerable<T> FindEntityOfType<T>() where T : IEntity => _loadedEntities.Where(x => x.GetType() == typeof(T)).Cast<T>();

        public void UnloadEntity(IEntity entity)
        {
            _queuedDespawns.Add(entity);
        }

        /// <summary>
        /// Be wary: This method only unloads them from the entity manager. It's up to you to remove them from your scene!
        /// </summary>
        public void UnloadAllEntities() 
        {
            _queuedDespawns.AddRange(_loadedEntities);
        }

        public IEnumerable<IEntity> FindEntitiesNearby(IEntity entity, float distance, Func<IEntity, bool> searchParams)
            => _loadedEntities.Where(x => x.IsWorldEntity && x != entity && searchParams(x) && PointUnit.Distance(entity.Position, x.Position) < distance);
    }
}
