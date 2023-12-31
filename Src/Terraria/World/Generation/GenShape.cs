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

namespace GameManager.World.Generation
{
    internal abstract class GenShape : GenBase
    {
        private ShapeData _outputData;
        protected bool _quitOnFail;

        public abstract bool Perform(Point origin, GenAction action);

        protected bool UnitApply(GenAction action, Point origin, int x, int y, params object[] args)
        {
            if (this._outputData != null)
                this._outputData.Add(x - origin.X, y - origin.Y);
            return action.Apply(origin, x, y, args);
        }

        public GenShape Output(ShapeData outputData)
        {
            this._outputData = outputData;
            return this;
        }

        public GenShape QuitOnFail(bool value = true)
        {
            this._quitOnFail = value;
            return this;
        }
    }
}
