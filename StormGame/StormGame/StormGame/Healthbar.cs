using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace StormGame
{
    class Healthbar : GameplayUIElement
    {
        private Texture2D _maxBar;
        private Texture2D _currentBar;
        private Rectangle Bounds;
        private Rectangle CurrentBar;

        public int MaxHealth
        {
            get { return _maxHealth; }
            set { _maxHealth = value; }
        }
        private int _maxHealth;

        public int CurrentHealth
        {
            get { return _currentHealth; }
            set { _currentHealth = value; }
        }
        private int _currentHealth;

        public void ModifyHealth(int m)
        {
            if( CurrentHealth + m > MaxHealth)
                CurrentHealth = MaxHealth;
            else
                CurrentHealth += m;

            UpdateHealthBars();
        }

        private void UpdateHealthBars()
        {
            decimal temp = (decimal)CurrentHealth / (decimal)MaxHealth;
            CurrentBar.Width = (int)((decimal)Bounds.Width * temp);
        }

        public Healthbar(int HP, Vector2 p)
        {
            _maxHealth = HP;
            _currentHealth = HP;
            Bounds = new Rectangle();
            CurrentBar = new Rectangle();
            Position = p;
            Bounds.X = (int)p.X;
            Bounds.Y = (int)p.Y;

            _maxBar = Globals.Content.Load<Texture2D>("currenthealth");
            _currentBar = Globals.Content.Load<Texture2D>("currenthealth");
            Bounds.Height = _maxBar.Height;
            Bounds.Width = _maxBar.Width *10;
            CurrentBar = Bounds;

        }

        public void Draw()
        {
            Globals.SpriteBatch.Draw(_maxBar, Bounds, Color.Red);
            Globals.SpriteBatch.Draw(_currentBar, CurrentBar, Color.Green);

            string sizeString = CurrentHealth.ToString() + " of " + MaxHealth.ToString();

            DrawShadowedString(Globals.Font1, sizeString, GetTextPosition(), Color.BlanchedAlmond);
        }

        private void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color)
        {
            Globals.SpriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            Globals.SpriteBatch.DrawString(font, value, position, color);
        }

        private Vector2 GetTextPosition()
        {
            Vector2 pos = new Vector2((Bounds.Width - Bounds.X) / 2, (Bounds.Height+Bounds.Y) / 2);
            
            return pos;
        }
        public void ResetHealth()
        {
            ModifyHealth(MaxHealth);
        }
    }
}
