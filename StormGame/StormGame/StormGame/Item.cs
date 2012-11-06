using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace StormGame
{
    public enum ItemType
    {
        Health,
        Powerup,
        Debris
    }

    public abstract class Item : DrawableObject
    {
        public bool CooldownReady;
        private float Cooldown;
        private float time;
        private float radius;
        public bool isSpecial;
        private bool isEjecting;
        private float pickUpDelay;
        private float pickUpDelayTimer;

        protected Storm storm;
        protected bool onGround;
        protected bool inInventory;
        private bool dropDelay;
        private float dropDelayTime;

        private List<SoundEffect> hitSounds;

        //Debris Variables
        float orbitRadius;
        private bool wasHit;
        public int Damage { get; set; }

        // Bounce control constants
        const float BounceHeight = 0.01f;
        const float BounceRate = 5.0f;
        const float BounceSync = -0.75f;
        private Vector2 _windforce = Vector2.Zero;
        private Vector2 _gravity = Vector2.Zero;
        const float AIRDRAG = 0.97f;
        const float MAX_VELOCITY = 50f;

        private float bounce;

        public Item()
        {
            LoadContent();
        }

        public override void LoadContent()
        {
            _debugLine = new Texture2D(Globals.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            hitSounds = new List<SoundEffect>();
            hitSounds.Add(Globals.Content.Load<SoundEffect>("Sounds/debrisHit1"));
            hitSounds.Add(Globals.Content.Load<SoundEffect>("Sounds/debrisHit2"));
            hitSounds.Add(Globals.Content.Load<SoundEffect>("Sounds/debrisHit3"));
            hitSounds.Add(Globals.Content.Load<SoundEffect>("Sounds/debrisHit4"));
        }

        public virtual void Update()
        {
            if (onGround)
            {
                // Bounce along a sine curve over time.
                // Include the X coordinate so that neighboring gems bounce in a nice wave pattern.            
                double t = Globals.GameTime.TotalGameTime.TotalSeconds * BounceRate + Position.X * BounceSync;
                bounce = (float)Math.Sin(t) * BounceHeight * Texture.Height;
                Position += new Vector2(0.0f, bounce);

                if (dropDelay)
                {
                    dropDelayTime += (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
                    if (dropDelayTime > 0.4f)
                    {
                        dropDelay = false;
                        dropDelayTime = 0f;
                    }
                }
            }

            if (isOrbiting)
            {
                time += (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
                Rotation = (Rotation + -0.02f) % (MathHelper.Pi * 2);
                UpdateDamage();

                if (!CooldownReady && time > Cooldown)
                {
                    CooldownReady = true;
                }
            }
            //else if (isEjecting)
            //{
            //    pickUpDelay += (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
            //    if (pickUpDelay >= pickUpDelayTimer)
            //    {
            //        isEjecting = false;
            //        pickUpDelay = 0.0f;
            //        Velocity *= AIRDRAG;
            //    }
            //}
            else
            {
                Velocity *= AIRDRAG;
                UpdateAI();
            }
        }

        public void DropOnGround(Vector2 position)
        {
            Position = position;
            onGround = true;
            inInventory = false;
            dropDelay = true;
            dropDelayTime = 0f;
        }

        Texture2D _debugLine;
        public override void Draw()
        {
            base.Draw();

            _debugLine.SetData(new[] { Color.White });
            DrawLine(Globals.SpriteBatch, _debugLine, 1, Color.Red, Position, Position + _windforce);
            DrawLine(Globals.SpriteBatch, _debugLine, 1, Color.Green, Position, Position + _gravity);
        }

        private void DrawLine(SpriteBatch batch, Texture2D blank, float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            batch.Draw(blank, point1, null, color, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, 0);
        }

        public virtual void DropOnGround() { }

        public virtual void Pickup() { }

        public virtual void Use() { }

        public virtual void UpdateAI() { }

        public void StartOrbiting()
        {
            Random rand = new Random();
            isOrbiting = true;
            orbitRadius = rand.Next(1500) + 100;
        }

        private void playRandomCollisionSound()
        {
            Random rand = new Random();
            int num = rand.Next(4);
            hitSounds[num].Play();
        }

        public void Move(Vector2 center, GameTime gameTime)
        {
            if (isOrbiting)
            {
                var a0 = Accel;
                var v0 = Velocity;
                var x0 = Position;

                var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                var distance = (center - Position);

                _gravity = distance;
                _gravity *= 0.4f;
                _windforce = CalcWindForce(distance, orbitRadius);

                Accel = _gravity;
                Accel += _windforce;

                Velocity += Accel * deltaTime;
                Velocity = ApplyDrag(Velocity);

                if (wasHit)
                {
                    Velocity *= 0.1f;
                    playRandomCollisionSound();
                }

                if (Velocity.Length() > MAX_VELOCITY)
                    Velocity = v0;

                wasHit = false;
            }

            Position += Velocity;

        }

        //public void Eject(Vector2 centerPoint)
        //{
        //    isEjecting = true;
        //    isOrbiting = false;
        //    this.Position = centerPoint;

        //    pickUpDelayTimer = 1.0f;

        //    Random rand = new Random();
        //    if (Velocity == new Vector2(0))
        //    {
        //        Velocity = new Vector2(rand.Next(5) - 3, rand.Next(5) - 3);
        //    }
        //}

        private void UpdateDamage()
        {
            Damage = (int)Math.Ceiling((Velocity.Length()) + (Weight / 4));
        }

        public Vector2 ApplyDrag(Vector2 velocity)
        {
            var drag = new Vector2(AIRDRAG);
            return velocity *= drag;
        }

        public Vector2 CalcWindForce(Vector2 distance, float windMagnitude)
        {
            var x = distance.X;
            var y = distance.Y;
            var a1 = (float)Math.Atan2((float)distance.X, (float)distance.Y);
            var windAngle = a1 + MathHelper.ToRadians(90);

            var windForce = new Vector2((float)Math.Sin(windAngle) * windMagnitude, (float)Math.Cos(windAngle) * windMagnitude);
            return windForce / distance.Length();
        }

        public void Collide()
        {
            if (CooldownReady)
            {
                Cooldown = 0.5f;
                CooldownReady = false;
                time = 0;
                wasHit = true;
            }
        }
    }
}
