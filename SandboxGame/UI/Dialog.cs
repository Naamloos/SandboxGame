using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Engine;
using SandboxGame.Engine.Assets;
using SandboxGame.Engine.Cameras;
using SandboxGame.Engine.Input;
using SandboxGame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.UI
{
    public class Dialog
    {
        private string _name;
        private string[] _dialog;
        private int _currentIndex = 0;
        private BaseEntity _entity;

        private GameContext _gameContext;

        private SpriteFont _font;
        private Sprite _dialogTicker;

        private Vector2 _dialogPos = Vector2.Zero;
        private Vector2 _namePos = Vector2.Zero;
        private Vector2 _tickerPos = Vector2.Zero;

        private Action _onDialogDone;

        public Dialog(string name, string dialog, GameContext gameContext, BaseEntity entity, Action OnDialogDone)
        {
            _name = name;
            _dialog = dialog.Split('\n');
            _entity = entity;
            _gameContext = gameContext;

            _font = _gameContext.AssetManager.GetFont("main");
            _dialogTicker = _gameContext.AssetManager.GetSprite("dialog");

            _onDialogDone = OnDialogDone;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_currentIndex >= _dialog.Length)
            {
                return;
            }

            _gameContext.Camera.DrawToUI(() =>
            {
                spriteBatch.DrawString(_font, _dialog[_currentIndex], new Vector2(_dialogPos.X - 2, _dialogPos.Y - 2), Color.Black);
                spriteBatch.DrawString(_font, _dialog[_currentIndex], new Vector2(_dialogPos.X + 2, _dialogPos.Y + 2), Color.Black);
                spriteBatch.DrawString(_font, _dialog[_currentIndex], new Vector2(_dialogPos.X + 2, _dialogPos.Y - 2), Color.Black);
                spriteBatch.DrawString(_font, _dialog[_currentIndex], new Vector2(_dialogPos.X - 2, _dialogPos.Y + 2), Color.Black);
                spriteBatch.DrawString(_font, _dialog[_currentIndex], _dialogPos, Color.White);

                spriteBatch.DrawString(_font, _name, new Vector2(_namePos.X - 2, _namePos.Y - 2), Color.Black);
                spriteBatch.DrawString(_font, _name, new Vector2(_namePos.X + 2, _namePos.Y + 2), Color.Black);
                spriteBatch.DrawString(_font, _name, new Vector2(_namePos.X + 2, _namePos.Y - 2), Color.Black);
                spriteBatch.DrawString(_font, _name, new Vector2(_namePos.X - 2, _namePos.Y + 2), Color.Black);
                spriteBatch.DrawString(_font, _name, _namePos, Color.Yellow);
                _dialogTicker.Draw(spriteBatch, (int)_tickerPos.X, (int)_tickerPos.Y);
            });
        }

        bool firstTick = true;
        public void Update(GameTime gameTime)
        {
            if (firstTick)
            {
                // mouse input hack
                firstTick = false;
                return;
            }

            if (_currentIndex >= _dialog.Length)
            {
                if (_onDialogDone is not null)
                {
                    _onDialogDone();
                    _onDialogDone = null;
                }
                return;
            }
            var nameSize = _font.MeasureString(_name);
            var dialogSize = _font.MeasureString(_dialog[_currentIndex]);

            var entityTopCenter = _gameContext.Camera.WorldToScreen(new Vector2(_entity.Bounds.Center.X, _entity.Bounds.Top));
            _tickerPos = new Vector2(entityTopCenter.X - (_dialogTicker.Width / 2), (entityTopCenter.Y - 15) - _dialogTicker.Height);
            _dialogPos = new Vector2(entityTopCenter.X - (dialogSize.X / 2), (_tickerPos.Y - 15) - dialogSize.Y);
            _namePos = new Vector2(entityTopCenter.X - (nameSize.X / 2), (_dialogPos.Y - 15) - nameSize.Y);

            if (_gameContext.MouseHelper.LeftClick || _gameContext.InputHelper.Interact)
            {
                _currentIndex++;
            }

            _dialogTicker.Update(gameTime);
        }
    }
}
