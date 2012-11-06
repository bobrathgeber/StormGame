using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace StormGame
{
    class Button
    {
        private Rectangle rec;
        private Texture2D upTexture;
        private Texture2D downTexture;
        private Texture2D hoverTexture;

        private string label;

        public bool isSelected;

        private bool isHover;
        private bool isDown;
        private bool isClicked;

        public Button(Vector2 position, string buttonLabel, Texture2D upTex, Texture2D downTex, Texture2D hoverTex)
        {
            this.upTexture = upTex;
            this.downTexture = downTex;
            this.hoverTexture = hoverTex;
            rec = new Rectangle((int)position.X, (int)position.Y, upTexture.Width, upTexture.Height);
            label = buttonLabel;
            isHover = false;
            isDown = false;
            isClicked = false;

            isSelected = false;
        }

        public Point Position //can only change the x and y values
        {
            get { return new Point(rec.X, rec.Y); }
            set { rec.X = value.X; rec.Y = value.Y; }
        }

        public Rectangle Bounds()
        {
            return rec;
        }

        public bool Clicked
        {
            get { return isClicked; }
        }

        //*****************************
        //Needs to be called in the game loop
        //*****************************

        public void Update()
        {
            if (rec.Contains(Globals.mouseState.X, Globals.mouseState.Y))
                isHover = true;
            else
                isHover = false;

            if (Globals.mouseState.LeftButton == ButtonState.Pressed)
                isDown = true;
            else
                isDown = false;

            if (isHover && Globals.oldMouseState.LeftButton == ButtonState.Pressed && Globals.mouseState.LeftButton == ButtonState.Released 
                || (Globals.oldKeyboardState.IsKeyUp(Keys.Space) && Globals.keyboardState.IsKeyDown(Keys.Space) && isSelected)) //A Mouse Click
                isClicked = true;
            else
                isClicked = false;
                
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isDown)
            {
                spriteBatch.Draw(downTexture, rec, Color.Gray);
            }
            else if (isSelected)
            {
                spriteBatch.Draw(upTexture, rec, Color.Gray);
            }
            else if (isHover)
            {
                spriteBatch.Draw(hoverTexture, rec, Color.LightGray);
            }
            else
            {
                spriteBatch.Draw(upTexture, rec, Color.White);
            }

            spriteBatch.DrawString(Globals.Font1, label, new Vector2(rec.X + 5, rec.Y + (rec.Height / 2)), Color.White);
        }
    }
}


