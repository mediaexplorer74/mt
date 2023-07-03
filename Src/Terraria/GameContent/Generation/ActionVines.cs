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
using GameManager.World.Generation;

namespace GameManager.GameContent.Generation
{
    internal class ActionVines : GenAction
    {
        private int _minLength;
        private int _maxLength;
        private int _vineId;

        public ActionVines(int minLength = 6, int maxLength = 10, int vineId = 52)
        {
            _minLength = minLength;
            _maxLength = maxLength;
            _vineId = vineId;
        }

        public override bool Apply(Point origin, int x, int y, params object[] args)
        {
            int num1 = GenBase._random.Next(_minLength, _maxLength + 1);
            int num2;
            for (num2 = 0; num2 < num1 && !GenBase._tiles[x, y + num2].active(); ++num2)
            {
                GenBase._tiles[x, y + num2].type = (ushort)_vineId;
                GenBase._tiles[x, y + num2].active(true);
            }

            if (num2 > 0)
                return UnitApply(origin, x, y, args);
            return false;
        }
    }
}
