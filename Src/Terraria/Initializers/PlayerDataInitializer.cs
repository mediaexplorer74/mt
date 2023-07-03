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
using GameManager.Graphics;

namespace GameManager.Initializers
{
    internal static class PlayerDataInitializer
    {
        public static void Load()
        {
            Main.playerTextures = new Texture2D[8, 15];
            PlayerDataInitializer.LoadStarterMale();
            PlayerDataInitializer.LoadStarterFemale();
            PlayerDataInitializer.LoadStickerMale();
            PlayerDataInitializer.LoadStickerFemale();
            PlayerDataInitializer.LoadGangsterMale();
            PlayerDataInitializer.LoadGangsterFemale();
            PlayerDataInitializer.LoadFancyMale();
            PlayerDataInitializer.LoadFancyFemale();
        }

        private static void LoadDebugs()
        {
            PlayerDataInitializer.CopyVariant(8, 0);
            PlayerDataInitializer.CopyVariant(9, 4);
            for (int index = 8; index < 10; ++index)
            {
                Main.playerTextures[index, 4] = Main.armorArmTexture[191];
                Main.playerTextures[index, 6] = Main.armorArmTexture[191];
                Main.playerTextures[index, 11] = Main.armorArmTexture[191];
                Main.playerTextures[index, 12] = Main.armorArmTexture[191];
                Main.playerTextures[index, 13] = Main.armorArmTexture[191];
                Main.playerTextures[index, 8] = Main.armorArmTexture[191];
            }
        }

        private static void LoadVariant(int ID, int[] pieceIDs)
        {
            for (int index = 0; index < pieceIDs.Length; ++index)
                Main.playerTextures[ID, pieceIDs[index]] = TextureManager.Load(string.Concat(new object[4]
        {
          (object) "Images/Player_",
          (object) ID,
          (object) "_",
          (object) pieceIDs[index]
        }));
        }

        private static void CopyVariant(int to, int from)
        {
            for (int index = 0; index < 15; ++index)
                Main.playerTextures[to, index] = Main.playerTextures[from, index];
        }

        private static void LoadStarterMale()
        {
            PlayerDataInitializer.LoadVariant(0, new int[14]
      {
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        8,
        9,
        10,
        11,
        12,
        13
      });
            Main.playerTextures[0, 14] = TextureManager.BlankTexture;
        }

        private static void LoadStickerMale()
        {
            PlayerDataInitializer.CopyVariant(1, 0);
            PlayerDataInitializer.LoadVariant(1, new int[6]
      {
        4,
        6,
        8,
        11,
        12,
        13
      });
        }

        private static void LoadGangsterMale()
        {
            PlayerDataInitializer.CopyVariant(2, 0);
            PlayerDataInitializer.LoadVariant(2, new int[6]
      {
        4,
        6,
        8,
        11,
        12,
        13
      });
        }

        private static void LoadFancyMale()
        {
            PlayerDataInitializer.CopyVariant(3, 0);
            PlayerDataInitializer.LoadVariant(3, new int[7]
      {
        4,
        6,
        8,
        11,
        12,
        13,
        14
      });
        }

        private static void LoadStarterFemale()
        {
            PlayerDataInitializer.CopyVariant(4, 0);
            PlayerDataInitializer.LoadVariant(4, new int[11]
      {
        3,
        4,
        5,
        6,
        7,
        8,
        9,
        10,
        11,
        12,
        13
      });
        }

        private static void LoadStickerFemale()
        {
            PlayerDataInitializer.CopyVariant(5, 4);
            PlayerDataInitializer.LoadVariant(5, new int[6]
      {
        4,
        6,
        8,
        11,
        12,
        13
      });
        }

        private static void LoadGangsterFemale()
        {
            PlayerDataInitializer.CopyVariant(6, 4);
            PlayerDataInitializer.LoadVariant(6, new int[6]
      {
        4,
        6,
        8,
        11,
        12,
        13
      });
        }

        private static void LoadFancyFemale()
        {
            PlayerDataInitializer.CopyVariant(7, 4);
            PlayerDataInitializer.LoadVariant(7, new int[6]
      {
        4,
        6,
        8,
        11,
        12,
        13
      });
        }
    }
}
