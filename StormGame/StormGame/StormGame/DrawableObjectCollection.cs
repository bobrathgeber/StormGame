using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StormGame
{
    class DrawableObjectCollection: List<DrawableObject>
    {
        List<DrawableObject> removeQueue = new List<DrawableObject>();
        public List<T> CopyFilter<T>() where T : DrawableObject
        {
            return this.OfType<T>().ToList<T>();
        }

        public void QueueRemoveObject(DrawableObject d)
        {
            removeQueue.Add(d);
        }

        public void QueueAddObject()
        {

        }

        public void ResolveQueue()
        {
            
        }
    }
}
