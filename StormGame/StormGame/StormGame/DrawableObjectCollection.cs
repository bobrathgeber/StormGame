using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StormGame
{
    public class DrawableObjectCollection: List<DrawableObject>
    {
        List<DrawableObject> removeQueue = new List<DrawableObject>();
        public List<T> CopyFilter<T>() where T : DrawableObject
        {
            return this.OfType<T>().ToList<T>();
        }

        public void RemoveFlaggedObjects()
        {
            for(int i = Count-1; i>=0; i--)
            {
                if (this[i].readyToRemove)
                    RemoveAt(i);
            }
        }

        public void SortList()
        {
            this.Sort(delegate(DrawableObject a, DrawableObject b)
            {
                int xdiff = a.Depth.CompareTo(b.Depth);
                if (xdiff != 0) return xdiff;
                else return a.Depth.CompareTo(b.Depth);
            });
        }
    }
}
