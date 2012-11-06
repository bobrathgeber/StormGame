using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StormGame
{
    class LevelSelectorButton : Button
    {
        public string data;

        public LevelSelectorButton(Vector2 position, string buttonLabel, Texture2D upTex, Texture2D downTex, Texture2D hoverTex, string data)
            : base(position, buttonLabel, upTex, downTex, hoverTex)
        {
            this.data = data;
        }

        public bool CompareData(string str)
        {
            return str.Equals(data); 
        }

    }
}
