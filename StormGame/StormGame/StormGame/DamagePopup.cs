using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace StormGame
{
    class DamagePopup : DrawableObject
    {
        private string DamageString;
        private const float FadeTime = 2.0f;
        private float time;

        public DamagePopup(int damage, Vector2 pos)
        {
            DamageString = damage.ToString();
            Position = pos;
            isAlive = true;
            time = 0.0f;
        }

        public void Update(GameTime gameTime)
        {
            Position -= new Vector2(0f, 1.0f);

            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (time > FadeTime)
            {
                isAlive = false;
            }
        }

        public override void Draw()
        {
            Globals.SpriteBatch.DrawString(Globals.Font1, DamageString, Position, Color.Red);
        }

    }
}
