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
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GameManager;
using GameManager.GameContent.UI.Chat;

namespace GameManager.UI.Chat
{
    public static class ChatManager
    {
        private static ConcurrentDictionary<string, ITagHandler> _handlers = new ConcurrentDictionary<string, ITagHandler>();
        public static readonly Vector2[] ShadowDirections = new Vector2[4]
    {
      -Vector2.UnitX,
      Vector2.UnitX,
      -Vector2.UnitY,
      Vector2.UnitY
    };

        public static Color WaveColor(Color color)
        {
            float num = (float)Game1.mouseTextColor / (float)byte.MaxValue;
            color = Color.Lerp(color, Color.Black, 1f - num);
            color.A = Game1.mouseTextColor;
            return color;
        }

        public static void ConvertNormalSnippets(TextSnippet[] snippets)
        {
            for (int index = 0; index < snippets.Length; ++index)
            {
                TextSnippet textSnippet = snippets[index];
                if (snippets[index].GetType() == typeof(TextSnippet))
                {
                    PlainTagHandler.PlainSnippet plainSnippet = new PlainTagHandler.PlainSnippet(textSnippet.Text, textSnippet.Color, textSnippet.Scale);
                    snippets[index] = (TextSnippet)plainSnippet;
                }
            }
        }

        public static void Register<T>(params string[] names) where T : ITagHandler, new()
        {
            T obj = new T();
            for (int index = 0; index < names.Length; ++index)
                ChatManager._handlers[names[index].ToLower()] = (ITagHandler)obj;
        }

        private static ITagHandler GetHandler(string tagName)
        {
            string key = tagName.ToLower();
            if (ChatManager._handlers.ContainsKey(key))
                return ChatManager._handlers[key];
            return (ITagHandler)null;
        }

        public static TextSnippet[] ParseMessage(string text, Color baseColor)
        {
            MatchCollection matchCollection = ChatManager.Regexes.Format.Matches(text);
            List<TextSnippet> list = new List<TextSnippet>();
            int startIndex = 0;
            foreach (Match match in matchCollection)
            {
                if (match.Index > startIndex)
                    list.Add(new TextSnippet(text.Substring(startIndex, match.Index - startIndex)));
                startIndex = match.Index + match.Length;
                string tagName = match.Groups["tag"].Value;
                string text1 = match.Groups["text"].Value;
                string options = match.Groups["options"].Value;
                ITagHandler handler = ChatManager.GetHandler(tagName);
                if (handler != null)
                {
                    list.Add(handler.Parse(text1, baseColor, options));
                    list[list.Count - 1].TextOriginal = match.ToString();
                }
                else
                    list.Add(new TextSnippet(text1, baseColor, 1f));
            }
            if (text.Length > startIndex)
                list.Add(new TextSnippet(text.Substring(startIndex, text.Length - startIndex), baseColor, 1f));
            return list.ToArray();
        }

        public static bool AddChatText(SpriteFont font, string text, Vector2 baseScale)
        {
            int num = Game1.screenWidth - 330;
            if ((double)ChatManager.GetStringSize(font, Game1.chatText + text, baseScale, -1f).X > (double)num)
                return false;
            Game1.chatText += text;
            return true;
        }

        public static Vector2 GetStringSize(SpriteFont font, string text, Vector2 baseScale, float maxWidth = -1f)
        {
            TextSnippet[] snippets = ChatManager.ParseMessage(text, Color.White);
            return ChatManager.GetStringSize(font, snippets, baseScale, maxWidth);
        }

