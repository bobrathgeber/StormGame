using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StormGame
{
    class BlockerObject :DrawableObject
    {
        public BlockerObject(string Tex, Vector2 Pos, Vector2 Scale, bool Invisible = false)
        {
            Texture = Globals.Content.Load<Texture2D>(Tex);
            InitializeVariables(Pos, Scale);
            this.Invisible = Invisible;
            //SrcRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            
        }

        public override string GetSaveData()
        {
            return "blockerobject%" + Position.X + "%" + Position.Y + "%" + scale.X + "%" + scale.Y +"%" + Invisible;
        }

        private void InitializeVariables(Vector2 pos, Vector2 scale)
        {
            Position = pos;
            this.scale = scale;
            isAlive = true;
        }

        public BlockerObject(Texture2D Tex, Vector2 Pos, Vector2 Scale)
        {
            Texture = Tex;
            InitializeVariables(Pos, Scale);
        }
    }
}
