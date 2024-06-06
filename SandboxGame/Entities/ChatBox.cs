using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using SandboxGame.Api;
using SandboxGame.Api.Assets;
using SandboxGame.Api.Units;
using SandboxGame.Connection;
using SandboxGame.Engine.Assets;
using SandboxGame.Engine.Cameras;
using SandboxGame.Engine.Entity;
using SandboxGame.Engine.Input;
using SandboxGame.Engine.Scenes;
using SandboxGame.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Entities
{
    public class ChatBox : BaseEntity
    {
        public override RectangleUnit Bounds => new RectangleUnit(0, 0, 0, 0);

        public override PointUnit Position { get => new PointUnit(0, 0); set { return; } }

        public override bool IsWorldEntity => false;

        public bool IsActive => active;

        public override bool IsInteractable => false;

        public override RenderLayer ClientRenderLayer => RenderLayer.UserInterface;

        private List<(string, Color)> chatHistory = new List<(string, Color)>();

        private bool active = false;
        private SpriteFont chatFont;
        private Camera camera;
        private ILoadedSprite chatBack;
        private InputHelper inputHelper;
        private GameWindow gameWindow;

        private string inputText = string.Empty;
        private Vector2 textSize;
        private Vector2 markerSize;

        private float maxChatMessages;

        private SceneManager sceneManager;
        private SpriteBatch spriteBatch;
        private ConnectionManager connectionManager;

        public ChatBox(AssetManager assetManager, Camera camera, GameWindow gameWindow, InputHelper inputHelper, 
            SceneManager sceneManager, SpriteBatch spriteBatch, ConnectionManager connectionManager)
        {
            chatFont = assetManager.GetFont();
            this.camera = camera;
            this.inputHelper = inputHelper;
            this.gameWindow = gameWindow;
            this.sceneManager = sceneManager;
            this.spriteBatch = spriteBatch;
            this.connectionManager = connectionManager;

            chatBack = assetManager.GetSprite("chat_back");
            markerSize = chatFont.MeasureString("> ");

            maxChatMessages = (int)Math.Floor((double)(200 / (markerSize.Y + 5)));
        }

        public void PushChatMessage(string chat, uint color)
        {
            chatHistory.Add((chat, new Color(color)));
        }

        public override void OnClientDraw()
        {
            if (active)
            {
                chatBack.Draw(0, (int)camera.ScreenView.Bottom - ((int)markerSize.Y + 10), widthOverride: 450, heightOverride: ((int)markerSize.Y + 10),
                    lightColor: ColorHelper.RGBA(0, 0, 0, 180));
                spriteBatch.DrawString(chatFont, "> ", new Vector2(5, camera.ScreenView.Bottom - (markerSize.Y + 5)), Color.BlueViolet);
                spriteBatch.DrawString(chatFont, inputText, new Vector2(5 + markerSize.X, camera.ScreenView.Bottom - (textSize.Y + 5)), Color.White);
            }

            for (int i = 0; i < chatHistory.Count; i++)
            {
                var message = chatHistory[(chatHistory.Count - 1) - i];
                var yPos = camera.ScreenView.Bottom - ((markerSize.Y + 5) * (i + 2));
                spriteBatch.DrawString(chatFont, message.Item1, new Vector2(5, yPos), message.Item2);
            }
        }

        public override void OnClientTick()
        {
            textSize = chatFont.MeasureString(inputText);

            if (!active && inputHelper.GetKeyPressed("chat"))
            {
                active = true;
                gameWindow.TextInput += readWrittenText;
            }
            if(!active && inputHelper.GetKeyPressed("command"))
            {
                active = true;
                inputText += "/";
                gameWindow.TextInput += readWrittenText;
            }

            if (active && inputHelper.GetKeyPressed(Keys.Escape)) // probably shouldn't be rebindable
            {
                active = false;
                inputText = string.Empty;
                gameWindow.TextInput -= readWrittenText;
            }

            if(inputHelper.GetKeyPressed(Keys.Enter)) // Not rebindable.
            {
                // publish to chat
                active = false;
                handleTextInput(inputText);
                inputText = string.Empty;
                gameWindow.TextInput -= readWrittenText;
            }
        }

        private void readWrittenText(object sender, TextInputEventArgs e)
        {
            if(e.Key == Keys.Back)
            {
                inputText = inputText.Substring(0, inputText.Length - 1);
                return;
            }
            if(e.Key == Keys.Escape || textSize.X > 500 - (markerSize.X + 10) || e.Key == Keys.Enter) // 500 = box width, 10 = 5x padding on both sides
            {
                return;
            }

            inputText += e.Character;
        }

        private void handleTextInput(string input)
        {
            connectionManager.SendChat(input);
        }
    }
}
