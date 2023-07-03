﻿/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using Microsoft.Xna.Framework;

namespace GameManager.UI
{
    public class UIScrollWheelEvent : UIMouseEvent
    {
        public readonly int ScrollWheelValue;

        public UIScrollWheelEvent(UIElement target, Vector2 mousePosition, int scrollWheelValue)
            : base(target, mousePosition)
        {
            ScrollWheelValue = scrollWheelValue;
        }
    }
}
