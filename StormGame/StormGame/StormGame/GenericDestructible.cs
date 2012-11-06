using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using StormGame.DrawableObjects;

namespace StormGame
{
    class GenericDestructible : Destructible
    {
        public GenericDestructible(Vector2 pos, string textureFileNameGood, string textureFileNamePoor, string textureFileNameDead, float frameRate, int numOfFrames, int health)
            : base()
        {
            Identifier = "genericdestructible";
            animations = new List<Animation>();
            Health = health;

            animations.Add(new Animation(Globals.Content.Load<Texture2D>(textureFileNameGood), numOfFrames, frameRate, true));
            animations.Add(new Animation(Globals.Content.Load<Texture2D>(textureFileNamePoor), numOfFrames, frameRate, true));
            animations.Add(new Animation(Globals.Content.Load<Texture2D>(textureFileNameDead), numOfFrames, frameRate, true));
            animationPlayer.PlayAnimation(animations[0]);
            this.LoadContent(pos, health);
        }

        public override DrawableObject onDeath(Storm storm)
        {
            isAlive = false;
            //droppedObject.Eject(Position);
            
            ItemLife item = new ItemLife();
            item.DropOnGround(Position);

            return item;
        }
    }
}
