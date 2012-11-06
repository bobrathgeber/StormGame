using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

/*Projected Summary: 
 * Debris is attached to the storm.
 * It gains momentum slowly.
 * It loses momentum when it hits something.
 * If the the speed is below a certain point... it vaporizes or gets no clipping or something.
 * Damage is determined by speed and weight.
 */


namespace StormGame
{
    

    public abstract class Debris : DrawableObject
    {        
        public bool CooldownReady;               
        private float Cooldown;
        private float time;
        private float radius;        
        public bool isSpecial;
        private bool isEjecting;
        private float pickUpDelay;
        private float pickUpDelayTimer;

        private const float AIRDRAG = 0.97f;
        const float MAX_VELOCITY = 50f;
        private bool wasHit;

        private Vector2 _windForce;
        private Vector2 _gravity;
        Texture2D _debugLine;

        //Sounds
        private List<SoundEffect> hitSounds;

        public int Damage { get; set; }
        protected Storm storm;


        public Debris(Storm storm)
        {
            this.storm = storm;
            Initialize();
             _debugLine = new Texture2D(Globals.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
        }
            
        public override void Initialize()
        {            
            Collidable = true;
            isOrbiting = false;
            CooldownReady = true;
            time = 0.0f;
            wasHit = false;

            //hitSounds = new List<SoundEffect>();
            //hitSounds.Add(Globals.Content.Load<SoundEffect>("Sounds/debrisHit1"));
            //hitSounds.Add(Globals.Content.Load<SoundEffect>("Sounds/debrisHit2"));
            //hitSounds.Add(Globals.Content.Load<SoundEffect>("Sounds/debrisHit3"));
            //hitSounds.Add(Globals.Content.Load<SoundEffect>("Sounds/debrisHit4"));

            UpdateDamage();
            base.Initialize();
        }

        public virtual void Update(Storm storm)
        {
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
            else if(isEjecting)
            {
                pickUpDelay += (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
                if (pickUpDelay >= pickUpDelayTimer)
                {
                    isEjecting = false;
                    pickUpDelay = 0.0f;
                    Velocity *= AIRDRAG;
                }                
            }
            else
            {
                Velocity *= AIRDRAG;
                UpdateAI(storm);                           
            }
            
        }

        public virtual void UpdateAI(Storm storm){}

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

        public override void Draw()
        {
            base.Draw();
            
            _debugLine.SetData(new[] { Color.White });
            DrawLine(Globals.SpriteBatch, _debugLine, 1, Color.Red, Position, Position + _windForce);
            DrawLine(Globals.SpriteBatch, _debugLine, 1, Color.Green, Position, Position + _gravity);
        }

        void DrawLine(SpriteBatch batch, Texture2D blank, float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            batch.Draw(blank, point1, null, color, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, 0);
        }

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
    }
}
