namespace StormGame
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System;

    public class Sprite : DrawableObject
    {
        public int depth;
        private bool isTiled;
        private Vector2 tiledDimensions;
        private int _tiledWidth;
        private int _tiledHeight;

        public Sprite(string texture, Vector2 pos, int depth, bool isTiled=false, int tiledWidth = 0, int tiledHeight=0)
        {
            Identifier = texture;
            this.Texture = Globals.Content.Load<Texture2D>(texture);
            this.depth = depth;
            this.Position = pos + new Vector2(Texture.Width/2, Texture.Height/2);
            this.isTiled = isTiled;
            _tiledHeight = tiledHeight;
            _tiledWidth = tiledWidth;
            //SrcRectangle = new Rectangle(0, 0, (int)Texture.Width, (int)Texture.Height);
            SetTilingDimensions(isTiled, tiledWidth, tiledHeight);
        }

        public override string GetSaveData()
        {
            return "Sprite%" + Identifier + "%" + Position.X + "%" + Position.Y + "%" + depth + "%" + isTiled + "%" + _tiledWidth + "%" + _tiledHeight;
        }

        private void SetTilingDimensions(bool isTiled, int tiledWidth, int tiledHeight)
        {
            if (isTiled)
            {
                int texWidth = Texture.Width;
                int texHeight = Texture.Height;

                tiledDimensions.X = (int)Math.Ceiling((float)tiledWidth / (float)texWidth);
                tiledDimensions.Y = (int)Math.Ceiling((float)tiledHeight / (float)texHeight);
            }
        }

        public override void Draw()
        {
            if (isTiled)
            {
                DrawTiles();
            }
            else
                base.Draw();
        }

        private void DrawTiles()
        {
            var originalPosition = Position;
            for (int i = 0; i < tiledDimensions.Y; i++)
            {
                for (int j = 0; j < tiledDimensions.X; j++)
                {                    
                    base.Draw();
                    Position += new Vector2(Texture.Width, 0);
                }
                Position = new Vector2(originalPosition.X, Position.Y+Texture.Height);
            }
            Position = originalPosition;
        }
        
    }
}
