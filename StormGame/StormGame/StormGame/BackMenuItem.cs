using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StormGame
{
    class BackMenuItem : MenuItem
    {
        //List<MenuItem> previousMenu;

        public BackMenuItem(string name)
            : base(name)
        {
            //SetPosition(pos, Globals.Content.Load<SpriteFont>("MenuFont"));
        }

        public override void Activate(MainMenu mainMenu)
        {
            mainMenu.BackToMainMenu();
        }
    }
}
