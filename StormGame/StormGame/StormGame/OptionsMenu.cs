using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StormGame
{
    class OptionsMenu : MenuItem
    {
        private List<MenuItem> options;

        public OptionsMenu(string name)
            : base(name)
        {
            options = new List<MenuItem>();
            options.Add(new BackMenuItem("Back"));
        }

        public override void Activate(MainMenu mainMenu)
        {
            mainMenu.DisplayMenu(options);
        }
    }
}
