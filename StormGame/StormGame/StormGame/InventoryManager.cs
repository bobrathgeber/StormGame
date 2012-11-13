using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace StormGame
{
    class InventoryManager : GameplayUIElement
    {

        public List<Item> satalliteList;

        private Texture2D smDebriBoxTexture;
        private Texture2D lgDebriBoxTexture;
        private int maxSpecial;
        private int maxNormal;

        public InventoryManager()
        {
            smDebriBoxTexture = Globals.Content.Load<Texture2D>("smDebrisBox");
            lgDebriBoxTexture = Globals.Content.Load<Texture2D>("lgDebrisBox");
            Position = new Vector2(50);

            satalliteList = new List<Item>();

            maxSpecial = 3;
            maxNormal = 10;
        }

        public void Add(Item debris)
        {
            satalliteList.Add(debris);
            debris.StartOrbiting();
        }

        public override void Draw()
        {
            Vector2 offSet = Position;
            int counter = 0;

            for (int i = 0; i < satalliteList.Count; i++)
            {
                Globals.SpriteBatch.Draw(lgDebriBoxTexture, new Vector2(Position.X + (lgDebriBoxTexture.Width * (i+1)), Position.Y), Color.White);
            }

            for (int i = 0; i < satalliteList.Count; i++)
            {
                var loc = new Vector2(Position.X + (lgDebriBoxTexture.Width * (counter + 1)), Position.Y);
                Globals.SpriteBatch.Draw(satalliteList[i].Texture, loc, satalliteList[i].SrcRectangle, Color.White);
                //Globals.SpriteBatch.Draw(satalliteList[i].Texture, new Vector2(Position.X + (lgDebriBoxTexture.Width * (counter + 1)), Position.Y), Color.White);
                counter++;
            }

            //counter = 0;
            //offSet.Y += smDebriBoxTexture.Height + 5;
            //for (int i = 0; i < maxNormal; i++)
            //{
            //    Globals.SpriteBatch.Draw(smDebriBoxTexture, new Vector2(Position.X + (smDebriBoxTexture.Width * (i + 1)), Position.Y + lgDebriBoxTexture.Height), Color.White);
            //}

            //for (int i = 0; i < satalliteList.Count; i++)
            //{
            //    if (!satalliteList[i].isSpecial)
            //    {
            //        Globals.SpriteBatch.Draw(satalliteList[i].Texture, new Vector2(Position.X + (smDebriBoxTexture.Width * (counter + 1)), Position.Y + lgDebriBoxTexture.Height), Color.White);
            //        counter++;
            //    }
            //}            

        }

    }
}
