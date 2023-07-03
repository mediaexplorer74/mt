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
    internal class SimpleStructure : GenStructure
    {
        private int[,] _data;
        private int _width;
        private int _height;
        private GenAction[] _actions;
        private bool _xMirror;
        private bool _yMirror;

        public int Width
        {
            get
            {
                return this._width;
            }
        }

        public int Height
        {
            get
            {
                return this._height;
            }
        }

        public SimpleStructure(params string[] data)
        {
            this.ReadData(data);
        }

        public SimpleStructure(string data)
        {
            this.ReadData(data.Split('\n'));
        }

        private void ReadData(string[] lines)
        {
            this._height = lines.Length;
            this._width = lines[0].Length;
            this._data = new int[this._width, this._height];
            for (int index1 = 0; index1 < this._height; ++index1)
            {
                for (int index2 = 0; index2 < this._width; ++index2)
                {
                    int num = (int)lines[index1][index2];
                    this._data[index2, index1] = num < 48 || num > 57 ? -1 : num - 48;
                }
            }
        }

        public SimpleStructure SetActions(params GenAction[] actions)
        {
            this._actions = actions;
            return this;
        }

        public SimpleStructure Mirror(bool horizontalMirror, bool verticalMirror)
        {
            this._xMirror = horizontalMirror;
            this._yMirror = verticalMirror;
            return this;
        }

        public override bool Place(Point origin, StructureMap structures)
        {
            if (!structures.CanPlace(new Microsoft.Xna.Framework.Rectangle(origin.X, origin.Y, this._width, this._height), 0))
                return false;
            for (int index1 = 0; index1 < this._width; ++index1)
            {
                for (int index2 = 0; index2 < this._height; ++index2)
                {
                    int num1 = this._xMirror ? -index1 : index1;
                    int num2 = this._yMirror ? -index2 : index2;
                    if (this._data[index1, index2] != -1 && !this._actions[this._data[index1, index2]].Apply(origin, num1 + origin.X, num2 + origin.Y))
                        return false;
                }
            }
            structures.AddStructure(new Microsoft.Xna.Framework.Rectangle(origin.X, origin.Y, this._width, this._height), 0);
            return true;
        }
    }
}
