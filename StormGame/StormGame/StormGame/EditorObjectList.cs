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
        private Dictionary<int, Sprite> backgroundTiles;
        private DrawableObject heldObject;
        private ObjectList objectList;
        private Texture2D pixil = new Texture2D(Globals.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);  

        public EditorObjectList()
        {
            
        }

        public void LoadContent()
        {
            drawableObjects = new Dictionary<int, DrawableObject>();
            backgroundTiles = new Dictionary<int, Sprite>();

            drawableObjects.Add(0, new StartingLocation(new Vector2(0)));
            drawableObjects.Add(1, new GenericDestructible(new Vector2(0), "Wheat", 0.8f, 15));
            drawableObjects.Add(2, new Cow(new Vector2(0)));
            drawableObjects.Add(3, new LargeDebris());
            drawableObjects.Add(4, new GenericDestructible(new Vector2(0), "House1", 1.0f, 15));
            drawableObjects.Add(5, new GenericDestructible(new Vector2(0), "Walmart", 1.0f, 15));
            //drawableObjects[5].scale = new Vector2(0.1f, 0.1f);
            drawableObjects.Add(6, new GenericDestructible(new Vector2(0), "LeftFence", 1.0f, 15));
            drawableObjects.Add(7, new GenericDestructible(new Vector2(0), "RightFence", 1.0f, 15));

            backgroundTiles.Add(0, new Sprite("Tiles/DesertTile256",Vector2.Zero, 0, false));
            backgroundTiles.Add(1, new Sprite("Tiles/GrassTile256",Vector2.Zero, 0, false));

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
            switch (objectList)
            {
                case ObjectList.Destructibles:
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
                    break;

                case ObjectList.BackgroundTiles:
                    if (Globals.mouseState.LeftButton == ButtonState.Pressed && Globals.oldMouseState.LeftButton == ButtonState.Released)
                    {
                        for (int i = 0; i < backgroundTiles.Count; i++)
                            if (new Rectangle(5, 5 + i * backgroundTiles[i].Texture.Height, backgroundTiles[i].Texture.Width, backgroundTiles[i].Texture.Height).Contains(new Point(Globals.mouseState.X, Globals.mouseState.Y)))
                            {
                                level.ChangeBackgroundTile(backgroundTiles[i].Identifier);
                            }
                    }
                    break;
            }
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
                    {
                        heldObject = drawableObjects[i];
                        drawableObjects[i].scale = Vector2.One;
                    }

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
            switch (objectList)
            {
                case ObjectList.Destructibles:
                    for (int i = firstObjectDisplayed; i < drawableObjects.Count; i++)
                        drawableObjects[i].Draw();
                    break;

                case ObjectList.BackgroundTiles:
                    for (int i = 0; i < backgroundTiles.Count; i++)
                    {
                        Globals.SpriteBatch.Draw(backgroundTiles[i].Texture, new Vector2(5, 5 + i * backgroundTiles[i].Height), Color.White);
                        DrawBoundingBox(backgroundTiles[i].Texture, new Vector2(5, 5 + i * backgroundTiles[i].Height));
                    }
                    break;
            }
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

        private void DrawBoundingBox(Texture2D texture, Vector2 position)
        {            
            pixil.SetData(new[] { Color.Black });
            float length1 = Vector2.Distance(new Vector2(texture.Bounds.X, texture.Bounds.Y), new Vector2(texture.Bounds.X + texture.Width, texture.Bounds.Y));
            float length2 = Vector2.Distance(new Vector2(texture.Bounds.X + texture.Width, texture.Bounds.Y), new Vector2(texture.Bounds.X + texture.Width, texture.Bounds.Y+texture.Height));
            float length3 = Vector2.Distance(new Vector2(texture.Bounds.X + texture.Width, texture.Bounds.Y + texture.Height), new Vector2(texture.Bounds.X, texture.Bounds.Y + texture.Height));
            float length4 = Vector2.Distance(new Vector2(texture.Bounds.X, texture.Bounds.Y + texture.Height), new Vector2(texture.Bounds.X, texture.Bounds.Y));

            Globals.SpriteBatch.Draw(pixil, new Vector2(position.X, position.Y), null, Color.White, MathHelper.ToRadians(270), Vector2.Zero, new Vector2(3, length1), SpriteEffects.None, 0);
            Globals.SpriteBatch.Draw(pixil, new Vector2(position.X + texture.Width, position.Y), null, Color.White, MathHelper.ToRadians(90), Vector2.Zero, new Vector2(length2, 3), SpriteEffects.None, 0);
            Globals.SpriteBatch.Draw(pixil, new Vector2(position.X + texture.Width, position.Y + texture.Height), null, Color.White, MathHelper.ToRadians(90), Vector2.Zero, new Vector2(3, length3), SpriteEffects.None, 0);
            Globals.SpriteBatch.Draw(pixil, new Vector2(position.X, position.Y + texture.Height), null, Color.White, MathHelper.ToRadians(270), Vector2.Zero, new Vector2(length4, 3), SpriteEffects.None, 0);
        }
    }
}
