/*
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
    internal class MoonLordScreenShaderData : ScreenShaderData
    {
        private int _moonLordIndex = -1;

        public MoonLordScreenShaderData(string passName)
            : base(passName) { }

        private void UpdateMoonLordIndex()
        {
            if (_moonLordIndex >= 0 && Game1.npc[_moonLordIndex].active && Game1.npc[_moonLordIndex].type == 398)
                return;

            int num = -1;
            for (int index = 0; index < Game1.npc.Length; ++index)
            {
                if (Game1.npc[index].active && Game1.npc[index].type == 398)
                {
                    num = index;
                    break;
                }
            }
            _moonLordIndex = num;
        }

        public override void Apply()
        {
            UpdateMoonLordIndex();
            if (_moonLordIndex != -1)
                UseTargetPosition(Game1.npc[_moonLordIndex].Center);
            base.Apply();
        }
    }
}
