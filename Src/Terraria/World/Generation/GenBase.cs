/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using System;
using GameManager;

namespace GameManager.World.Generation
{
    public class GenBase
    {
        protected static Random _random
        {
            get
            {
                return WorldGen.genRand;
            }
        }

        protected static Tile[,] _tiles
        {
            get
            {
                return Game1.tile;
            }
        }

        protected static int _worldWidth
        {
            get
            {
                return Game1.maxTilesX;
            }
        }

        protected static int _worldHeight
        {
            get
            {
                return Game1.maxTilesY;
            }
        }

        public delegate bool CustomPerUnitAction(int x, int y, params object[] args);
    }
}
