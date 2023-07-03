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
using Microsoft.Xna.Framework.Graphics;
using System;
using GameManager.Graphics;
using GameManager.UI;

namespace GameManager.GameContent.UI.Elements
{
    internal class UIPanel : UIElement
    {
        public Color BorderColor = Color.Black;
        public Color BackgroundColor = new Color(63, 82, 151) * 0.7f;
        private const int CORNER_SIZE = 12;
        private const int TEXTURE_PADDING = 3;
        private const int BAR_SIZE = 2;
        private static Texture2D _borderTexture;
        private static Texture2D _backgroundTexture;

        public UIPanel()
        {
            if (_borderTexture == null)
                _borderTexture = TextureManager.Load("Images/UI/PanelBorder");
            if (_backgroundTexture == null)
                _backgroundTexture = TextureManager.Load("Images/UI/PanelBackground");
            SetPadding(12f);
        }

        private void DrawPanel(SpriteBatch spriteBatch, Texture2D texture, Color color)
        {
            CalculatedStyle dimensions = GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
            Point point2 = new Point((int)(dimensions.X + dimensions.Width) - 12, (int)(dimensions.Y + dimensions.Height) - 12);
            int width = (int)Math.Ceiling(dimensions.Width) - 24;
            int height = (int)Math.Ceiling(dimensions.Height) - 24;
            spriteBatch.Draw(texture, new Rectangle(point1.X, point1.Y, 12, 12), new Rectangle?(new Rectangle(0, 0, 12, 12)), color);
            spriteBatch.Draw(texture, new Rectangle(point2.X, point1.Y, 12, 12), new Rectangle?(new Rectangle(20, 0, 12, 12)), color);
            spriteBatch.Draw(texture, new Rectangle(point1.X, point2.Y, 12, 12), new Rectangle?(new Rectangle(0, 20, 12, 12)), color);
            spriteBatch.Draw(texture, new Rectangle(point2.X, point2.Y, 12, 12), new Rectangle?(new Rectangle(20, 20, 12, 12)), color);
            spriteBatch.Draw(texture, new Rectangle(point1.X + 12, point1.Y, width, 12), new Rectangle?(new Rectangle(15, 0, 2, 12)), color);
            spriteBatch.Draw(texture, new Rectangle(point1.X + 12, point2.Y, width, 12), new Rectangle?(new Rectangle(15, 20, 2, 12)), color);
            spriteBatch.Draw(texture, new Rectangle(point1.X, point1.Y + 12, 12, height), new Rectangle?(new Rectangle(0, 15, 12, 2)), color);
            spriteBatch.Draw(texture, new Rectangle(point2.X, point1.Y + 12, 12, height), new Rectangle?(new Rectangle(20, 15, 12, 2)), color);
            spriteBatch.Draw(texture, new Rectangle(point1.X + 12, point1.Y + 12, width, height), new Rectangle?(new Rectangle(15, 15, 2, 2)), color);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            DrawPanel(spriteBatch, _backgroundTexture, BackgroundColor);
            DrawPanel(spriteBatch, _borderTexture, BorderColor);
        }
    }
}
