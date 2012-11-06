using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace StormGame
{
    class LevelSelector
    {
        private Texture2D backgroundImage;
        private List<LevelSelectorButton> levelButtons;
        private Texture2D buttonTex;
        //private Dictionary<string, string> levelFileNames;
        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;

        public LevelSelector()
        {
            levelButtons = new List<LevelSelectorButton>();
            
        }

        public void LoadContent()
        {
            
            CreateLevelButtons();
            levelButtons[0].isSelected = true;
            
        }

        public void Update(Game1 game)
        {
            foreach (LevelSelectorButton b in levelButtons)
                b.Update();

            CheckKeyboardInput(game);
            
        }

        public void Draw()
        {
            Globals.SpriteBatch.Begin();

            Globals.SpriteBatch.Draw(backgroundImage, new Vector2(0), Color.White);
            foreach(Button b in levelButtons)
            {
                b.Draw(Globals.SpriteBatch);
            }
            Globals.SpriteBatch.End();
        }

        private Button getSelectedButton()
        {
            foreach (Button b in levelButtons)
            {
                if (b.isSelected)
                    return b;
            }
            return null;
            
        }

        private void CheckKeyboardInput(Game1 game)
        {
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.W) && !oldKeyboardState.IsKeyDown(Keys.W))
            {
                Button selectedButton = getSelectedButton();
                if (levelButtons[0] == selectedButton)
                {
                    levelButtons[levelButtons.Count - 1].isSelected = true;
                }
                else
                {
                    for (int i = 0; i < levelButtons.Count; i++)
                    {
                        if (levelButtons[i].isSelected)
                            levelButtons[i - 1].isSelected = true;
                    }
                }
                selectedButton.isSelected = false;
            }
            else if (keyboardState.IsKeyDown(Keys.S) && !oldKeyboardState.IsKeyDown(Keys.S))
            {
                Button selectedButton = getSelectedButton();
                if (levelButtons[levelButtons.Count - 1] == selectedButton)
                {
                    levelButtons[0].isSelected = true;
                }
                else
                {
                    for (int i = levelButtons.Count - 1; i >= 0; i--)
                    {
                        if (levelButtons[i].isSelected)
                            levelButtons[i + 1].isSelected = true;
                    }
                }
                selectedButton.isSelected = false;
            }
            else if (keyboardState.IsKeyDown(Keys.Space) && !oldKeyboardState.IsKeyDown(Keys.Space))
            {
                foreach (LevelSelectorButton button in levelButtons)
                {
                    if (button.isSelected)
                    {
                        //var line = button.data.Split('%');
                        Level level = new Level("Levels/" + button.data);
                        game.LoadNewLevel(level);
                    }
                }    
            }

            foreach (LevelSelectorButton b in levelButtons)
            {
                if( b.Clicked)
                {
                    Level level = new Level("Levels/" + b.data);
                    game.LoadNewLevel(level);
                }
            }

            oldKeyboardState = keyboardState;
        }

        private void CreateLevelButtons()
        {
            backgroundImage = Globals.Content.Load<Texture2D>("levelselectorbg");
            buttonTex = Globals.Content.Load<Texture2D>("LevelButton");

            // Make a reference to a directory.
            DirectoryInfo di = new DirectoryInfo("Levels/");

            // Get a reference to each file in that directory.
            FileInfo[] fiArr = di.GetFiles();

            // Display the names of the files.
            for(int i = 0; i< fiArr.Length; i++)
            {
                levelButtons.Add(new LevelSelectorButton(new Vector2(150, (i * buttonTex.Height) + 100), fiArr[i].ToString(), buttonTex, buttonTex, buttonTex, fiArr[i].Name));
            
            }
        }
    }
}
