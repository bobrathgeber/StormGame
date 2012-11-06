using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace StormGame
{
    class PopupWindow
    {
        private Button closeButton;
        private String message;
        private Texture2D backgroundTexture;
        private Rectangle windowSize;
        private Vector2 Origin;
        private bool ReadyToRemove;

        public PopupWindow(String text)
        {
            ReadyToRemove = false;
            message = text;
            Origin = new Vector2(200, 220);
            windowSize = new Rectangle((int)Origin.X, (int)Origin.Y, 600, 400);
            var butTex = Globals.Content.Load<Texture2D>("CloseButton");
            backgroundTexture = Globals.Content.Load<Texture2D>("currenthealth");
            var ButtonLocation = Origin + new Vector2(windowSize.X, windowSize.Y);
            closeButton = new Button(ButtonLocation, "", butTex, butTex, butTex);
            closeButton.isSelected = true;
        }

        public void Update()
        {
            closeButton.Update();
            if (closeButton.Clicked)
                ReadyToRemove = true;
        }

        public virtual void Draw()
        {
            Globals.SpriteBatch.Draw(backgroundTexture, windowSize, Color.Black);
            closeButton.Draw(Globals.SpriteBatch);
            Globals.SpriteBatch.DrawString(Globals.Font1, message, Origin, Color.White);
        }

        public bool isDead()
        {
            return ReadyToRemove;
        }
    }
}
