﻿/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using GameManager;
using GameManager.Graphics.Shaders;

namespace GameManager.GameContent.Shaders
{
    internal class BloodMoonScreenShaderData : ScreenShaderData
    {
        public BloodMoonScreenShaderData(string passName)
            : base(passName) { }

        public override void Apply()
        {
            UseOpacity((1f - Utils.SmoothStep((float)Game1.worldSurface + 50f, (float)Game1.rockLayer + 100f, (float)((Game1.screenPosition.Y + (Game1.screenHeight / 2)) / 16.0))) * 0.75f);
            base.Apply();
        }
    }
}
