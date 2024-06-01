using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Api;
using SandboxGame.Api.Camera;
using SandboxGame.Api.Entity;
using SandboxGame.Api.Units;
using SandboxGame.Engine;
using SandboxGame.Engine.Assets;
using SandboxGame.Engine.Cameras;
using SandboxGame.Engine.Entity;
using SandboxGame.Engine.Input;
using System;

namespace SandboxGame.Entities
{
    public class Dialog : BaseEntity
    {
        private string _name;
        private string[] _dialog;
        private int _currentIndex = 0;
        private ICameraTarget _entity;

        private SpriteFont _font;
        private LoadedSprite _dialogTicker;

        private Vector2 _dialogPos = Vector2.Zero;
        private Vector2 _namePos = Vector2.Zero;
        private Vector2 _tickerPos = Vector2.Zero;

        private Action _onDialogDone;

        private Camera _camera;
        private InputHelper _inputHelper;
        private MouseHelper _mouseHelper;
        private SpriteBatch _spriteBatch;

        public Dialog(AssetManager assetManager, Camera camera, InputHelper inputHelper, MouseHelper mouseHelper, SpriteBatch spriteBatch)
        {
            _camera = camera;
            _inputHelper = inputHelper;
            _mouseHelper = mouseHelper;

            _font = assetManager.GetFont("main");
            _dialogTicker = assetManager.GetSprite("dialog");
            _spriteBatch = spriteBatch;
        }

        public void SetData(string name, string content, ICameraTarget entity, Action OnDialogDone = null)
        {
            _name = name;
            _dialog = content.Split('\n');
            _entity = entity;
            _onDialogDone = OnDialogDone;
        }

        bool firstTick = true;

        public override RenderLayer RenderLayer => RenderLayer.UserInterface;

        public override RectangleUnit Bounds => _camera.ScreenView;

        public override PointUnit Position { get => new PointUnit(0, 0); set { return; } }

        public override bool IsWorldEntity => false;

        public override bool Interactable => true;

        private float _oldZoom = 0;
        public override void Update()
        {
            if(firstTick)
            {
                _oldZoom = _camera.Zoom;
                _camera.FocusZoom(1.8f);
                _camera.Follow(_entity);
                return;
            }
            if (_currentIndex >= _dialog.Length)
            {
                return;
            }

            var nameSize = _font.MeasureString(_name);
            var dialogSize = _font.MeasureString(_dialog[_currentIndex]);

            var entityTopCenter = _camera.WorldToScreen(new PointUnit(_entity.Bounds.Center.X, _entity.Bounds.Top));
            _tickerPos = new Vector2(entityTopCenter.X - _dialogTicker.Width / 2, entityTopCenter.Y - 15 - _dialogTicker.Height);
            _dialogPos = new Vector2(entityTopCenter.X - dialogSize.X / 2, _tickerPos.Y - 15 - dialogSize.Y);
            _namePos = new Vector2(entityTopCenter.X - nameSize.X / 2, _dialogPos.Y - 15 - nameSize.Y);

            _dialogTicker.Update();
        }

        public override void OnClick()
        {
            _currentIndex++;
            if (_currentIndex >= _dialog.Length)
            {
                _camera.SetZoom(_oldZoom);
                _camera.StopFollowing();
                if (_onDialogDone is not null)
                {
                    _onDialogDone();
                    _onDialogDone = null;
                }
                EntityManager.UnloadEntity(this);
            }
        }

        public override void Draw()
        {
            if (_currentIndex >= _dialog.Length)
            {
                return;
            }

            _camera.DrawToUI(() =>
            {
                _spriteBatch.DrawString(_font, _dialog[_currentIndex], new Vector2(_dialogPos.X - 2, _dialogPos.Y - 2), Color.Black);
                _spriteBatch.DrawString(_font, _dialog[_currentIndex], new Vector2(_dialogPos.X + 2, _dialogPos.Y + 2), Color.Black);
                _spriteBatch.DrawString(_font, _dialog[_currentIndex], new Vector2(_dialogPos.X + 2, _dialogPos.Y - 2), Color.Black);
                _spriteBatch.DrawString(_font, _dialog[_currentIndex], new Vector2(_dialogPos.X - 2, _dialogPos.Y + 2), Color.Black);
                _spriteBatch.DrawString(_font, _dialog[_currentIndex], _dialogPos, Color.White);

                _spriteBatch.DrawString(_font, _name, new Vector2(_namePos.X - 2, _namePos.Y - 2), Color.Black);
                _spriteBatch.DrawString(_font, _name, new Vector2(_namePos.X + 2, _namePos.Y + 2), Color.Black);
                _spriteBatch.DrawString(_font, _name, new Vector2(_namePos.X + 2, _namePos.Y - 2), Color.Black);
                _spriteBatch.DrawString(_font, _name, new Vector2(_namePos.X - 2, _namePos.Y + 2), Color.Black);
                _spriteBatch.DrawString(_font, _name, _namePos, Color.Yellow);
                _dialogTicker.Draw((int)_tickerPos.X, (int)_tickerPos.Y);
            });
        }
    }
}
