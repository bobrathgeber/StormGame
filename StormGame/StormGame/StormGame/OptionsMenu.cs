using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace StormGame
{
    class OptionsMenu : MenuItem
    {
        private List<MenuItem> options;
        private MenuItemOption musicVolume;

        public OptionsMenu(string name)
            : base(name)
        {
            options = new List<MenuItem>();
            options.Add(new BackMenuItem("Back"));
            musicVolume = new MenuItemOption(OptionType.MasterVolume, new Microsoft.Xna.Framework.Vector2(200,200));
        }

        public override void Update()
        {
            musicVolume.Update();
            base.Update();
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            musicVolume.Draw(spritebatch);
            base.Draw(spritebatch);
        }

        public override void Activate(MainMenu mainMenu)
        {
            mainMenu.DisplayMenu(options);
        }
    } 
}
