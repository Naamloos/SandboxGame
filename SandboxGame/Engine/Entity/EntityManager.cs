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
    public class EntityManager : IClientEntityManager
    {
        private IServiceProvider _serviceProvider;

        private List<BaseClientEntity> _loadedEntities = new List<BaseClientEntity>();

        private MouseHelper _mouseHelper;
        private Camera _camera;

        public EntityManager(IServiceProvider serviceProvider, MouseHelper mouseHelper, Camera _camera) 
        {
            this._serviceProvider = serviceProvider;
            this._mouseHelper = mouseHelper;
            this._camera = _camera;
        }

        private List<BaseClientEntity> _queuedSpawns = new List<BaseClientEntity>();
        private List<BaseClientEntity> _queuedDespawns = new List<BaseClientEntity>();

        public T SpawnEntity<T>() where T : BaseClientEntity
        {
            var args = typeof(T).GetConstructors().First().GetParameters().Select(x => _serviceProvider.GetService(x.ParameterType)).ToArray();

            var entity = (T)Activator.CreateInstance(typeof(T), args);

            _queuedSpawns.Add(entity);

            return entity;
        }

        public BaseClientEntity SpawnDialog(string name, string content, ICameraTarget entity, Action whenDone = null)
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
            _loadedEntities = _loadedEntities.OrderBy(x => (int)x.ClientRenderLayer).ToList();
        }

        public void UpdateEntities()
        {
            var count = _loadedEntities.Count;
            for (var i = 0; i < count; i++)
            {
                _loadedEntities[i].OnClientTick();

                if (_loadedEntities[i].IsInteractable)
                {
                    var isUI = _loadedEntities[i].ClientRenderLayer == RenderLayer.UserInterface;
                    var intersects = isUI ? _mouseHelper.ScreenPos.AsRectangle().Intersects(_loadedEntities[i].Bounds)
                        : _mouseHelper.WorldPos.AsRectangle().Intersects(_loadedEntities[i].Bounds);

                    if (intersects && _mouseHelper.LeftClick)
                    {
                        _loadedEntities[i].OnClientClick();
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
                if (entity.ClientRenderLayer == RenderLayer.UserInterface)
                {
                    _camera.DrawToUI(() => entity.OnClientDraw());
                }
                else
                {
                    entity.OnClientDraw();
                }
            }
        }

        public IEnumerable<BaseClientEntity> FindEntities(Func<BaseClientEntity, bool> predicate) => _loadedEntities.Where(predicate);

        public IEnumerable<T> FindEntityOfType<T>() where T : BaseClientEntity => _loadedEntities.Where(x => x.GetType() == typeof(T)).Cast<T>();

        public void UnloadEntity(BaseClientEntity entity)
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

        public IEnumerable<BaseClientEntity> FindEntitiesNearby(BaseClientEntity entity, float distance, Func<BaseClientEntity, bool> searchParams)
            => _loadedEntities.Where(x => x != entity && searchParams(x) && PointUnit.Distance(entity.Bounds.Center, x.Bounds.Center) < distance);
    }
}