        public static Vector2 GetStringSize(SpriteFont font, TextSnippet[] snippets, Vector2 baseScale, float maxWidth = -1f)
        {
            Vector2 vec = new Vector2((float)Game1.mouseX, (float)Game1.mouseY);
            Vector2 zero = Vector2.Zero;
            Vector2 minimum = zero;
            Vector2 vector2_1 = minimum;
            float num1 = font.MeasureString(" ").X;
            float num2 = 0.0f;
            for (int index1 = 0; index1 < snippets.Length; ++index1)
            {
                TextSnippet textSnippet = snippets[index1];
                textSnippet.Update();
                float num3 = textSnippet.Scale;
                Vector2 size;
                if (textSnippet.UniqueDraw(true, out size, (SpriteBatch)null, new Vector2(), new Color(), 1f))
                {
                    minimum.X += size.X * baseScale.X * num3;
                    vector2_1.X = Math.Max(vector2_1.X, minimum.X);
                    vector2_1.Y = Math.Max(vector2_1.Y, minimum.Y + size.Y);
                }
                else
                {
                    string[] strArray1 = textSnippet.Text.Split('\n');
                    foreach (string str in strArray1)
                    {
                        char[] chArray = new char[1]
            {
              ' '
            };
                        string[] strArray2 = str.Split(chArray);
                        for (int index2 = 0; index2 < strArray2.Length; ++index2)
                        {
                            if (index2 != 0)
                                minimum.X += num1 * baseScale.X * num3;
                            if ((double)maxWidth > 0.0)
                            {
                                float num4 = font.MeasureString(strArray2[index2]).X * baseScale.X * num3;
                                if ((double)minimum.X - (double)zero.X + (double)num4 > (double)maxWidth)
                                {
                                    minimum.X = zero.X;
                                    minimum.Y += (float)font.LineSpacing * num2 * baseScale.Y;
                                    vector2_1.Y = Math.Max(vector2_1.Y, minimum.Y);
                                    num2 = 0.0f;
                                }
                            }
                            if ((double)num2 < (double)num3)
                                num2 = num3;
                            Vector2 vector2_2 = font.MeasureString(strArray2[index2]);
                            Utils.Between(vec, minimum, minimum + vector2_2);
                            minimum.X += vector2_2.X * baseScale.X * num3;
                            vector2_1.X = Math.Max(vector2_1.X, minimum.X);
                            vector2_1.Y = Math.Max(vector2_1.Y, minimum.Y + vector2_2.Y);
                        }
                        if (strArray1.Length > 1)
                        {
                            minimum.X = zero.X;
                            minimum.Y += (float)font.LineSpacing * num2 * baseScale.Y;
                            vector2_1.Y = Math.Max(vector2_1.Y, minimum.Y);
                            num2 = 0.0f;
                        }
                    }
                }
            }
            return vector2_1;
        }

        public static void DrawColorCodedStringShadow(SpriteBatch spriteBatch, SpriteFont font, TextSnippet[] snippets, Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 baseScale, float maxWidth = -1f, float spread = 2f)
        {
            for (int index = 0; index < ChatManager.ShadowDirections.Length; ++index)
            {
                int hoveredSnippet;
                ChatManager.DrawColorCodedString(spriteBatch, font, snippets, position + ChatManager.ShadowDirections[index] * spread, baseColor, rotation, origin, baseScale, out hoveredSnippet, maxWidth, true);
            }
        }

        public static Vector2 DrawColorCodedString(SpriteBatch spriteBatch, SpriteFont font, TextSnippet[] snippets, Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 baseScale, out int hoveredSnippet, float maxWidth, bool ignoreColors = false)
        {
            int num1 = -1;
            Vector2 vec = new Vector2((float)Game1.mouseX, (float)Game1.mouseY);
            Vector2 vector2_1 = position;
            Vector2 vector2_2 = vector2_1;
            float num2 = font.MeasureString(" ").X;
            Color color = baseColor;
            float num3 = 0.0f;
            for (int index1 = 0; index1 < snippets.Length; ++index1)
            {
                TextSnippet textSnippet = snippets[index1];
                textSnippet.Update();
                if (!ignoreColors)
                    color = textSnippet.GetVisibleColor();
                float scale = textSnippet.Scale;
                Vector2 size;
                if (textSnippet.UniqueDraw(false, out size, spriteBatch, vector2_1, color, scale))
                {
                    if (Utils.Between(vec, vector2_1, vector2_1 + size))
                        num1 = index1;
                    vector2_1.X += size.X * baseScale.X * scale;
                    vector2_2.X = Math.Max(vector2_2.X, vector2_1.X);
                }
                else
                {
                    string[] strArray1 = textSnippet.Text.Split('\n');
                    foreach (string str in strArray1)
                    {
                        char[] chArray = new char[1]
            {
              ' '
            };
                        string[] strArray2 = str.Split(chArray);
                        for (int index2 = 0; index2 < strArray2.Length; ++index2)
                        {
                            if (index2 != 0)
                                vector2_1.X += num2 * baseScale.X * scale;
                            if ((double)maxWidth > 0.0)
                            {
                                float num4 = font.MeasureString(strArray2[index2]).X * baseScale.X * scale;
                                if ((double)vector2_1.X - (double)position.X + (double)num4 > (double)maxWidth)
                                {
                                    vector2_1.X = position.X;
                                    vector2_1.Y += (float)font.LineSpacing * num3 * baseScale.Y;
                                    vector2_2.Y = Math.Max(vector2_2.Y, vector2_1.Y);
                                    num3 = 0.0f;
                                }
                            }
                            if ((double)num3 < (double)scale)
                                num3 = scale;
                            spriteBatch.DrawString(font, strArray2[index2], vector2_1, color, rotation, origin, baseScale * textSnippet.Scale * scale, SpriteEffects.None, 0.0f);
                            Vector2 vector2_3 = font.MeasureString(strArray2[index2]);
                            if (Utils.Between(vec, vector2_1, vector2_1 + vector2_3))
                                num1 = index1;
                            vector2_1.X += vector2_3.X * baseScale.X * scale;
                            vector2_2.X = Math.Max(vector2_2.X, vector2_1.X);
                        }
                        if (strArray1.Length > 1)
                        {
                            vector2_1.Y += (float)font.LineSpacing * num3 * baseScale.Y;
                            vector2_1.X = position.X;
                            vector2_2.Y = Math.Max(vector2_2.Y, vector2_1.Y);
                            num3 = 0.0f;
                        }
                    }
                }
            }
            hoveredSnippet = num1;
            return vector2_2;
        }

