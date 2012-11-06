using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StormGame
{
    class LevelMenuItem : MenuItem
    {
        public LevelMenuItem(string name)
            : base(name)
        {

        }

        public override void Activate(MainMenu mainMenu)
        {
            Level level = new Level("../../../Levels/" + name);
            Globals.game.LoadNewLevel(level);
        }
    }
}
