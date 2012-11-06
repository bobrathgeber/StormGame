using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StormGame
{
    class PopupWindowList :List<PopupWindow>
    {
        //private List<PopupWindow> 
        public PopupWindow GetCurrentOrDefault()
        {
            if (HasItems())
                return this.First();
            else
                return new NullPopupWindow("I AM ERROR, aka null");
        }
        public bool HasItems()
        {
            return this.Count > 0;

        }

        //public override void Add(PopupWindow popup)
        //{

        //    this.Add(popup);
        //}
    }
}
