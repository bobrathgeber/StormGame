using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StormGame
{
    //Name
    //On Activate
    //States for selected/hovered
    //Optional Activations: Pop-up
    //                      Load Level
    //                      Open Menu
    //Will know previous Menu (Back)

    abstract class MenuItem
    {
        public string name;
        public Vector2 position;        
        public Color color;
        private Rectangle BoundingBox;
        private bool isSelected;

        private const int MENU_ITEM_X = 150;
        private const int MENU_ITEM_Y = 400;
        private const int MENU_ITEM_SPACING = 50;

        public MenuItem(string name)
        {
            this.name = name;
            Deselect();            
        }

        protected void CreateBoundingBox(SpriteFont font)
        {
            Vector2 dimensions = font.MeasureString(name);
            BoundingBox = new Rectangle((int)position.X, (int)position.Y, (int)dimensions.X, (int)dimensions.Y);
        }

        public void SetPosition(Vector2 pos, SpriteFont font)
        {
            position = pos;
            CreateBoundingBox(font);
        }

        public void Select()
        {
            color = Color.DarkRed;
            if (!isSelected)
            {
                position.X += 20;
                BoundingBox.Width += 20;
            }

            isSelected = true;
        }

        public void Deselect()
        {            
            color = Color.White;
            if (isSelected)
            {
                position.X -= 20;
                BoundingBox.Width -= 20;
            }

            isSelected = false;
        }

        public bool CheckHover()
        {
            if (BoundingBox.Contains(Globals.mouseState.X, Globals.mouseState.Y))
                return true;
            else
                return false;

        }

        public virtual void Update() { }

        public virtual void Draw(SpriteBatch spritebatch)
        {
            spritebatch.DrawString(Globals.Font1, name, position, color);
        }

        public virtual void Activate(MainMenu mainMenu) { }
    }
}
