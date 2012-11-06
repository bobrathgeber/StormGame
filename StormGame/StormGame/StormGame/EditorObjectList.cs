using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace StormGame
{
    public enum ObjectList
    {
        Destructibles = 1,
        Debris = 2,
        BackgroundTiles = 3
    }

    class EditorObjectList
    {
        private int firstObjectDisplayed=0;
        private Dictionary<int,DrawableObject> drawableObjects;
        private Dictionary<int, Texture2D> backgroundTiles;
        private DrawableObject heldObject;
        private ObjectList objectList;

        public EditorObjectList()
        {
            
        }

        public void LoadContent()
        {
            drawableObjects = new Dictionary<int, DrawableObject>();
            backgroundTiles = new Dictionary<int, Texture2D>();

            drawableObjects.Add(0, new StartingLocation(new Vector2(0)));
            drawableObjects.Add(1, new GenericDestructible(new Vector2(0), "wheat", "wheat", "wheat", 0.8f, 4, 15));
            drawableObjects.Add(2, new Cow(new Vector2(0)));
            drawableObjects.Add(3, new LargeDebris());
            drawableObjects.Add(4, new GenericDestructible(new Vector2(0), "MedBldgGood", "MedBldgPoor", "MedBldgDead", 1.0f, 1, 15));
            drawableObjects.Add(5, new GenericDestructible(new Vector2(0), "WalmartBldgGood", "WalmartBldgPoor", "WalmartBldgDead", 1.0f, 1, 15));
            drawableObjects.Add(6, new GenericDestructible(new Vector2(0), "FenceLeft", "FenceLeft", "FenceLeftDead", 1.0f, 1, 15));
            drawableObjects.Add(7, new GenericDestructible(new Vector2(0), "FenceRight", "FenceRight", "FenceRightDead", 1.0f, 1, 15));

            backgroundTiles.Add(0, Globals.Content.Load<Texture2D>("Tiles/DesertTile256"));
            backgroundTiles.Add(1, Globals.Content.Load<Texture2D>("Tiles/GrassTile256"));

            for (int i = 0; i < drawableObjects.Count; i++)
                drawableObjects[i].Update();

            objectList = ObjectList.Destructibles;
            UpdatePosition();
        }

        public void Update(Level level)
        {
            UpdateObjectList(); 
            DragOutNewObject(level);
            for (int i = 0; i < drawableObjects.Count; i++)
                drawableObjects[i].Update();

            SwitchObjectList();
        }

        private void SwitchObjectList()
        {
            if(Globals.keyboardState.IsKeyDown(Keys.D1) && Globals.oldKeyboardState.IsKeyUp(Keys.D1))
                objectList = ObjectList.Destructibles;
            else if (Globals.keyboardState.IsKeyDown(Keys.D2) && Globals.oldKeyboardState.IsKeyUp(Keys.D2))
                objectList = ObjectList.Debris;
            else if (Globals.keyboardState.IsKeyDown(Keys.D3) && Globals.oldKeyboardState.IsKeyUp(Keys.D3))
                objectList = ObjectList.BackgroundTiles;
        }

        private void DragOutNewObject(Level level)
        {
            if (Globals.mouseState.LeftButton == ButtonState.Pressed)
            {
                PickupNewObject();
                if (heldObject != null)
                {
                    level.DeselectAll();
                    MoveHeldObject();
                }
            }
            else if (heldObject != null && Globals.mouseState.LeftButton == ButtonState.Released)
                DropNewObject(level);
        }

        private void DropNewObject(Level level)
        {
            level.AddDrawableObject(heldObject);
            LoadContent();
            heldObject = null;
        }

        private void MoveHeldObject()
        {
            heldObject.Position = new Vector2(Globals.mouseState.X, Globals.mouseState.Y);
        }

        private void PickupNewObject()
        {
            if (heldObject == null)
            {
                for (int i = firstObjectDisplayed; i < drawableObjects.Count; i++)
                    if (drawableObjects[i].BoundingBox.Contains(new Point(Globals.mouseState.X, Globals.mouseState.Y)))
                        heldObject = drawableObjects[i];

            }
        }

        private void UpdateObjectList()
        {
            int maxAvailbleObjects = drawableObjects.Count;

            if (Globals.keyboardState.IsKeyDown(Keys.Down) && !Globals.oldKeyboardState.IsKeyDown(Keys.Down)
                || Globals.mouseState.ScrollWheelValue < Globals.oldMouseState.ScrollWheelValue)
            {
                if (firstObjectDisplayed > firstObjectDisplayed - 4)
                    firstObjectDisplayed++;
                UpdatePosition();
            }
            else if (Globals.keyboardState.IsKeyDown(Keys.Up) && !Globals.oldKeyboardState.IsKeyDown(Keys.Up)
                || Globals.mouseState.ScrollWheelValue > Globals.oldMouseState.ScrollWheelValue)
            {
                if (firstObjectDisplayed > 0)
                    firstObjectDisplayed--;
                UpdatePosition();
            }
        }

        public void Draw()
        {
            for(int i = firstObjectDisplayed; i<drawableObjects.Count; i++)
                drawableObjects[i].Draw();
        }

        private void UpdatePosition()
        {
            int bottomOfPrevObj = 0;
            for (int i = firstObjectDisplayed; i < drawableObjects.Count; i++)
            {
                drawableObjects[i].Position = new Vector2(drawableObjects[i].Width / 2, bottomOfPrevObj + drawableObjects[i].Height/2);
                bottomOfPrevObj = bottomOfPrevObj + (int)drawableObjects[i].Height;
            }
        }
    }
}
