using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace StormGame
{
    static class Globals
    {
        public static SpriteFont Font1 { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }
        public static ContentManager Content { get; set; }
        public static GraphicsDevice GraphicsDevice { get; set; }
        public static GameState gameState { get; set; }
        public static Game1 game { get; set; }
        public static GameTime GameTime { get; set; }
        public static Vector2 cameraPosition { get; set; }

        public static MouseState mouseState;
        public static MouseState oldMouseState;
        public static KeyboardState keyboardState;
        public static KeyboardState oldKeyboardState;

        public const int SCREEN_WIDTH = 1280;
        public const int SCREEN_HEIGHT = 720;
        public static Vector2 SCREEN_CENTER = new Vector2(Globals.SCREEN_WIDTH / 2, Globals.SCREEN_HEIGHT / 2);

        public static bool editorMode;

    }
}
