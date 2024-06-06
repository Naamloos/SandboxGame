using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Api.Assets;
using SandboxGame.Api.Camera;
using SandboxGame.Api.Units;
using SandboxGame.Engine;
using SandboxGame.Engine.Assets;
using SandboxGame.Engine.Cameras;
using SandboxGame.Engine.Entity;
using SandboxGame.Engine.Input;
using System.Linq;

namespace SandboxGame.Entities
{
    public class Npc : BaseEntity, ICameraTarget
    {
        public override RectangleUnit Bounds
        {
            get
            {
                return new RectangleUnit(Position.X, Position.Y, 32, 32);
            }
        }

        public override PointUnit Position { get; set; }

        public override bool IsWorldEntity => true;

        public override bool IsInteractable => !inDialog && FindEntitiesNearby(250, x => x.GetType() == typeof(Player)).Any();

        private ILoadedSprite sprite;
        private SpriteFont dialogFont;
        private ILoadedSprite dialogTicker;

        private const string NPC_NAME = "Sign";
        private const string NPC_DIALOG = "Welcome to SandboxGame!\nThere's not much yet, but there might be!\nMaybe one day!";

        private Camera camera;
        private MouseHelper mouseHelper;
        private InputHelper inputHelper;
        private AssetManager assetManager;
        private EntityManager entityManager;

        public Npc(AssetManager assetManager, Camera camera, MouseHelper mouseHelper, EntityManager entityManager) : base()
        {
            this.camera = camera;
            this.mouseHelper = mouseHelper;
            this.assetManager = assetManager;
            this.entityManager = entityManager;

            this.sprite = assetManager.GetSprite("nature_sign");
            this.dialogFont = assetManager.GetFont("main");
            this.dialogTicker = assetManager.GetSprite("dialog");
        }

        public override void OnClientDraw()
        {
            sprite.Draw(Bounds, interactable: this.IsInteractable);
        }

        private float oldZoom = 0;
        private bool inDialog = false;
        public override void OnClientClick()
        {
            if(!inDialog)
            {
                inDialog = true;
                camera.Follow(this);

                entityManager.SpawnDialog(NPC_NAME, NPC_DIALOG, this, () =>
                {
                    inDialog = false;
                });
            }
        }

        public override void OnClientTick()
        {
            sprite.Update();
        }
    }
}
