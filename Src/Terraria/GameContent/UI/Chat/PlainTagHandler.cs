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
using GameManager.UI.Chat;

namespace GameManager.GameContent.UI.Chat
{
    public class PlainTagHandler : ITagHandler
    {
        TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
        {
            return new PlainSnippet(text);
        }

        public class PlainSnippet : TextSnippet
        {
            public PlainSnippet(string text = "")
                : base(text) { }

            public PlainSnippet(string text, Color color, float scale = 1f)
                : base(text, color, scale) { }

            public override Color GetVisibleColor()
            {
                return Color;
            }
        }
    }
}
