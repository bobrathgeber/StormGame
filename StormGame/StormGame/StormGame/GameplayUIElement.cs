using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace StormGame
{
    abstract class GameplayUIElement
    {
        public Vector2 Position;
        public virtual void Update() { }
        public virtual void Draw(){}
    }
}
