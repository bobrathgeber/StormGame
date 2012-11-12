using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using StormGame.DrawableObjects;

namespace StormGame
{
    public abstract class Destructible : DrawableObject
    {      
        public int CurrentHealth { get; set; }
        protected DrawableObject droppedObject;
        
        private List<DamagePopup> DamagePopups;
    

        public void LoadContent(Vector2 pos, int health)
        {
            Collidable = true;
            isAlive = true;

            DamagePopups = new List<DamagePopup>();
            droppedObject = GenerateDebris();

            Health = CurrentHealth = health;
            Position = pos;
            //Texture = animations[0].Texture;
        }

        private DrawableObject GenerateDebris()
        {
            var obj = new LargeDebris();
            return obj;
        }

        public void DamageHealth(int Damage)
        {
            CurrentHealth -= Damage;
            DamagePopup dp = new DamagePopup(Damage, Position);
            DamagePopups.Add(dp);
        }

        public override void Update()
        {
            //SrcRectangle = animationPlayer.GetSourceRectangle(Globals.GameTime);
        }

        public override void Draw()
        {
            base.Draw();

            foreach (DamagePopup dp in DamagePopups)
            {
                dp.Draw();
            }
        }

        public float PercentHealth()
        {
            return (float)CurrentHealth / (float)Health;
        }

        public void UpdateTexture()
        {
            if (PercentHealth() > 0.4f)
            {
                animationPlayer.PlayAnimation(animation, "Good", 0.25f, true);
            }
            else if (PercentHealth() > 0)
            {
                animationPlayer.PlayAnimation(animation, "Poor", 0.25f, true);
            }
            else
            {
                animationPlayer.PlayAnimation(animation, "Dead", 0.25f, true);
            }
        }

        public void UpdateDamagePopups(GameTime gameTime)
        {
            for (int i = DamagePopups.Count; i > 0; i--)
            {
                DamagePopups[i - 1].Update(gameTime);
                if (!DamagePopups[i - 1].isAlive)
                    DamagePopups.Remove(DamagePopups[i - 1]);
            }
        }

        public abstract DrawableObject onDeath(Storm storm);

    }
}
