﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StormGame
{
    //public enum LevelStatus
    //{
    //    Playing,
    //    Died,
    //    Complete
    //}

    public abstract class DrawableObject
    {
        public string Identifier;

        //Physics Variables
        public Vector2 Velocity = new Vector2(0);
        public Vector2 Accel = new Vector2(0);
        public Boolean Collidable = true;
        public Vector2 Position;
        public float Rotation = 0;

        //Status Variables
        public bool isAlive { get; set; }
        public int Health;
        public float Weight;
        public bool isOrbiting = false;
        public bool Selected = false;

        //Visual Variables
        public Texture2D Texture { get; set; }        
        protected AnimationPlayer animationPlayer;
        protected Animation animation;
        public Rectangle SrcRectangle { get { if (animation != null) return animation.GetFrameRectangle(animationPlayer.currentFrame); else return Texture.Bounds; } }
        public float frameRate = 1.0f;
        public int numOfFrames = 1;
        public bool Invisible = false;
        public Color _color = Color.White;
        public Vector2 scale = new Vector2(1);

        public Rectangle BoundingBox { get { return new Rectangle((int)TopLeftPoint.X, (int)TopLeftPoint.Y, (int)Width, (int)Height); } }
        public Vector2 Origin { get { return new Vector2(Width / 2, Height / 2); } }
        public float Width { get { return SrcRectangle.Width * scale.X; } }
        public float Height { get { return SrcRectangle.Height * scale.Y; } }
        public Vector2 TopLeftPoint { get { return new Vector2(Position.X - Origin.X, Position.Y - Origin.Y); } }
        public Vector2 TopRightPoint { get { return new Vector2(Position.X + Origin.X, Position.Y - Origin.Y); } }
        public Vector2 BottomLeftPoint { get { return new Vector2(Position.X - Origin.X, Position.Y + Origin.Y); } }
        public Vector2 BottomRightPoint { get { return new Vector2(Position.X + Origin.X, Position.Y + Origin.Y); } }     
                

        public virtual void Initialize()
        {
        }

        public virtual void LoadContent() 
        {
        }

        public bool CheckCollision(Vector2 v)
        {
            bool isColliding = BoundingBox.Contains(new Point((int)v.X, (int)v.Y));
            if (isAlive)
                return isColliding;
            else
                return false;
        }

        public bool CheckCollision(Rectangle r1)
        {
            bool isColliding = r1.Intersects(BoundingBox);
            if (isAlive)
                return isColliding;
            else
                return false;
        }

        public virtual void Draw()
        {
            if (animation != null)
                animationPlayer.Draw(Globals.GameTime, Globals.SpriteBatch, Position, SpriteEffects.None, Rotation, scale, Origin);
            else if (Texture != null)
                Globals.SpriteBatch.Draw(Texture, Position, SrcRectangle, _color, Rotation, Origin, scale, SpriteEffects.None, 0);

            if (this.Selected)
                DrawBoundingBox();

            //if (Texture != null)
            //{
                
            //        Globals.SpriteBatch.Draw(Texture, Position, SrcRectangle, new Color(105, 200, 105, 150), Rotation, Origin, scale, SpriteEffects.None, 0);
                    
            //    }
            //    else if (!Invisible || Globals.editorMode)
            //        Globals.SpriteBatch.Draw(Texture, Position, SrcRectangle, _color, Rotation, Origin, scale, SpriteEffects.None, 0);
            //}
            
        }

        public virtual void Update() { }
        
        Texture2D blank = new Texture2D(Globals.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);        
        private void DrawBoundingBox()
        {
            blank.SetData(new[] { Color.White });
            float length1 = Vector2.Distance(TopLeftPoint, TopRightPoint);
            float length2 = Vector2.Distance(TopRightPoint, BottomRightPoint);
            float length3 = Vector2.Distance(BottomRightPoint, BottomLeftPoint);
            float length4 = Vector2.Distance(BottomLeftPoint, TopLeftPoint);

            Globals.SpriteBatch.Draw(blank, TopLeftPoint, null, Color.White, MathHelper.ToRadians(270), Vector2.Zero, new Vector2(3, SrcRectangle.Width), SpriteEffects.None, 0);
            Globals.SpriteBatch.Draw(blank, TopRightPoint, null, Color.White, MathHelper.ToRadians(90), Vector2.Zero, new Vector2(SrcRectangle.Height, 3), SpriteEffects.None, 0);
            Globals.SpriteBatch.Draw(blank, BottomRightPoint, null, Color.White, MathHelper.ToRadians(90), Vector2.Zero, new Vector2(3, SrcRectangle.Width), SpriteEffects.None, 0);
            Globals.SpriteBatch.Draw(blank, BottomLeftPoint, null, Color.White, MathHelper.ToRadians(270), Vector2.Zero, new Vector2(SrcRectangle.Height, 3), SpriteEffects.None, 0);
        }

        
    }
}