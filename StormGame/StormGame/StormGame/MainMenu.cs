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
        private List<MenuItem> mainMenuItems;
        private List<MenuItem> currentMenuItems;
        private MenuItem selectedMenuItem;
        private const int MENU_ITEM_X = 150;
        private const int MENU_ITEM_Y = 400;
        private const int MENU_ITEM_SPACING = 50;
        private SpriteFont font;

        //Visual Background Variables
        private Texture2D windGraphic;
        private Texture2D background1;
        private int[,] windCoordinates;

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

            DisplayMenu(mainMenuItems);
            background1 = Globals.Content.Load<Texture2D>("menubg");
            windGraphic = Globals.Content.Load<Texture2D>("MainMenuWind");
            windCoordinates = new int[,] { { -250, 10 }, { -220, 20 }, { -300, 40 }, { 0, 360 }, 
                                           { -350, 66 }, { -200, 142 }, { -400, 155 }, { -320, 187 },
                                           { -450, 166 }, { -300, 432 }, { -400, 225 }, { -220, 287 },
                                           { -550, 266 }, { -400, 222 }, { -400, 445 }, { -120, 387 },
                                           { -650, 366 }, { -400, 272 }, { -500, 255 }, { -220, 487 },
                                           { -750, 466 }, { -500, 552 }, { -500, 355 }, { -420, 587 },
                                           { -750, 566 }, { -600, 772 }, { -300, 455 }, { -220, 687 },
                                           { 250, 10 }, { 220, 20 }, { 300, 40 }, { 50, 360 }, 
                                           { 350, 66 }, { 200, 142 }, { 400, 155 }, { 320, 187 },
                                           { 450, 166 }, { 300, 432 }, { 400, 225 }, { 220, 287 },
                                           { 550, 266 }, { 400, 222 }, { 400, 445 }, { 120, 387 },
                                           { 650, 366 }, { 400, 272 }, { 500, 255 }, { 220, 487 },
                                           { 750, 466 }, { 500, 552 }, { 500, 355 }, { 420, 587 },
                                           { 750, 566 }, { 600, 772 }, { 300, 455 }, { 220, 687 },
                                           { 650, 10 }, { 720, 20 }, { 800, 200 }, { 900, 360 }, 
                                           { 550, 66 }, { 400, 142 }, { 400, 255 }, { 420, 657 }};
        }

        public void Update()
        {
            UpdateMenuItems();
            UpdateWind();
        }

        private void UpdateWind()
        {
            for (int i = 0; i < windCoordinates.Length/2; i++)
            {                
                if(windCoordinates[i,0] > Globals.SCREEN_WIDTH)
                    windCoordinates[i, 0] -= (int)(Globals.SCREEN_WIDTH * 1.5f) ;
                else
                    windCoordinates[i, 0] += 50;
            }
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
                        Globals.audioManager.PlaySound("Sounds/click");
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
            Globals.SpriteBatch.Draw(background1, Vector2.Zero, Color.White);
            DrawWind();
            foreach (MenuItem mi in currentMenuItems)
                spriteBatch.DrawString(font, mi.name, mi.position, mi.color);

            spriteBatch.End();
        }

        private void DrawWind()
        {
            for (int i = 0; i < windCoordinates.Length/2; i++)
                Globals.SpriteBatch.Draw(windGraphic, new Vector2(windCoordinates[i, 0], windCoordinates[i, 1]), Color.White);
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
                    menu[i].SetPosition(new Vector2(Globals.SCREEN_WIDTH - 350, Globals.SCREEN_HEIGHT -150), font);
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
