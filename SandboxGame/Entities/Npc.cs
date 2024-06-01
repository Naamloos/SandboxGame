using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public override bool Interactable => !inDialog && FindEntitiesNearby(250, x => x.GetType() == typeof(Player)).Any();

        private LoadedSprite sprite;
        private SpriteFont dialogFont;
        private LoadedSprite dialogTicker;

        private const string NPC_NAME = "Markiplier";
        private const string NPC_DIALOG = "Hello everybody, my name is Markiplier\nAnd welcome to Five Nights At Freddy's\nHar Har HarHar Har";

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

            this.sprite = assetManager.GetSprite("markiplier");
            this.dialogFont = assetManager.GetFont("main");
            this.dialogTicker = assetManager.GetSprite("dialog");
        }

        public override void Draw()
        {
            sprite.Draw((int)Position.X, (int)Position.Y, camera: camera, interactable: this.Interactable);
        }

        private float oldZoom = 0;
        private bool inDialog = false;
        public override void OnClick()
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

        public override void Update()
        {
            sprite.Update();
        }
    }
}
