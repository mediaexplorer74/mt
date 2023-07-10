using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using GameManager.Graphics.Effects;
using GameManager.Utilities;

namespace GameManager.GameContent.Skies
{
	public class NebulaSky : CustomSky
	{
		private struct LightPillar
		{
			public Vector2 Position;

			public float Depth;
		}

		private LightPillar[] _pillars;

		private UnifiedRandom _random = new UnifiedRandom();

		private Asset<Texture2D> _planetTexture;

		private Asset<Texture2D> _bgTexture;

		private Asset<Texture2D> _beamTexture;

		private Asset<Texture2D>[] _rockTextures;

		private bool _isActive;

		private float _fadeOpacity;

		public override void OnLoad()
		{
			_planetTexture = Main.Assets.Request<Texture2D>("Images/Misc/NebulaSky/Planet", Main.content, (AssetRequestMode)1);
			_bgTexture = Main.Assets.Request<Texture2D>("Images/Misc/NebulaSky/Background", Main.content, (AssetRequestMode)1);
			_beamTexture = Main.Assets.Request<Texture2D>("Images/Misc/NebulaSky/Beam", Main.content, (AssetRequestMode)1);
			_rockTextures = new Asset<Texture2D>[3];
			for (int i = 0; i < _rockTextures.Length; i++)
			{
				_rockTextures[i] = Main.Assets.Request<Texture2D>("Images/Misc/NebulaSky/Rock_" + i, Main.content, (AssetRequestMode)1);
			}
		}

		public override void Update(GameTime gameTime)
		{
			if (_isActive)
			{
				_fadeOpacity = Math.Min(1f, 0.01f + _fadeOpacity);
			}
			else
			{
				_fadeOpacity = Math.Max(0f, _fadeOpacity - 0.01f);
			}
		}

		public override Color OnTileColor(Color inColor)
		{
			return new Color(Vector4.Lerp(inColor.ToVector4(), Vector4.One, _fadeOpacity * 0.5f));
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (maxDepth >= float.MaxValue && minDepth < float.MaxValue)
			{
				spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * _fadeOpacity);
				spriteBatch.Draw(_bgTexture.Value, new Rectangle(0, Math.Max(0, (int)((Main.worldSurface * 16.0 - (double)Main.screenPosition.Y - 2400.0) * 0.10000000149011612)), Main.screenWidth, Main.screenHeight), Color.White * Math.Min(1f, (Main.screenPosition.Y - 800f) / 1000f * _fadeOpacity));
				Vector2 value = new Vector2(Main.screenWidth >> 1, Main.screenHeight >> 1);
				Vector2 value2 = 0.01f * (new Vector2((float)Main.maxTilesX * 8f, (float)Main.worldSurface / 2f) - Main.screenPosition);
				spriteBatch.Draw(_planetTexture.Value, value + new Vector2(-200f, -200f) + value2, null, Color.White * 0.9f * _fadeOpacity, 0f, new Vector2(_planetTexture.Width() >> 1, _planetTexture.Height() >> 1), 1f, SpriteEffects.None, 1f);
			}
			int num = -1;
			int num2 = 0;
			for (int i = 0; i < _pillars.Length; i++)
			{
				float depth = _pillars[i].Depth;
				if (num == -1 && depth < maxDepth)
				{
					num = i;
				}
				if (depth <= minDepth)
				{
					break;
				}
				num2 = i;
			}
			if (num == -1)
			{
				return;
			}
			Vector2 value3 = Main.screenPosition + new Vector2(Main.screenWidth >> 1, Main.screenHeight >> 1);
			Rectangle rectangle = new Rectangle(-1000, -1000, 4000, 4000);
			float scale = Math.Min(1f, (Main.screenPosition.Y - 1000f) / 1000f);
			for (int j = num; j < num2; j++)
			{
				Vector2 value4 = new Vector2(1f / _pillars[j].Depth, 0.9f / _pillars[j].Depth);
				Vector2 position = _pillars[j].Position;
				position = (position - value3) * value4 + value3 - Main.screenPosition;
				if (rectangle.Contains((int)position.X, (int)position.Y))
				{
					float num3 = value4.X * 450f;
					spriteBatch.Draw(_beamTexture.Value, position, null, Color.White * 0.2f * scale * _fadeOpacity, 0f, Vector2.Zero, new Vector2(num3 / 70f, num3 / 45f), SpriteEffects.None, 0f);
					int num4 = 0;
					for (float num5 = 0f; num5 <= 1f; num5 += 0.03f)
					{
						float num6 = 1f - (num5 + Main.GlobalTimeWrappedHourly * 0.02f + (float)Math.Sin(j)) % 1f;
						spriteBatch.Draw(_rockTextures[num4].Value, position + new Vector2((float)Math.Sin(num5 * 1582f) * (num3 * 0.5f) + num3 * 0.5f, num6 * 2000f), null, Color.White * num6 * scale * _fadeOpacity, num6 * 20f, new Vector2(_rockTextures[num4].Width() >> 1, _rockTextures[num4].Height() >> 1), 0.9f, SpriteEffects.None, 0f);
						num4 = (num4 + 1) % _rockTextures.Length;
					}
				}
			}
		}

		public override float GetCloudAlpha()
		{
			return (1f - _fadeOpacity) * 0.3f + 0.7f;
		}

		public override void Activate(Vector2 position, params object[] args)
		{
			_fadeOpacity = 0.002f;
			_isActive = true;
			_pillars = new LightPillar[40];
			for (int i = 0; i < _pillars.Length; i++)
			{
				_pillars[i].Position.X = (float)i / (float)_pillars.Length * ((float)Main.maxTilesX * 16f + 20000f) + _random.NextFloat() * 40f - 20f - 20000f;
				_pillars[i].Position.Y = _random.NextFloat() * 200f - 2000f;
				_pillars[i].Depth = _random.NextFloat() * 8f + 7f;
			}
			Array.Sort(_pillars, SortMethod);
		}

		private int SortMethod(LightPillar pillar1, LightPillar pillar2)
		{
			return pillar2.Depth.CompareTo(pillar1.Depth);
		}

		public override void Deactivate(params object[] args)
		{
			_isActive = false;
		}

		public override void Reset()
		{
			_isActive = false;
		}

		public override bool IsActive()
		{
			if (!_isActive)
			{
				return _fadeOpacity > 0.001f;
			}
			return true;
		}
	}
}
