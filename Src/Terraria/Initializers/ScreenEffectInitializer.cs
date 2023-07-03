﻿/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using GameManager.GameContent.Shaders;
using GameManager.GameContent.Skies;
using GameManager.Graphics.Effects;
using GameManager.Graphics.Shaders;

namespace GameManager.Initializers
{
    internal static class ScreenEffectInitializer
    {
        public static void Load()
        {
            Filters.Scene["Nebula"] = new Filter(new ScreenShaderData("FilterTower").UseColor(1f, 0.0f, 0.9f).UseOpacity(0.5f), EffectPriority.High);
            Filters.Scene["Solar"] = new Filter(new ScreenShaderData("FilterTower").UseColor(1f, 0.7f, 0.0f).UseOpacity(0.3f), EffectPriority.High);
            Filters.Scene["Stardust"] = new Filter(new ScreenShaderData("FilterTower").UseColor(0.0f, 0.5f, 1f).UseOpacity(0.5f), EffectPriority.High);
            Filters.Scene["Vortex"] = new Filter(new ScreenShaderData("FilterTower").UseColor(0.0f, 0.7f, 0.7f).UseOpacity(0.5f), EffectPriority.High);
            Filters.Scene["MonolithNebula"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(1f, 0.0f, 0.9f).UseOpacity(0.5f), EffectPriority.Medium);
            Filters.Scene["MonolithSolar"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(1f, 0.7f, 0.0f).UseOpacity(0.3f), EffectPriority.Medium);
            Filters.Scene["MonolithStardust"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0.0f, 0.5f, 1f).UseOpacity(0.5f), EffectPriority.Medium);
            Filters.Scene["MonolithVortex"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0.0f, 0.7f, 0.7f).UseOpacity(0.5f), EffectPriority.Medium);
            Filters.Scene["MoonLord"] = new Filter((ScreenShaderData)new MoonLordScreenShaderData("FilterMoonLord"), EffectPriority.VeryHigh);
            Filters.Scene["MoonLordShake"] = new Filter((ScreenShaderData)new MoonLordScreenShaderData("FilterMoonLordShake"), EffectPriority.VeryHigh);
            Filters.Scene["testInvert"] = new Filter(new ScreenShaderData("FilterInvert"), EffectPriority.VeryHigh);
            Filters.Scene["BloodMoon"] = new Filter(new BloodMoonScreenShaderData("FilterBloodMoon").UseColor(2f, -0.8f, -0.6f), EffectPriority.Medium);
            Overlays.Scene.Load();
            Filters.Scene.Load();
            ScreenEffectInitializer.LoadSkies();
        }

        private static void LoadSkies()
        {
            SkyManager.Instance["Martian"] = new MartianSky();
            SkyManager.Instance["Nebula"] = new NebulaSky();
            SkyManager.Instance["Stardust"] = new StardustSky();
            SkyManager.Instance["Vortex"] = new VortexSky();
            SkyManager.Instance["Solar"] = new SolarSky();
            SkyManager.Instance["Slime"] = new SlimeSky();
            SkyManager.Instance["MoonLord"] = new MoonLordSky();
            SkyManager.Instance["MonolithNebula"] = new NebulaSky();
            SkyManager.Instance["MonolithStardust"] = new StardustSky();
            SkyManager.Instance["MonolithVortex"] = new VortexSky();
            SkyManager.Instance["MonolithSolar"] = new SolarSky();
            SkyManager.Instance.Load();
        }
    }
}