        public static Vector2 DrawColorCodedStringWithShadow(SpriteBatch spriteBatch, SpriteFont font, TextSnippet[] snippets, Vector2 position, float rotation, Vector2 origin, Vector2 baseScale, out int hoveredSnippet, float maxWidth = -1f, float spread = 2f)
        {
            ChatManager.DrawColorCodedStringShadow(spriteBatch, font, snippets, position, Color.Black, rotation, origin, baseScale, maxWidth, spread);
            return ChatManager.DrawColorCodedString(spriteBatch, font, snippets, position, Color.White, rotation, origin, baseScale, out hoveredSnippet, maxWidth, false);
        }

        public static void DrawColorCodedStringShadow(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 baseScale, float maxWidth = -1f, float spread = 2f)
        {
            for (int index = 0; index < ChatManager.ShadowDirections.Length; ++index)
                ChatManager.DrawColorCodedString(spriteBatch, font, text, position + ChatManager.ShadowDirections[index] * spread, baseColor, rotation, origin, baseScale, maxWidth, true);
        }

        public static Vector2 DrawColorCodedString(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 baseScale, float maxWidth = -1f, bool ignoreColors = false)
        {
            Vector2 position1 = position;
            Vector2 vector2 = position1;
            string[] strArray1 = text.Split('\n');
            float num1 = font.MeasureString(" ").X;
            Color color = baseColor;
            float num2 = 1f;
            float num3 = 0.0f;
            foreach (string str1 in strArray1)
            {
                char[] chArray = new char[1]
        {
          ':'
        };
                foreach (string str2 in str1.Split(chArray))
                {
                    if (str2.StartsWith("sss"))
                    {
                        if (str2.StartsWith("sss1"))
                        {
                            if (!ignoreColors)
                                color = Color.Red;
                        }
                        else if (str2.StartsWith("sss2"))
                        {
                            if (!ignoreColors)
                                color = Color.Blue;
                        }
                        else if (str2.StartsWith("sssr") && !ignoreColors)
                            color = Color.White;
                    }
                    else
                    {
                        string[] strArray2 = str2.Split(' ');
                        for (int index = 0; index < strArray2.Length; ++index)
                        {
                            if (index != 0)
                                position1.X += num1 * baseScale.X * num2;
                            if ((double)maxWidth > 0.0)
                            {
                                float num4 = font.MeasureString(strArray2[index]).X * baseScale.X * num2;
                                if ((double)position1.X - (double)position.X + (double)num4 > (double)maxWidth)
                                {
                                    position1.X = position.X;
                                    position1.Y += (float)font.LineSpacing * num3 * baseScale.Y;
                                    vector2.Y = Math.Max(vector2.Y, position1.Y);
                                    num3 = 0.0f;
                                }
                            }
                            if ((double)num3 < (double)num2)
                                num3 = num2;
                            spriteBatch.DrawString(font, strArray2[index], position1, color, rotation, origin, baseScale * num2, SpriteEffects.None, 0.0f);
                            position1.X += font.MeasureString(strArray2[index]).X * baseScale.X * num2;
                            vector2.X = Math.Max(vector2.X, position1.X);
                        }
                    }
                }
                position1.X = position.X;
                position1.Y += (float)font.LineSpacing * num3 * baseScale.Y;
                vector2.Y = Math.Max(vector2.Y, position1.Y);
                num3 = 0.0f;
            }
            return vector2;
        }

        public static Vector2 DrawColorCodedStringWithShadow(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 baseScale, float maxWidth = -1f, float spread = 2f)
        {
            TextSnippet[] snippets = ChatManager.ParseMessage(text, baseColor);
            ChatManager.ConvertNormalSnippets(snippets);
            ChatManager.DrawColorCodedStringShadow(spriteBatch, font, snippets, position, Color.Black, rotation, origin, baseScale, maxWidth, spread);
            int hoveredSnippet;
            return ChatManager.DrawColorCodedString(spriteBatch, font, snippets, position, Color.White, rotation, origin, baseScale, out hoveredSnippet, maxWidth, false);
        }

        public static class Regexes
        {
            public static readonly Regex Format = new Regex("(?<!\\\\)\\[(?<tag>[a-zA-Z]{1,10})(\\/(?<options>[^:]+))?:(?<text>.+?)(?<!\\\\)\\]", RegexOptions.Compiled);
        }
    }
}
