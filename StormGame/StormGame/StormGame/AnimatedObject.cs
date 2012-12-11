using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace StormGame
{
    class AnimatedObject : DrawableObject
    {
        public AnimatedObject(string Tex, Vector2 Pos, Vector2 Scale, float rotation, float framerate)
        {
            Identifier = Tex;
            frameRate = frameRate;
            Position = Pos;
            Rotation = rotation;
            Texture = Globals.Content.Load<Texture2D>(Identifier);
            Dictionary<string, Rectangle> spriteMap = Globals.Content.Load<Dictionary<string, Rectangle>>(Identifier + "SpriteMap");
            animation = new Animation(Texture, spriteMap);
            animationPlayer.PlayAnimation(animation, "Idle", framerate, true);
        }

        public override string GetSaveData()
        {
            return "AnimatedObject%" + Identifier + "%" + Position.X + "%" + Position.Y + "%" + scale.X + "%" + Rotation + "%" + frameRate;
        }
    }
}
