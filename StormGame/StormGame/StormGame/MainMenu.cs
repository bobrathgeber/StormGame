using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace StormGame
{
    //Background Image
    //List of Menu Items
    //Manage Menu Items

    class MainMenu
    {
        private Texture2D backgroundImage;
        private List<MenuItem> mainMenuItems;
        private List<MenuItem> currentMenuItems;
        private MenuItem selectedMenuItem;
        private const int MENU_ITEM_X = 150;
        private const int MENU_ITEM_Y = 400;
        private const int MENU_ITEM_SPACING = 50;
        private SpriteFont font;
        private SoundEffect click;

        public MainMenu()
        {

        }

        public void LoadContent(ContentManager content)
        {
            //backgroundImage = content.Load<Texture2D>("MenuBG");
            mainMenuItems = new List<MenuItem>();
            font = content.Load<SpriteFont>("MenuFont");
            mainMenuItems.Add(new NewGameMenu("New Game"));
            //mainMenuItems.Add(new OptionsMenu("Load"));
            mainMenuItems.Add(new OptionsMenu("Options"));
            mainMenuItems.Add(new ExitMenu("Exit"));

            click = Globals.Content.Load<SoundEffect>("Sounds/click");

            DisplayMenu(mainMenuItems);
            
        }

        public void Update()
        {
            UpdateMenuItems();
        }

        private void UpdateMenuItems()
        {
            foreach (MenuItem mi in currentMenuItems)
            {
                if (mi.CheckHover())
                {
                    mi.Select();
                    selectedMenuItem = mi;
                    foreach (MenuItem mi2 in currentMenuItems)
                        if (mi2 != mi)
                            mi2.Deselect();

                    if (Globals.mouseState.LeftButton == ButtonState.Released && Globals.oldMouseState.LeftButton == ButtonState.Pressed)
                    {
                        mi.Deselect();
                        mi.Activate(this);
                        click.Play();
                    }
                    break;
                }
                else
                    mi.Deselect();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (MenuItem mi in currentMenuItems)
                spriteBatch.DrawString(font, mi.name, mi.position, mi.color);

            spriteBatch.End();
        }

        public void DisplayMenu(List<MenuItem> newMenu)
        {
            currentMenuItems = newMenu;
            SetMenuPositions(currentMenuItems);
        }

        private void SetMenuPositions(List<MenuItem> menu)
        {
            for (int i = 0; i < menu.Count; i++)
            {
                if(menu[i].name == "Back")
                    menu[i].SetPosition(new Vector2(Globals.SCREEN_WIDTH - 350, Globals.SCREEN_HEIGHT -50), font);
                else
                    menu[i].SetPosition(new Vector2(MENU_ITEM_X, MENU_ITEM_Y + (i * (14 + MENU_ITEM_SPACING))), font);
            }
        }

        public void BackToMainMenu()
        {
            DisplayMenu(mainMenuItems);
        }
    }
}
