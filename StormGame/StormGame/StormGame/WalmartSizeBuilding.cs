using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace StormGame
{
    class WalmartSizeBuilding : Destructible
    {
        private Texture2D tex1;
        private Texture2D tex2;
        private Texture2D tex3;

        private List<Texture2D> textures;
        private int health;
        //**************************************************************
        //THIS IS AN OLD CLASS, REPLACED BY GenericDestructible
        //**************************************************************

        public WalmartSizeBuilding(Vector2 pos)
            : base()
        {
            textures = new List<Texture2D>();

            tex1 = Globals.Content.Load<Texture2D>("WalmartBldgGood");
            tex2 = Globals.Content.Load<Texture2D>("WalmartBldgPoor");
            tex3 = Globals.Content.Load<Texture2D>("WalmartBldgDead");
            textures.Add(tex1);
            textures.Add(tex2);
            textures.Add(tex3);
            health = 4000;

            this.LoadContent(pos, health);
        }

        public override DrawableObject onDeath(Storm storm)
        {

            return null;
        }

    }
}
