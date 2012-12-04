using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
namespace StormGame
{
    class ClickAnimation : DrawableObject
    {
        private Storm storm;

        public ClickAnimation(Storm storm)
        {
            this.storm = storm;
            Identifier = "ClickAnimation";
            Texture = Globals.Content.Load<Texture2D>(Identifier);
            Dictionary<string, Rectangle> spriteMap = Globals.Content.Load<Dictionary<string, Rectangle>>(Identifier + "SpriteMap");
            animation = new Animation(Texture, spriteMap);
            animationPlayer.PlayAnimation(animation, "ClickAnimation", 0.07f, true);
        }
        public override void Update()
        {
            //Vector2 targetClick = new Vector2(Globals.mouseState.X, Globals.mouseState.Y);
            //Vector2 directionVector = (targetClick - storm.Position);
            //directionVector.Normalize();
            //Accel += directionVector * storm.speed * Globals.GameTime.ElapsedGameTime.Seconds;
            //Position = targetClick;
        }

    }
}
