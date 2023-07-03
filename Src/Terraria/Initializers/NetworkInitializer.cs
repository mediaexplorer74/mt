/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using GameManager.GameContent.NetModules;
using GameManager.Net;

namespace GameManager.Initializers
{
    internal static class NetworkInitializer
    {
        public static void Load()
        {
            NetManager.Instance.Register<NetLiquidModule>();
        }
    }
}
