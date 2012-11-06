using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace StormGame.DrawableObjects
{
    class ItemLife : Item
    {

        public ItemLife() : base()
        {
            Texture = Globals.Content.Load<Texture2D>("Powerup_Default");
            animationPlayer.PlayAnimation(new Animation(Texture, 1, 0.4f, true));
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
