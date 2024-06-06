using SandboxGame.Api;
using SandboxGame.Api.Camera;
using SandboxGame.Api.Entity;
using SandboxGame.Api.Units;
using SandboxGame.Engine.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SandboxGame.Entities
{
    /// <summary>
    /// New expanded dialog entity, Supposed to replace the old dialog entity
    /// </summary>
    public class ExpandedDialog : BaseClientEntity
    {
        public RenderLayer ClientRenderLayer => RenderLayer.UserInterface;

        public RectangleUnit Bounds => _camera.ScreenView;

        public PointUnit Position { get => PointUnit.Zero; set { } }

        public bool IsWorldEntity => false;

        public bool IsInteractable => true;

        public IClientEntityManager EntityManager { get; set; }

        private ICamera _camera;

        public ExpandedDialog(ICamera _camera)
        {
            this._camera = _camera;
        }

        public void OnClientDraw()
        {

        }

        public void OnClientClick()
        {
            
        }

        public void SetPosition(PointUnit position)
        {
            
        }

        public void OnClientTick()
        {
            
        }
    }
}
