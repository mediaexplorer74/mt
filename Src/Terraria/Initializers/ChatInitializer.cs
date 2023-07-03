﻿/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using GameManager.GameContent.UI.Chat;
using GameManager.UI.Chat;

namespace GameManager.Initializers
{
    internal static class ChatInitializer
    {
        public static void Load()
        {
            ChatManager.Register<ColorTagHandler>("c", "color");
            ChatManager.Register<ItemTagHandler>("i", "item");
            ChatManager.Register<NameTagHandler>("n", "name");
            ChatManager.Register<AchievementTagHandler>("a", "achievement");
        }
    }
}
