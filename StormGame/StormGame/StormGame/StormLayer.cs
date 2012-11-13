using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace StormGame
{
    class StormLayer : DrawableObject
    {
        public StormLayer(string texture, Vector2 pos)
        {
            Texture = Globals.Content.Load<Texture2D>(texture);
            Position = pos;
            _color = new Color(255, 255, 255, 70);
        }
    }
}
