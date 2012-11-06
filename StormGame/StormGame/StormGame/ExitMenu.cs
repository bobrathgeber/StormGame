using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StormGame
{
    class ExitMenu : MenuItem
    {
        public ExitMenu(string name)
            : base(name)
        {

        }

        public override void Activate(MainMenu mainMenu)
        {
            Globals.game.Exit();
        }
    }
}
