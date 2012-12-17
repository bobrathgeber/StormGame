using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StormGame
{
    public enum OptionType
    {
        MasterVolume = 1,
        FullScreen = 2,
        Resolution = 3
    }

    class MenuItemOption
    {
        //Needs Left and Right arrows and controls
        //Needs a list of "options" have have a name and a function; "MenuItemOption"
        public Vector2 Position;
        private Button _leftButton;
        private Button _rightButton;
        private float _currentOption;
        private OptionType _type;
        private const int NUMBER_OF_VOLUME_INTERVALS = 20;

        public MenuItemOption(OptionType optionType, Vector2 position)
        {
            _type = optionType;
            Position = position;
            _leftButton = new Button(Vector2.Zero, "", Globals.Content.Load<Texture2D>("LeftArrow"), Globals.Content.Load<Texture2D>("LeftArrow"), Globals.Content.Load<Texture2D>("LeftArrow"));
            _rightButton = new Button(Vector2.Zero, "", Globals.Content.Load<Texture2D>("RightArrow"), Globals.Content.Load<Texture2D>("RightArrow"), Globals.Content.Load<Texture2D>("RightArrow"));
            _leftButton.Position = new Point((int)Position.X, (int)Position.Y);
            _rightButton.Position = new Point((int)Position.X + 100, (int)Position.Y);
            ChangeOption(1.0f);
        }

        public void Update()
        {
            _leftButton.Update();
            _rightButton.Update();

            if (_leftButton.Clicked)
            {
                if (_currentOption == 0)
                    ChangeOption(NUMBER_OF_VOLUME_INTERVALS);
                else
                    ChangeOption(_currentOption-1);                
            }
            if (_rightButton.Clicked)
            {
                if (_currentOption == NUMBER_OF_VOLUME_INTERVALS)
                    ChangeOption(0);
                else
                    ChangeOption(_currentOption+1);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _leftButton.Draw(spriteBatch);
            _rightButton.Draw(spriteBatch);
            spriteBatch.DrawString(Globals.Font1, _currentOption.ToString(), new Vector2(_leftButton.Position.X+50, _leftButton.Position.Y), Color.White);
        }

        private void ChangeOption(float optionNum)
        {
            _currentOption = optionNum;
            switch (_type)
            {
                case OptionType.MasterVolume:                    
                    Globals.audioManager.MusicVolume = MathHelper.Lerp(0f, 1f, _currentOption / NUMBER_OF_VOLUME_INTERVALS);
                    break;

                case OptionType.FullScreen:
                    Globals.audioManager.MusicVolume = MathHelper.Lerp(0f, 1f, _currentOption / NUMBER_OF_VOLUME_INTERVALS);
                    break;
            }
        }
    }
}
