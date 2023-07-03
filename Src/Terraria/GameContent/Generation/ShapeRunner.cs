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
using GameManager.World.Generation;

namespace GameManager.GameContent.Generation
{
    internal class ShapeRunner : GenShape
    {
        private float _startStrength;
        private int _steps;
        private Vector2 _startVelocity;

        public ShapeRunner(float strength, int steps, Vector2 velocity)
        {
            _startStrength = strength;
            _steps = steps;
            _startVelocity = velocity;
        }

        public override bool Perform(Point origin, GenAction action)
        {
            float num1 = (float)_steps;
            float num2 = (float)_steps;
            double num3 = (double)_startStrength;
            Vector2 vector2_1 = new Vector2((float)origin.X, (float)origin.Y);
            Vector2 vector2_2 = _startVelocity == Vector2.Zero ? Utils.RandomVector2(GenBase._random, -1f, 1f) : _startVelocity;

            while (num1 > 0.0 && num3 > 0.0)
            {
                num3 = _startStrength * (num1 / num2);
                float num4 = num1 - 1f;
                int num5 = Math.Max(1, (int)(vector2_1.X - num3 * 0.5));
                int num6 = Math.Max(1, (int)(vector2_1.Y - num3 * 0.5));
                int num7 = Math.Min(GenBase._worldWidth, (int)(vector2_1.X + num3 * 0.5));
                int num8 = Math.Min(GenBase._worldHeight, (int)(vector2_1.Y + num3 * 0.5));
                for (int x = num5; x < num7; ++x)
                {
                    for (int y = num6; y < num8; ++y)
                    {
                        if ((double)Math.Abs((float)x - vector2_1.X) + Math.Abs((float)y - vector2_1.Y) < num3 * 0.5 * (1.0 + GenBase._random.Next(-10, 11) * 0.015))
                            UnitApply(action, origin, x, y);
                    }
                }

                int num9 = (int)(num3 / 50.0) + 1;
                num1 = num4 - (float)num9;
                vector2_1 += vector2_2;
                for (int index = 0; index < num9; ++index)
                {
                    vector2_1 += vector2_2;
                    vector2_2 += Utils.RandomVector2(GenBase._random, -0.5f, 0.5f);
                }

                vector2_2 = Vector2.Clamp(vector2_2 + Utils.RandomVector2(GenBase._random, -0.5f, 0.5f), -Vector2.One, Vector2.One);
            }
            return true;
        }
    }
}
