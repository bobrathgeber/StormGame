using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace StormGame
{
    class StormFront : DrawableObject
    {
        Vector2 _startingPosition;

        public StormFront(Vector2 pos)
        {
            Identifier = "Storm Front";
            Texture = Globals.Content.Load<Texture2D>("StormFront");
            Dictionary<string, Rectangle> spriteMap = Globals.Content.Load<Dictionary<string, Rectangle>>("StormFrontSpriteMap");
            animation = new Animation(Texture, spriteMap);
            animationPlayer.PlayAnimation(animation, "Idle", 0.1f, true);
            _color = new Color(255, 255, 255, 50);
            Velocity = new Vector2(1f, 0);
            _startingPosition = Position = pos;
        }

        public override string GetSaveData()
        {
            return "Stormfront%" + _startingPosition.X + "%" + _startingPosition.Y;
        }

        public override void Update()
        {
            Position = Velocity + Position;
        }

        static private Texture2D CreateRectangle(int width, int height, Color colori)
        {
            Texture2D rectangleTexture = new Texture2D(Globals.GraphicsDevice, width, height, false,
            SurfaceFormat.Color);// create the rectangle texture, ,but it will have no color! lets fix that
            Color[] color = new Color[width * height];//set the color to the amount of pixels in the textures
            for (int i = 0; i < color.Length; i++)//loop through all the colors setting them to whatever values we want
            {
                color[i] = colori;
            }
            rectangleTexture.SetData(color);//set the color data on the texture
            return rectangleTexture;//return the texture
        }
    }
}
