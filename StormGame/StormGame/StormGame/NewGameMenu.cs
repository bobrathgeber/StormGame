using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StormGame
{
    class NewGameMenu : MenuItem
    {
        private List<MenuItem> levels;

        public NewGameMenu(string name)
            : base(name)
        {
            levels = new List<MenuItem>();
            CreateLevelButtons();
        }

        public override void Activate(MainMenu mainMenu)
        {
            mainMenu.DisplayMenu(levels);
        }

        private void CreateLevelButtons()
        {
            // Make a reference to a directory.
            DirectoryInfo di = new DirectoryInfo("Levels/");

            // Get a reference to each file in that directory.
            FileInfo[] fiArr = di.GetFiles();

            // Display the names of the files.
            for (int i = 0; i < fiArr.Length; i++)
            {
                levels.Add(new LevelMenuItem(fiArr[i].Name));
            }
            levels.Add(new BackMenuItem("Back"));
        }
    }
}
