using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace StormGame
{

    public enum LevelStatus
    {
        Playing,
        Died,
        Complete
    }

    public class Level
    {
        private Storm storm;
        private DrawableObjectCollection drawableObjects;
        private PopupWindowList popups;
        private LevelStatus levelStatus = LevelStatus.Playing;

        private StartingLocation startingLocation = new StartingLocation(new Vector2(0));
        private StormFront stormFront;

        private bool disableControls = false;
        private float disableControlsCoolDown;
        private float disableTime;

        //private float timeSinceKeyPressed;
        //private bool doubleKeyPressed;
        //private Keys lastKeyPressed;
        //private bool singleTap;

        public bool manualPause { get; set; }
        private bool paused;
        private Texture2D pauseOverlayTexture;

        

        private Camera _camera;
        private List<Layer> _layers;

        private Objective objective;
        //private SmokeParticleSystem smoke;

        //Editor Mode Variables
        //-------------------------------------
        private Vector2 mousePosition;
        private Vector2 oldMousePosition;
        private Vector2 absoluteMousePosition;
        private Vector2 oldAbsoluteMousePosition;
        private string levelFileName;
        private List<string> dataObjects;
        private List<DrawableObject> selectedObjects;
        private bool savingLevel;
        private string levelName="";
        private EditorObjectList editorObjectList;
        public int Width { get; set; }
        public int Height { get; set; }

        private const float STOPPINGFORCE = 0.95f;


        public Level(string levelFileName)
        {
            
            editorObjectList = new EditorObjectList();
            popups = new PopupWindowList();
            this.levelFileName = levelFileName;

            //Editor Objects
            //----------------------------------
            selectedObjects = new List<DrawableObject>();
            Globals.editorMode = false;
            paused = false;
            savingLevel = false;
            mousePosition = new Vector2();
            oldMousePosition = new Vector2();

            LoadContent();
        }

        public void LoadContent()
        {
            
            editorObjectList.LoadContent();
            
            _camera = new Camera(Globals.GraphicsDevice.Viewport) { Limits = new Rectangle(0, 0, 4000, 2000) };
            ResetManagers();
            
            pauseOverlayTexture = CreateRectangle(Globals.GraphicsDevice.Viewport.Width, Globals.GraphicsDevice.Viewport.Height, new Color(0, 0, 0, 150));

            GenericDestructible Walmart = new GenericDestructible(new Vector2(3360, 910), "Walmart", 1.0f, 5000);
            drawableObjects.Add(Walmart);
            objective = new Objective(Walmart);            
        }

        public void ResetManagers()
        {
            dataObjects = new List<string>();
            drawableObjects = new DrawableObjectCollection();
         

            LoadLevelFile(levelFileName);
            storm = new Storm(startingLocation);
            stormFront = new StormFront(new Vector2(0, Height /2));
            drawableObjects.Add(stormFront);
            UpdateCamera();
        }

        public void Update(GameTime gameTime)
        {
            paused = false;
            ManageInput(gameTime);

            //Level Editor Logic
            //-------------------------------
            if (Globals.editorMode)
            {
                editorObjectList.Update(this);
                if (savingLevel)
                    levelName = ModifyString(levelName);
                
                if (Globals.mouseState.LeftButton == ButtonState.Released && Globals.oldMouseState.LeftButton == ButtonState.Pressed)
                    SelectObject();
                else if (Globals.mouseState.LeftButton == ButtonState.Pressed) 
                    MoveSelectedObjects();
            }

            // Game Logic
            //-------------------------------
            else
            {
                UpdatePopup();

                if (!paused)
                {
                    MoveStorm(gameTime);
                    UpdateDestructible(gameTime);
                    UpdateItems(gameTime);
                    UpdateItems();
                    UpdateStormHealth();
                    CheckObjectiveCompletion();
                    stormFront.Update();
                }                
            }
        }

        private void UpdateItems()
        {
            foreach (var item in drawableObjects.OfType<Item>())
            {
                if (storm.withinPickupRangeOf(item) && !item.pickupBlocked)
                    storm.inventoryManager.PickupItem(item);
                item.Update();
            }
        }

        private void UpdatePopup()
        {
            if (popups.HasItems() || manualPause)
                paused = true;
            var p = popups.GetCurrentOrDefault();
            p.Update();
            if (p.isDead())
            {
                popups.Remove(p);
            }
        }

        private void CheckObjectiveCompletion()
        {
            if (objective.CheckCompletion())
            {
                if (popups.Count == 0 && levelStatus == LevelStatus.Complete || levelStatus == LevelStatus.Died)
                    Globals.gameState = GameState.LevelSelect;
                levelStatus = LevelStatus.Complete;
                popups.Add(new PopupWindow("You defeated the evil Walmart! :D"));
            }
        }

        private void UpdateStormHealth()
        {
            if(StormInFront())
                storm.stormHealth.ModifyHealth(-1);
            else
                storm.stormHealth.ModifyHealth(-3);
            if (storm.stormHealth.CurrentHealth <= 0)
            {
                ResetManagers();
                levelStatus = LevelStatus.Died;
                popups.Add(new PopupWindow("You have Died. :("));
                storm.stormHealth.ResetHealth();
            }
        }

        private bool StormInFront()
        {
            if (stormFront.BoundingBox.Contains(new Point((int)storm.Position.X, (int)storm.Position.Y)))
                return true;
            return false;
        }

        private void SelectObject()
        {
            bool objectWasSelected = false;
            int oldSelectionAmount = selectedObjects.Count;
            foreach (DrawableObject d in drawableObjects)
                objectWasSelected = SelectObject(d);
        }

        private string ModifyString(string textString)
        {
            Keys[] pressedKeys;
            pressedKeys = Globals.keyboardState.GetPressedKeys();

            foreach (Keys key in pressedKeys)
            {
                if (Globals.oldKeyboardState.IsKeyUp(key))
                {
                    if (key == Keys.Back) // overflows
                        textString = textString.Remove(textString.Length - 1, 1);
                    else
                        if (key == Keys.Space)
                            textString = textString.Insert(textString.Length, " ");
                        else
                            textString += key.ToString();
                }
            }
            return textString;
        } 

        private bool SelectObject(DrawableObject gameObject)
        {
            if (gameObject.BoundingBox.Contains(new Point((int)absoluteMousePosition.X, (int)oldAbsoluteMousePosition.Y)))
            {
                if (Globals.keyboardState.IsKeyDown(Keys.LeftAlt))
                {
                    DeselectObject(gameObject);
                    return false;
                }
                else if (!gameObject.Selected)
                {
                    selectedObjects.Add(gameObject);
                    gameObject.Selected = !gameObject.Selected;
                    return true;
                }                
            }
            return false;
        }

        private void DeselectObject(DrawableObject gameObject)
        {
            selectedObjects.Remove(gameObject);
            gameObject.Selected = !gameObject.Selected;
        }

        private void MoveSelectedObjects()
        {
            foreach (DrawableObject d in drawableObjects)
            {
                if (d.Selected)
                    d.Position += absoluteMousePosition - oldAbsoluteMousePosition;
            }
        }

        public void Draw()
        {
            foreach (Layer layer in _layers)
                layer.Draw(Globals.SpriteBatch, drawableObjects, storm);

            Globals.SpriteBatch.Begin();
            
            if (Globals.editorMode)
            {
                editorObjectList.Draw();
                Globals.SpriteBatch.DrawString(Globals.Font1, "Selected Object Position", new Vector2(5, Globals.SCREEN_HEIGHT-60), Color.White);
                if (selectedObjects.Count > 0)
                {
                    Globals.SpriteBatch.DrawString(Globals.Font1, "  X: " + selectedObjects[0].Position.X.ToString() + "   " + "Y: " + selectedObjects[0].Position.Y.ToString(), new Vector2(5, Globals.SCREEN_HEIGHT - 45), Color.White);
                    Globals.SpriteBatch.DrawString(Globals.Font1, "  Origin X: " + selectedObjects[0].Origin.X.ToString() + "   " + "Y: " + selectedObjects[0].Origin.Y.ToString(), new Vector2(5, Globals.SCREEN_HEIGHT - 30), Color.White);
                    Globals.SpriteBatch.DrawString(Globals.Font1, "  Scale X: " + selectedObjects[0].scale.X.ToString() + "   " + "Y: " + selectedObjects[0].scale.Y.ToString(), new Vector2(5, Globals.SCREEN_HEIGHT - 15), Color.White);
                
                }
                Globals.SpriteBatch.DrawString(Globals.Font1, "Mouse Position", new Vector2(5, Globals.SCREEN_HEIGHT), Color.White);
                Globals.SpriteBatch.DrawString(Globals.Font1, "  X: " + (mousePosition.X + _camera.Position.X).ToString() + "   " + "Y: " + (mousePosition.Y + _camera.Position.Y).ToString(), new Vector2(5, Globals.SCREEN_HEIGHT + 15), Color.White);
                Globals.SpriteBatch.DrawString(Globals.Font1, "Camera Position" , new Vector2(5, Globals.SCREEN_HEIGHT + 30), Color.White);
                Globals.SpriteBatch.DrawString(Globals.Font1, "  cX: " + _camera.Position.X.ToString() + "   " + "Y: " + _camera.Position.Y.ToString(), new Vector2(5, Globals.SCREEN_HEIGHT + 45), Color.White);
                if (savingLevel)
                {
                    Globals.SpriteBatch.DrawString(Globals.Font1, levelName, Globals.SCREEN_CENTER, Color.Black);
                }

            }
            else if (paused)
            {
                var _string = "PAUSED";
                var pos = new Vector2(Globals.GraphicsDevice.Viewport.Width / 2 - 50, 200);
                Globals.SpriteBatch.Draw(pauseOverlayTexture, new Vector2(0), Color.Blue);
                Globals.SpriteBatch.DrawString(Globals.Font1, _string, pos + new Vector2(1.0f, 1.0f), Color.Black);
                Globals.SpriteBatch.DrawString(Globals.Font1, _string, pos, Color.White);
            }

            popups.GetCurrentOrDefault().Draw();

            Globals.SpriteBatch.End();

        }


        private void UpdateItems(GameTime gameTime)
        {
            foreach (var item in drawableObjects.OfType<Item>())
            {
                item.Update();
                
                foreach (var destructible in drawableObjects.OfType<Destructible>())
                {
                    if (destructible.CheckCollision(item.BoundingBox) && item.CooldownReady)
                    {
                        destructible.DamageHealth(item.Damage);
                        item.Collide();
                    }
                }
            }
        }

        public void ToggleManualPause()
        {
            manualPause = !manualPause;
        }

        //public void SpawnDebris(Vector2 origin)
        //{
        //    Debris newdebris = new LargeDebris();
        //    drawableObjects.Add(newdebris);
        //    newdebris.Eject(origin);
        //}

        private void ManageInput(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            oldMousePosition = mousePosition;
            oldAbsoluteMousePosition = absoluteMousePosition;

            absoluteMousePosition = new Vector2(Globals.mouseState.X + _camera.Position.X, Globals.mouseState.Y + _camera.Position.Y);
            mousePosition = new Vector2(Globals.mouseState.X, Globals.mouseState.Y);


            Vector2 Accel = new Vector2();
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (disableControls)
            {
                disableTime += deltaTime;
                if (disableTime > disableControlsCoolDown)
                {
                    disableControls = false;
                }
            }
            else
            {
                CheckDoublePress(elapsedTime);

                if (Globals.keyboardState.IsKeyDown(Keys.Right) || Globals.keyboardState.IsKeyDown(Keys.D))
                    Accel.X += storm.speed * elapsedTime;

                else if (Globals.keyboardState.IsKeyDown(Keys.Left) || Globals.keyboardState.IsKeyDown(Keys.A))
                    Accel.X += -storm.speed * elapsedTime;

                if (Globals.keyboardState.IsKeyDown(Keys.Up) || Globals.keyboardState.IsKeyDown(Keys.W))
                    Accel.Y += -storm.speed * elapsedTime;

                else if (Globals.keyboardState.IsKeyDown(Keys.Down) || Globals.keyboardState.IsKeyDown(Keys.S))
                    Accel.Y += storm.speed * elapsedTime;

                if (Globals.keyboardState.IsKeyDown(Keys.Space) && !storm.isExpanding && !(Globals.oldKeyboardState.IsKeyDown(Keys.Space)))
                    storm.Expand(deltaTime);             

                storm.Velocity += Accel;
            }

            if (Globals.keyboardState.IsKeyDown(Keys.F12) && !Globals.oldKeyboardState.IsKeyDown(Keys.F12))
                ToggleManualPause();

            if (Globals.keyboardState.IsKeyDown(Keys.F9) && !Globals.oldKeyboardState.IsKeyDown(Keys.F9))
                ToggleEditorMode();

            
            

            //Editor Mode Controls
            //-----------------------------------
            if (Globals.editorMode)
            { 
                if (Globals.mouseState.RightButton == ButtonState.Pressed)
                    _camera.Position -= (mousePosition) - (oldMousePosition);
                if (Globals.keyboardState.IsKeyDown(Keys.F5) && !Globals.oldKeyboardState.IsKeyDown(Keys.F5) && Globals.editorMode)
                    savingLevel = true;
                if (Globals.keyboardState.IsKeyDown(Keys.Enter) && !Globals.oldKeyboardState.IsKeyDown(Keys.Enter) && savingLevel)
                {
                    savingLevel = false;
                    SaveLevelFile(levelName);
                }
                if (Globals.keyboardState.IsKeyDown(Keys.D) && !Globals.oldKeyboardState.IsKeyDown(Keys.D) && !savingLevel)
                    DeselectAll();
                if (Globals.keyboardState.IsKeyDown(Keys.Delete) && !Globals.oldKeyboardState.IsKeyDown(Keys.Delete) && !savingLevel)
                    DeleteObjects();
            }
        }

        private void DeleteObjects()
        {
            for(int i = drawableObjects.Count-1; i>=0; i--)
                if (drawableObjects[i].Selected)
                    drawableObjects.Remove(drawableObjects[i]);
        }

        private void CheckDoublePress(float elapsedTime)
        {
            //if (Globals.controllerState.IsDoubleTap(Keys.Down))
            //    storm.speed = 25;
            //else
            //    storm.speed = 15;

            //Executed Single Tap
            // IF FIRST KEYDOWN
            // save active key
            // start timer
            // save as 'last key pressed'

            // If keyup
            // reset first key down flag

            // If first keydown matches last key pressed
            // if less than double tap thresh
            // run



            //if (Globals.oldKeyboardState.IsKeyUp(Keys.Right) && Globals.keyboardState.IsKeyDown(Keys.Right))
            //{
            //    if (!singleTap)
            //    {
            //        singleTap = true;
            //        timeSinceKeyPressed = 0;
            //        lastKeyPressed = Keys.Right;
            //    }
            //    else if(timeSinceKeyPressed < 0.2f) 
            //    {
            //        doubleKeyPressed = true;
            //        storm.speed = 25;
            //    }
            //    else
            //    {
            //        singleTap = false;
            //        doubleKeyPressed = false;
            //        storm.speed = 5;
            //        timeSinceKeyPressed += elapsedTime;
            //    }
            //}                
            //else if (Globals.keyboardState.IsKeyUp(lastKeyPressed))
            //{
            //    timeSinceKeyPressed += elapsedTime;
            //    storm.speed = 5;
            //}
            
        }

        private void ToggleEditorMode()
        {
            Globals.editorMode = !Globals.editorMode;
        }

        

        private void MoveStorm(GameTime gameTime)
        {
            storm.Update(gameTime);            
            CheckImpassableCollisions(drawableObjects);
            UpdateCamera();
            storm.Velocity *= STOPPINGFORCE;
            storm.ApplyVelocity();        

        }

        private void CheckImpassableCollisions(List<DrawableObject> listOfObjects)
        {
            foreach (DrawableObject obj in listOfObjects)
            {
                if (obj.CheckCollision(storm.Position) && obj.isAlive)
                {
                    var stormCenter = storm.Position;
                    var box = obj.BoundingBox;

                    var distances = new Dictionary<string, float>();
                    distances.Add("Left", Math.Abs(stormCenter.X - box.Left));
                    distances.Add("Right", Math.Abs(stormCenter.X - box.Right));
                    distances.Add("Top", Math.Abs(stormCenter.Y - box.Top));
                    distances.Add("Bottom", Math.Abs(stormCenter.Y - box.Bottom));

                    // sorting the list smallest to biggest and grabbing the first item
                    var minDistance = distances.OrderBy(x => x.Value).First();

                    var leftRightBounceVector = new Vector2(-1, 1);
                    var topBottomBounceVector = new Vector2(1, -1);

                    // assume top/bottom until proven otherwise
                    var bounceVector = topBottomBounceVector;
                    if (minDistance.Key == "Left" || minDistance.Key == "Right")
                    {
                        bounceVector = leftRightBounceVector;
                    }

                    storm.BounceOffObject(bounceVector);

                    while (obj.CheckCollision(storm.Position))
                    {
                        storm.Velocity *= 1.1f;
                        storm.ApplyVelocity();
                    }

                    DisableStormMovement(0.3f);
                }
            }
            
        }

        private void DisableStormMovement(float p)
        {
            disableControlsCoolDown = p;
            disableControls = true;
            disableTime = 0;
        }

        private void UpdateDestructible(GameTime gameTime)
        {
            var ds = drawableObjects.CopyFilter<Destructible>();
            var destructablesJustKilled = new List<Destructible>();
            foreach (var destructible in ds)
            {
                destructible.Update();
                if (destructible.isAlive)
                {
                    destructible.UpdateTexture();

                    if (destructible.CurrentHealth <= 0)
                    {                        
                        addDrawableObject(destructible.onDeath(storm));
                        storm.stormHealth.ModifyHealth(2000);
                    }
                }
                destructible.UpdateDamagePopups(gameTime);
            }
            
        }

        public void addDrawableObject(DrawableObject dObj)
        {
            if (dObj != null)
                drawableObjects.Add(dObj);
        }

        private void UpdateCamera()
        {
            _camera.Position = storm.Position - _camera.Origin;
        }

        static private Texture2D CreateRectangle(int width, int height, Color colori)
        {
            Texture2D rectangleTexture = new Texture2D(Globals.GraphicsDevice, width, height, false, 
            SurfaceFormat.Color);// create the rectangle texture, ,but it will have no color! lets fix that
            Color[] color = new Color[width * height];//set the color to the amount of pixels in the textures
            for (int i = 0; i < color.Length; i++)//loop through all the colors setting them to whatever values we want
            {
                color[i] = colori;
            }
            rectangleTexture.SetData(color);//set the color data on the texture
            return rectangleTexture;//return the texture
        }

        private void LoadLevelFile(string fileName)
        {
            string data;
            using (StreamReader sr = new StreamReader(fileName))
            {
                while ((data = sr.ReadLine()) != null)
                {
                    dataObjects.Add(data);

                    var line = data.Split('%');

                    //Dimentions
                    //Identifier  %  Width  %  Height
                    //----------------------------------------------
                    if (line[0] == "dimentions")
                    {
                        Width = int.Parse(line[1]);
                        Height = int.Parse(line[2]);
                        _camera = new Camera(Globals.GraphicsDevice.Viewport) { Limits = new Rectangle(0, 0, int.Parse(line[1]), int.Parse(line[2])) };
                        drawableObjects.Add(new BlockerObject("1x1", new Vector2(-50, -50), new Vector2((Width + 100), 50), false)); //top
                        drawableObjects.Add(new BlockerObject("1x1", new Vector2(Width, 0), new Vector2(50, Height), false)); //right
                        drawableObjects.Add(new BlockerObject("1x1", new Vector2(-50, Height), new Vector2((Width + 100), 50), false)); //bottom
                        drawableObjects.Add(new BlockerObject("1x1", new Vector2(-50, 0), new Vector2(50, Height), false)); //left
                        _layers = new List<Layer>{
                            new Layer(_camera) { Parallax = new Vector2(1.0f, 1.0f) }
                        };
                    }


                    //Popups
                    //Identifier  %  Text  %  Trigger
                    //----------------------------------------------
                    if (line[0] == "popup")
                    {
                        popups.Add(new PopupWindow(System.Text.RegularExpressions.Regex.Unescape(line[1])));
                    }

                    //Tiles; images used for details
                    //Identifier  %  X-Coord  %  Y-Coord  %  FileName % Is Tiled
                    //----------------------------------------------
                    if (line[0] == "tile")
                    {
                        if(line[4] == "true")
                            _layers[0].Sprites.Add(new Sprite(Globals.Content.Load<Texture2D>(line[3]), new Vector2(float.Parse(line[1]), float.Parse(line[2])), 0,true, Width,Height));
                        else
                            _layers[0].Sprites.Add(new Sprite(Globals.Content.Load<Texture2D>(line[3]), new Vector2(float.Parse(line[1]), float.Parse(line[2])), 0));
                    }

                    //Destructible Objects
                    //Identifier  %  X-Coord  %  Y-Coord
                    //----------------------------------------------
                    if (line[0] == "mediumbuilding")
                    {
                        drawableObjects.Add(new GenericDestructible(new Vector2(float.Parse(line[1]), float.Parse(line[2])), "House1", 1f, 50));
                    }

                    //Impassibles
                    //Identifier  %  X-Coord  %  Y-Coord  %  Scale
                    //----------------------------------------------
                    if (line[0] == "impassible")
                    {
                        drawableObjects.Add(new BlockerObject(line[6], new Vector2(float.Parse(line[1]), float.Parse(line[2])), new Vector2(float.Parse(line[3]), float.Parse(line[4])), string.Equals(line[5], "true")));
                    }

                    //Storm
                    //Identifier  %  X-Coord  %  Y-Coord
                    //----------------------------------------------
                    if (line[0] == "startinglocation")
                    {
                        startingLocation = new StartingLocation(new Vector2(float.Parse(line[1]), float.Parse(line[2])));
                        drawableObjects.Add(startingLocation);
                    }

                    //Debris
                    //Identifier  %  X-Coord  %  Y-Coord
                    //----------------------------------------------
                    if (line[0] == "cow")
                        drawableObjects.Add(new Cow(new Vector2(float.Parse(line[1]), float.Parse(line[2]))));
                    if (line[0] == "largedebris")
                        drawableObjects.Add(new LargeDebris(new Vector2(float.Parse(line[1]), float.Parse(line[2]))));                        
                }
            }
        }

        private void SaveLevelFile(string levelName)
        {
            string filePath = "Levels/" + levelName + ".txt";

            using (FileStream stream = File.Open(filePath, FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(stream);
                WriteHeaderData(sw);
                WriteLevelData(sw);                
                stream.Flush();
                sw.Close();
            }  
        }

        private void WriteHeaderData(StreamWriter sw)
        {
            sw.WriteLine("popup%Welcome to Storm Game!%start");
            sw.WriteLine("popup%Use Arrow Keys to Move. \nUse Spacebar to explode.%start");
            sw.WriteLine("dimentions%4000%2000");
            sw.WriteLine("startinglocation%600%600");
            sw.WriteLine("tile%0%0%beach1-1");
            sw.WriteLine("tile%2000%0%beach1-2");
        }

        private void WriteLevelData(StreamWriter sw)
        {
            foreach (DrawableObject obj in drawableObjects)
            {
                string data="";
                if(obj.Identifier == "mediumbuilding" ||
                    obj.Identifier == "cow" ||
                    obj.Identifier == "largedebris")
                data += obj.Identifier + "%" + obj.Position.X + "%" + obj.Position.Y;

                //if(obj.Identifier == "tile")

                sw.WriteLine(data);
            }
        }

        public void AddDrawableObject(DrawableObject newObject)
        {
            newObject.Position += _camera.Position;
            drawableObjects.Add(newObject);
        }

        public void DeselectAll()
        {
            foreach (DrawableObject d in drawableObjects)
                d.Selected = false;
        }
    }
}