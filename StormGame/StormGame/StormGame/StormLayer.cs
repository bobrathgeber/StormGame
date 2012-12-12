using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace StormGame
{
    class StormLayer : DrawableObject
    {
        public StormLayer(string texture, Vector2 pos)
        {
            Texture = Globals.Content.Load<Texture2D>(texture);
            //Dictionary<string, Rectangle> spriteMap = Globals.Content.Load<Dictionary<string, Rectangle>>(texture);
            //animation = new Animation(Texture, spriteMap);
            //animationPlayer.PlayAnimation(animation, "Walk", 0.3f, true);
            SetDrawDepthRange(100, 105);
            Position = pos;
            _color = new Color(255, 255, 255, 70);
        }
    }
}
