using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StormGame
{
    class NullPopupWindow :PopupWindow
    {
        public NullPopupWindow(string s) : base(s)
        {
        }

        public override void Draw()
        {
            //Do nothing.
        }
    }
}
