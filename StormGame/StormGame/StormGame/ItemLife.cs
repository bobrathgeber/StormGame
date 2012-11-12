using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace StormGame.DrawableObjects
{
    class ItemLife : Item
    {

        public ItemLife() : base()
        {
            Texture = Globals.Content.Load<Texture2D>("Powerup");
            Dictionary<string, Rectangle> spriteMap = Globals.Content.Load<Dictionary<string, Rectangle>>("PowerupSpriteMap");
            animation = new Animation(Texture, spriteMap);
            animationPlayer.PlayAnimation(animation, "Idle", 0.3f, true);
        }

        public override void Update()
        {           
            base.Update();
        }

        public override void Pickup()
        {
            onGround = false;
        }
    }
}
