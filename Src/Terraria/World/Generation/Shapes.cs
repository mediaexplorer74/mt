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
using System;
using GameManager;

namespace GameManager.World.Generation
{
    internal static class Shapes
    {
        public class Circle : GenShape
        {
            private int _verticalRadius;
            private int _horizontalRadius;

            public Circle(int radius)
            {
                this._verticalRadius = radius;
                this._horizontalRadius = radius;
            }

            public Circle(int horizontalRadius, int verticalRadius)
            {
                this._horizontalRadius = horizontalRadius;
                this._verticalRadius = verticalRadius;
            }

            public override bool Perform(Point origin, GenAction action)
            {
                int num1 = (this._horizontalRadius + 1) * (this._horizontalRadius + 1);
                for (int y = origin.Y - this._verticalRadius; y <= origin.Y + this._verticalRadius; ++y)
                {
                    float num2 = (float)this._horizontalRadius / (float)this._verticalRadius * (float)(y - origin.Y);
                    int num3 = Math.Min(this._horizontalRadius, (int)Math.Sqrt((double)num1 - (double)num2 * (double)num2));
                    for (int x = origin.X - num3; x <= origin.X + num3; ++x)
                    {
                        if (!this.UnitApply(action, origin, x, y) && this._quitOnFail)
                            return false;
                    }
                }
                return true;
            }
        }

        public class HalfCircle : GenShape
        {
            private int _radius;

            public HalfCircle(int radius)
            {
                this._radius = radius;
            }

            public override bool Perform(Point origin, GenAction action)
            {
                int num1 = (this._radius + 1) * (this._radius + 1);
                for (int y = origin.Y - this._radius; y <= origin.Y; ++y)
                {
                    int num2 = Math.Min(this._radius, (int)Math.Sqrt((double)(num1 - (y - origin.Y) * (y - origin.Y))));
                    for (int x = origin.X - num2; x <= origin.X + num2; ++x)
                    {
                        if (!this.UnitApply(action, origin, x, y) && this._quitOnFail)
                            return false;
                    }
                }
                return true;
            }
        }

        public class Slime : GenShape
        {
            private int _radius;
            private float _xScale;
            private float _yScale;

            public Slime(int radius)
            {
                this._radius = radius;
                this._xScale = 1f;
                this._yScale = 1f;
            }

            public Slime(int radius, float xScale, float yScale)
            {
                this._radius = radius;
                this._xScale = xScale;
                this._yScale = yScale;
            }

            public override bool Perform(Point origin, GenAction action)
            {
                float num1 = (float)this._radius;
                int num2 = (this._radius + 1) * (this._radius + 1);
                for (int y = origin.Y - (int)((double)num1 * (double)this._yScale); y <= origin.Y; ++y)
                {
                    float num3 = (float)(y - origin.Y) / this._yScale;
                    int num4 = (int)Math.Min((float)this._radius * this._xScale, this._xScale * (float)Math.Sqrt((double)num2 - (double)num3 * (double)num3));
                    for (int x = origin.X - num4; x <= origin.X + num4; ++x)
                    {
                        if (!this.UnitApply(action, origin, x, y) && this._quitOnFail)
                            return false;
                    }
                }
                for (int y = origin.Y + 1; y <= origin.Y + (int)((double)num1 * (double)this._yScale * 0.5) - 1; ++y)
                {
                    float num3 = (float)(y - origin.Y) * (2f / this._yScale);
                    int num4 = (int)Math.Min((float)this._radius * this._xScale, this._xScale * (float)Math.Sqrt((double)num2 - (double)num3 * (double)num3));
                    for (int x = origin.X - num4; x <= origin.X + num4; ++x)
                    {
                        if (!this.UnitApply(action, origin, x, y) && this._quitOnFail)
                            return false;
                    }
                }
                return true;
            }
        }

        public class Rectangle : GenShape
        {
            private int _width;
            private int _height;

            public Rectangle(int width, int height)
            {
                this._width = width;
                this._height = height;
            }

            public override bool Perform(Point origin, GenAction action)
            {
                for (int x = origin.X; x < origin.X + this._width; ++x)
                {
                    for (int y = origin.Y; y < origin.Y + this._height; ++y)
                    {
                        if (!this.UnitApply(action, origin, x, y) && this._quitOnFail)
                            return false;
                    }
                }
                return true;
            }
        }

        public class Tail : GenShape
        {
            private float _width;
            private Vector2 _endOffset;

            public Tail(float width, Vector2 endOffset)
            {
                this._width = width * 16f;
                this._endOffset = endOffset * 16f;
            }

            public override bool Perform(Point origin, GenAction action)
            {
                Vector2 start = new Vector2((float)(origin.X << 4), (float)(origin.Y << 4));
                return Utils.PlotTileTale(start, start + this._endOffset, this._width, (Utils.PerLinePoint)((x, y) =>
                {
                    if (!this.UnitApply(action, origin, x, y))
                        return !this._quitOnFail;
                    return true;
                }));
            }
        }

        public class Mound : GenShape
        {
            private int _halfWidth;
            private int _height;

            public Mound(int halfWidth, int height)
            {
                this._halfWidth = halfWidth;
                this._height = height;
            }

            public override bool Perform(Point origin, GenAction action)
            {
                float num1 = (float)this._halfWidth;
                for (int index1 = -this._halfWidth; index1 <= this._halfWidth; ++index1)
                {
                    int num2 = Math.Min(this._height, (int)(-((double)(this._height + 1) / ((double)num1 * (double)num1)) * ((double)index1 + (double)num1) * ((double)index1 - (double)num1)));
                    for (int index2 = 0; index2 < num2; ++index2)
                    {
                        if (!this.UnitApply(action, origin, index1 + origin.X, origin.Y - index2) && this._quitOnFail)
                            return false;
                    }
                }
                return true;
            }
        }
    }
}
