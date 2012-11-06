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
        
        private Animation animation1;
        private Behavior behavior { get; set; }
        Vector2 targetDestination;
        bool needsNewBehavior = true;
        float runSpeed = 0.5f;
        //float walkSpeed = 0.2f;

        public Cow(Vector2 position)
            : base()
        {
            Identifier = "cow";
            Position = position;
            Texture = Globals.Content.Load<Texture2D>("CowAnimationTest");
            animation1 = new Animation(Texture, 2, 0.4f, true);
            animationPlayer.PlayAnimation(animation1);
            Weight = 100;
            
            this.Initialize();
            LoadContent();
        }

        public override void LoadContent()
        {
            
            base.LoadContent();
        }

        public override void Update()
        {
            SrcRectangle = animationPlayer.GetSourceRectangle(Globals.GameTime);
            base.Update();
        }

        public override void UpdateAI()
        {
             if (needsNewBehavior)
            {
                Vector2 distance = Vector2.Zero;//Position - storm.Position;

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
    }
}
