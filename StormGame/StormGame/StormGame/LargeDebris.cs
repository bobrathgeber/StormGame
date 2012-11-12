using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using StormGame.DrawableObjects;

namespace StormGame
{
    class LargeDebris : Item
    {
        public LargeDebris() : base()
        {
            this.Initialize();
        }

        public LargeDebris(Vector2 pos)
            : base()
        {
            this.Initialize();
            Position = pos;
        }

        public override void Initialize()
        {
            Identifier = "largedebris";
            Weight = 10;

            Texture = Globals.Content.Load<Texture2D>("WoodDebris");
            Dictionary<string, Rectangle> spriteMap = Globals.Content.Load<Dictionary<string, Rectangle>>("WoodDebrisSpriteMap");
            animation = new Animation(Texture, spriteMap);
            animationPlayer.PlayAnimation(animation, "Idle", 1.0f, true);

            base.Initialize();
        }
    }
}
