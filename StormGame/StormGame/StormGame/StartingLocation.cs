using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StormGame
{
    public class StartingLocation : DrawableObject
    {
        public StartingLocation(Vector2 pos)
        {
            Identifier = "startinglocation";
            Position = pos;
            Texture = Globals.Content.Load<Texture2D>("startingLocation");
            Invisible = true;
            isAlive = false;
            Collidable = false;
        }

        public override string GetSaveData()
        {
            return "startinglocation%" + Position.X + "%" + Position.Y;
        }
    }
}
