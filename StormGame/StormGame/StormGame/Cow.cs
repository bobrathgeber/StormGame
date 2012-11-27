using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using StormGame.DrawableObjects;

namespace StormGame
{
    class Cow : Item
    {
        public enum Behavior
        {
            runningFromStorm,
            walking,
            idle
        }
        
        private Behavior behavior { get; set; }
        Vector2 targetDestination;
        bool needsNewBehavior = true;
        float runSpeed = 0.5f;
        //float walkSpeed = 0.2f;

        public Cow(Vector2 position, bool onground = true)
            : base(ItemType.Debris)
        {
            Identifier = "cow";
            Position = position;  
            Weight = 100;
            onGround = onground;

            Texture = Globals.Content.Load<Texture2D>("Cow");
            Dictionary<string, Rectangle> spriteMap = Globals.Content.Load<Dictionary<string, Rectangle>>("CowSpriteMap");
            animation = new Animation(Texture, spriteMap);
            animationPlayer.PlayAnimation(animation, "Walk", 0.3f, true);
            
            this.Initialize();
            LoadContent();
        }

        public override void LoadContent()
        {
            
            base.LoadContent();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void UpdateAI()
        {
             if (needsNewBehavior)
            {
                Vector2 distance = new Vector2(-1, -1);//Position - storm.Position;

                if (distance.Length() < 300)
                {
                    distance.Normalize();
                    distance *= 700;
                    behavior = Behavior.runningFromStorm;
                    SetNewTargetDestination(distance);
                    var direction = (targetDestination - Position);
                    Velocity = direction * runSpeed * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
                    direction.Normalize();
                    Rotate(direction);
                    needsNewBehavior = false;
                }
                else
                {
                    behavior = Behavior.idle;
                }

                
            }
            else
                {
                switch (behavior)
                {
                    case Behavior.runningFromStorm:
                        float distance = (Position - targetDestination).Length();
                        if (distance < 5 || Velocity.Length() < 0.5)
                        {
                            Velocity = new Vector2(0);
                            needsNewBehavior = true;
                        }
                        break;

                    case Behavior.walking:

                        break;

                    case Behavior.idle:

                        break;

                }
           }            
        }

        private void SetNewTargetDestination(Vector2 distance)
        {
            var location = Position;
            Random rand = new Random();
            location += distance;
            
            location.X += rand.Next(200)-100;
            location.Y += rand.Next(200) - 100;

            targetDestination = location;
        }


        private void Rotate(Vector2 direction)
        {
            Rotation = (float)(Math.Atan2(direction.Y, direction.X)) + 1.571f;
        }

        public override void onPickup()
        {
            base.onPickup();
        }
    }
}
