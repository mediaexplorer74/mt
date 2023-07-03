﻿/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using Microsoft.Xna.Framework.Graphics;
using GameManager;
using GameManager.DataStructures;
using GameManager.Graphics.Shaders;

namespace GameManager.GameContent.Dyes
{
    internal class TeamArmorShaderData : ArmorShaderData
    {
        private static bool isInitialized = false;
        private static ArmorShaderData[] dustShaderData;

        public TeamArmorShaderData(Effect shader, string passName)
            : base(shader, passName)
        {
            if (isInitialized)
                return;

            isInitialized = true;
            dustShaderData = new ArmorShaderData[Main.teamColor.Length];
            for (int index = 1; index < Main.teamColor.Length; ++index)
                dustShaderData[index] = new ArmorShaderData(shader, passName).UseColor(Main.teamColor[index]);
            dustShaderData[0] = new ArmorShaderData(shader, "Default");
        }

        public override void Apply(Entity entity, DrawData? drawData)
        {
            Player player = entity as Player;
            if (player == null || player.team == 0)
                dustShaderData[0].Apply(player, drawData);
            else
            {
                this.UseColor(Main.teamColor[player.team]);
                base.Apply(player, drawData);
            }
        }

        public override ArmorShaderData GetSecondaryShader(Entity entity)
        {
            Player player = entity as Player;
            return dustShaderData[player.team];
        }
    }
}
