using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using StormGame.DrawableObjects;

namespace StormGame
{
    class GenericDestructible : Destructible
    {
        public GenericDestructible(Vector2 pos, string spriteSheet, float frameRate, int health)
            : base()
        {
            this.ObjectType = DrawableObjectType.Destructible;
            Identifier = spriteSheet;
            Texture = Globals.Content.Load<Texture2D>(spriteSheet);
            Dictionary<string, Rectangle> spriteMap = Globals.Content.Load<Dictionary<string, Rectangle>>(spriteSheet + "SpriteMap");
            animation = new Animation(Texture, spriteMap);
            animationPlayer.PlayAnimation(animation, "Good", frameRate, true);

            Health = health;

            this.LoadContent(pos, health);
        }

        public override DrawableObject onDeath(Storm storm)
        {
            isAlive = false;
            Collidable = false;

            Item item = GetRandomItem();
            if (item != null)
            {
                item.DropOnGround(Position);
                //if (item.Type == ItemType.Debris)
                //    item.Eject(item.Position);
            }

            return item;
        }
    }
}
