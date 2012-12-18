using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StormGame
{
    class TitleScreen
    {
        private List<Texture2D> _images;
        private TimeSpan _displayDuration;
        private TimeSpan _fadeSpeed;
        private TimeSpan _time;
        private KeyboardState _keyboardState;
        private KeyboardState _oldKeyboardState;
        private int _currentImage = 0;
        private bool _skippable;
        private float _currentAlpha = 0;
        private TitleImageState _imageState = TitleImageState.FadingIn;
        private Vector2 _screenSize;


        public bool Finished = false;

        public TitleScreen(Texture2D image, Vector2 ScreenSize, TimeSpan displayDuration, TimeSpan fadeSpeed, bool skippable)
        {
            _displayDuration = displayDuration;
            _fadeSpeed = fadeSpeed;
            _skippable = skippable;
            _screenSize = ScreenSize;
            _images = new List<Texture2D>();
            AddImage(image);
            _oldKeyboardState = Keyboard.GetState();
        }

        public void AddImage(Texture2D image)
        {
            _images.Add(image);
        }

        public void Update(GameTime gameTime)
        {
            _keyboardState = Keyboard.GetState();
            _time += gameTime.ElapsedGameTime;

            if (_keyboardState.IsKeyDown(Keys.Space) && _oldKeyboardState.IsKeyUp(Keys.Space) && _skippable)
                NextImage();

            switch (_imageState)
            {
                case TitleImageState.FadingIn:
                    if (_time >= _fadeSpeed)
                    {
                        _imageState = TitleImageState.FullAlpha;
                        _time = TimeSpan.Zero;
                        _currentAlpha = 1;
                    }
                    else
                        _currentAlpha = MathHelper.Lerp(0, 1, (float)_time.Ticks / (float)_fadeSpeed.Ticks);
                    break;

                case TitleImageState.FullAlpha:
                    if (_time >= _displayDuration)
                    {
                        _imageState = TitleImageState.FadingOut;
                        _time = TimeSpan.Zero;
                    }
                    break;

                case TitleImageState.FadingOut:
                    if (_time >= _fadeSpeed)                    
                        NextImage();
                    else
                        _currentAlpha = MathHelper.Lerp(1, 0, (float)_time.Ticks / (float)_fadeSpeed.Ticks);
                    break;
            }
            _oldKeyboardState = _keyboardState;

        }

        private void NextImage()
        {
            _time = TimeSpan.Zero;
            _currentImage++;
            _currentAlpha = 0;
            _imageState = TitleImageState.FadingIn;
            if (_currentImage >= _images.Count)
                Finished = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {            
            var color = new Color(1, 1, 1, _currentAlpha);
            Vector2 position = new Vector2(_screenSize.X/2 - _images[_currentImage].Bounds.Width / 2, _screenSize.Y/2 - _images[_currentImage].Bounds.Height / 2);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            spriteBatch.Draw(_images[_currentImage], position, color);
            spriteBatch.End();
            Console.WriteLine(_imageState + " " + _currentAlpha);
        }
    }

    public enum TitleImageState
    {
        /// <summary>
        /// Return to pre-fade volume
        /// </summary>
        FadingIn,
        /// <summary>
        /// Snap to fade target volume
        /// </summary>
        FullAlpha,
        /// <summary>
        /// Keep current volume
        /// </summary>
        FadingOut
    }
}
