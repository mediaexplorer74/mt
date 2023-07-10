using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using GameManager.DataStructures;
using GameManager.GameContent.Liquid;
using GameManager.Graphics;
using GameManager.Graphics.Light;
using GameManager.Graphics.Shaders;
using GameManager.ID;

namespace GameManager.GameContent.Shaders
{
	public class WaterShaderData : ScreenShaderData
	{
		private struct Ripple
		{
			private static readonly Rectangle[] RIPPLE_SHAPE_SOURCE_RECTS = new Rectangle[3]
			{
				new Rectangle(0, 0, 0, 0),
				new Rectangle(1, 1, 62, 62),
				new Rectangle(1, 65, 62, 62)
			};

			public readonly Vector2 Position;

			public readonly Color WaveData;

			public readonly Vector2 Size;

			public readonly RippleShape Shape;

			public readonly float Rotation;

			public Rectangle SourceRectangle => RIPPLE_SHAPE_SOURCE_RECTS[(int)Shape];

			public Ripple(Vector2 position, Color waveData, Vector2 size, RippleShape shape, float rotation)
			{
				Position = position;
				WaveData = waveData;
				Size = size;
				Shape = shape;
				Rotation = rotation;
			}
		}

		private const float DISTORTION_BUFFER_SCALE = 0.25f;

		private const float WAVE_FRAMERATE = 0.0166666675f;

		private const int MAX_RIPPLES_QUEUED = 200;

		public bool _useViscosityFilter = true;

		private RenderTarget2D _distortionTarget;

		private RenderTarget2D _distortionTargetSwap;

		private bool _usingRenderTargets;

		private Vector2 _lastDistortionDrawOffset = Vector2.Zero;

		private float _progress;

		private Ripple[] _rippleQueue = new Ripple[200];

		private int _rippleQueueCount;

		private int _lastScreenWidth;

		private int _lastScreenHeight;

		public bool _useProjectileWaves = true;

		private bool _useNPCWaves = true;

		private bool _usePlayerWaves = true;

		private bool _useRippleWaves = true;

		private bool _useCustomWaves = true;

		private bool _clearNextFrame = true;

		private Texture2D[] _viscosityMaskChain = new Texture2D[3];

		private int _activeViscosityMask;

		private Asset<Texture2D> _rippleShapeTexture;

		private bool _isWaveBufferDirty = true;

		private int _queuedSteps;

		private const int MAX_QUEUED_STEPS = 2;

		public event Action<TileBatch> OnWaveDraw;

		public WaterShaderData(string passName)
			: base(passName)
		{
			Main.OnRenderTargetsInitialized += InitRenderTargets;
			Main.OnRenderTargetsReleased += ReleaseRenderTargets;
			_rippleShapeTexture = Main.Assets.Request<Texture2D>("Images/Misc/Ripples", Main.content, (AssetRequestMode)1);
			Main.OnPreDraw += PreDraw;
		}

		public override void Update(GameTime gameTime)
		{
			_useViscosityFilter = Main.WaveQuality >= 3;
			_useProjectileWaves = Main.WaveQuality >= 3;
			_usePlayerWaves = Main.WaveQuality >= 2;
			_useRippleWaves = Main.WaveQuality >= 2;
			_useCustomWaves = Main.WaveQuality >= 2;
			if (!Main.gamePaused && Main.hasFocus)
			{
				_progress += (float)gameTime.ElapsedGameTime.TotalSeconds * base.Intensity * 0.75f;
				_progress %= 86400f;
				if (_useProjectileWaves || _useRippleWaves || _useCustomWaves || _usePlayerWaves)
				{
					_queuedSteps++;
				}
				base.Update(gameTime);
			}
		}

		private void StepLiquids()
		{
			_isWaveBufferDirty = true;
			Vector2 value = (Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange));
			Vector2 vector = value - Main.screenPosition;
			TileBatch tileBatch = Main.tileBatch;
			GraphicsDevice graphicsDevice = Main.instance.GraphicsDevice;
			graphicsDevice.SetRenderTarget(_distortionTarget);
			if (_clearNextFrame)
			{
				graphicsDevice.Clear(new Color(0.5f, 0.5f, 0f, 1f));
				_clearNextFrame = false;
			}
			DrawWaves();
			graphicsDevice.SetRenderTarget(_distortionTargetSwap);
			graphicsDevice.Clear(new Color(0.5f, 0.5f, 0.5f, 1f));
			Main.tileBatch.Begin();
			vector *= 0.25f;
			vector.X = (float)Math.Floor(vector.X);
			vector.Y = (float)Math.Floor(vector.Y);
			Vector2 vector2 = vector - _lastDistortionDrawOffset;
			_lastDistortionDrawOffset = vector;
			tileBatch.Draw(_distortionTarget, new Vector4(vector2.X, vector2.Y, _distortionTarget.Width, _distortionTarget.Height), new VertexColors(Color.White));
			GameShaders.Misc["WaterProcessor"].Apply(new DrawData(_distortionTarget, Vector2.Zero, Color.White));
			tileBatch.End();
			RenderTarget2D distortionTarget = _distortionTarget;
			_distortionTarget = _distortionTargetSwap;
			_distortionTargetSwap = distortionTarget;
			if (_useViscosityFilter)
			{
				LiquidRenderer.Instance.SetWaveMaskData(_viscosityMaskChain[_activeViscosityMask]);
				tileBatch.Begin();
				Rectangle cachedDrawArea = LiquidRenderer.Instance.GetCachedDrawArea();
				Rectangle value2 = new Rectangle(0, 0, cachedDrawArea.Height, cachedDrawArea.Width);
				Vector4 destination = new Vector4(cachedDrawArea.X + cachedDrawArea.Width, cachedDrawArea.Y, cachedDrawArea.Height, cachedDrawArea.Width);
				destination *= 16f;
				destination.X -= value.X;
				destination.Y -= value.Y;
				destination *= 0.25f;
				destination.X += vector.X;
				destination.Y += vector.Y;
				graphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
				tileBatch.Draw(_viscosityMaskChain[_activeViscosityMask], destination, value2, new VertexColors(Color.White), Vector2.Zero, SpriteEffects.FlipHorizontally, (float)Math.PI / 2f);
				tileBatch.End();
				_activeViscosityMask++;
				_activeViscosityMask %= _viscosityMaskChain.Length;
			}
			graphicsDevice.SetRenderTarget(null);
		}

		private void DrawWaves()
		{
			Vector2 screenPosition = Main.screenPosition;
			Vector2 value = (Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange));
			Vector2 value2 = -_lastDistortionDrawOffset / 0.25f + value;
			TileBatch tileBatch = Main.tileBatch;
			_ = Main.instance.GraphicsDevice;
			Vector2 dimensions = new Vector2(Main.screenWidth, Main.screenHeight);
			Vector2 value3 = new Vector2(16f, 16f);
			tileBatch.Begin();
			GameShaders.Misc["WaterDistortionObject"].Apply();
			if (_useNPCWaves)
			{
				for (int i = 0; i < 200; i++)
				{
					if (Main.npc[i] == null || !Main.npc[i].active || (!Main.npc[i].wet && Main.npc[i].wetCount == 0) || !Collision.CheckAABBvAABBCollision(screenPosition, dimensions, Main.npc[i].position - value3, Main.npc[i].Size + value3))
					{
						continue;
					}
					NPC nPC = Main.npc[i];
					Vector2 vector = nPC.Center - value2;
					Vector2 vector2 = nPC.velocity.RotatedBy(0f - nPC.rotation) / new Vector2(nPC.height, nPC.width);
					float num = vector2.LengthSquared();
					num = num * 0.3f + 0.7f * num * (1024f / (float)(nPC.height * nPC.width));
					num = Math.Min(num, 0.08f);
					num += (nPC.velocity - nPC.oldVelocity).Length() * 0.5f;
					vector2.Normalize();
					Vector2 velocity = nPC.velocity;
					velocity.Normalize();
					vector -= velocity * 10f;
					if (!_useViscosityFilter && (nPC.honeyWet || nPC.lavaWet))
					{
						num *= 0.3f;
					}
					if (nPC.wet)
					{
						tileBatch.Draw(TextureAssets.MagicPixel.Value, new Vector4(vector.X, vector.Y, (float)nPC.width * 2f, (float)nPC.height * 2f) * 0.25f, null, new VertexColors(new Color(vector2.X * 0.5f + 0.5f, vector2.Y * 0.5f + 0.5f, 0.5f * num)), new Vector2((float)TextureAssets.MagicPixel.Width() / 2f, (float)TextureAssets.MagicPixel.Height() / 2f), SpriteEffects.None, nPC.rotation);
					}
					if (nPC.wetCount != 0)
					{
						num = nPC.velocity.Length();
						num = 0.195f * (float)Math.Sqrt(num);
						float scaleFactor = 5f;
						if (!nPC.wet)
						{
							scaleFactor = -20f;
						}
						QueueRipple(nPC.Center + velocity * scaleFactor, new Color(0.5f, (nPC.wet ? num : (0f - num)) * 0.5f + 0.5f, 0f, 1f) * 0.5f, new Vector2(nPC.width, (float)nPC.height * ((float)(int)nPC.wetCount / 9f)) * MathHelper.Clamp(num * 10f, 0f, 1f), RippleShape.Circle);
					}
				}
			}
			if (_usePlayerWaves)
			{
				for (int j = 0; j < 255; j++)
				{
					if (Main.player[j] == null || !Main.player[j].active || (!Main.player[j].wet && Main.player[j].wetCount == 0) || !Collision.CheckAABBvAABBCollision(screenPosition, dimensions, Main.player[j].position - value3, Main.player[j].Size + value3))
					{
						continue;
					}
					Player player = Main.player[j];
					Vector2 vector3 = player.Center - value2;
					float num2 = player.velocity.Length();
					num2 = 0.05f * (float)Math.Sqrt(num2);
					Vector2 velocity2 = player.velocity;
					velocity2.Normalize();
					vector3 -= velocity2 * 10f;
					if (!_useViscosityFilter && (player.honeyWet || player.lavaWet))
					{
						num2 *= 0.3f;
					}
					if (player.wet)
					{
						tileBatch.Draw(TextureAssets.MagicPixel.Value, new Vector4(vector3.X - (float)player.width * 2f * 0.5f, vector3.Y - (float)player.height * 2f * 0.5f, (float)player.width * 2f, (float)player.height * 2f) * 0.25f, new VertexColors(new Color(velocity2.X * 0.5f + 0.5f, velocity2.Y * 0.5f + 0.5f, 0.5f * num2)));
					}
					if (player.wetCount != 0)
					{
						float scaleFactor2 = 5f;
						if (!player.wet)
						{
							scaleFactor2 = -20f;
						}
						num2 *= 3f;
						QueueRipple(player.Center + velocity2 * scaleFactor2, player.wet ? num2 : (0f - num2), new Vector2(player.width, (float)player.height * ((float)(int)player.wetCount / 9f)) * MathHelper.Clamp(num2 * 10f, 0f, 1f), RippleShape.Circle);
					}
				}
			}
			if (_useProjectileWaves)
			{
				for (int k = 0; k < 1000; k++)
				{
					Projectile projectile = Main.projectile[k];
					if (projectile.wet && !projectile.lavaWet)
					{
						_ = !projectile.honeyWet;
					}
					else
						_ = 0;
					bool flag = projectile.lavaWet;
					bool flag2 = projectile.honeyWet;
					bool flag3 = projectile.wet;
					if (projectile.ignoreWater)
					{
						flag3 = true;
					}
					if (!(projectile != null && projectile.active && ProjectileID.Sets.CanDistortWater[projectile.type] && flag3) || ProjectileID.Sets.NoLiquidDistortion[projectile.type] || !Collision.CheckAABBvAABBCollision(screenPosition, dimensions, projectile.position - value3, projectile.Size + value3))
					{
						continue;
					}
					if (projectile.ignoreWater)
					{
						bool num3 = Collision.LavaCollision(projectile.position, projectile.width, projectile.height);
						flag = Collision.WetCollision(projectile.position, projectile.width, projectile.height);
						flag2 = Collision.honey;
						if (!(num3 || flag || flag2))
						{
							continue;
						}
					}
					Vector2 vector4 = projectile.Center - value2;
					float num4 = projectile.velocity.Length();
					num4 = 2f * (float)Math.Sqrt(0.05f * num4);
					Vector2 velocity3 = projectile.velocity;
					velocity3.Normalize();
					if (!_useViscosityFilter && (flag2 || flag))
					{
						num4 *= 0.3f;
					}
					float num5 = Math.Max(12f, (float)projectile.width * 0.75f);
					float num6 = Math.Max(12f, (float)projectile.height * 0.75f);
					tileBatch.Draw(TextureAssets.MagicPixel.Value, new Vector4(vector4.X - num5 * 0.5f, vector4.Y - num6 * 0.5f, num5, num6) * 0.25f, new VertexColors(new Color(velocity3.X * 0.5f + 0.5f, velocity3.Y * 0.5f + 0.5f, num4 * 0.5f)));
				}
			}
			tileBatch.End();
			if (_useRippleWaves)
			{
				tileBatch.Begin();
				for (int l = 0; l < _rippleQueueCount; l++)
				{
					Vector2 vector5 = _rippleQueue[l].Position - value2;
					Vector2 size = _rippleQueue[l].Size;
					Rectangle sourceRectangle = _rippleQueue[l].SourceRectangle;
					Texture2D value4 = _rippleShapeTexture.Value;
					tileBatch.Draw(value4, new Vector4(vector5.X, vector5.Y, size.X, size.Y) * 0.25f, sourceRectangle, new VertexColors(_rippleQueue[l].WaveData), new Vector2(sourceRectangle.Width / 2, sourceRectangle.Height / 2), SpriteEffects.None, _rippleQueue[l].Rotation);
				}
				tileBatch.End();
			}
			_rippleQueueCount = 0;
			if (_useCustomWaves && this.OnWaveDraw != null)
			{
				tileBatch.Begin();
				this.OnWaveDraw(tileBatch);
				tileBatch.End();
			}
		}

		private void PreDraw(GameTime gameTime)
		{
			ValidateRenderTargets();
			if (!_usingRenderTargets || !Main.IsGraphicsDeviceAvailable)
			{
				return;
			}
			if (_useProjectileWaves || _useRippleWaves || _useCustomWaves || _usePlayerWaves)
			{
				for (int i = 0; i < Math.Min(_queuedSteps, 2); i++)
				{
					StepLiquids();
				}
			}
			else if (_isWaveBufferDirty || _clearNextFrame)
			{
				GraphicsDevice graphicsDevice = Main.instance.GraphicsDevice;
				graphicsDevice.SetRenderTarget(_distortionTarget);
				graphicsDevice.Clear(new Color(0.5f, 0.5f, 0f, 1f));
				_clearNextFrame = false;
				_isWaveBufferDirty = false;
				graphicsDevice.SetRenderTarget(null);
			}
			_queuedSteps = 0;
		}

		public override void Apply()
		{
			if (_usingRenderTargets && Main.IsGraphicsDeviceAvailable)
			{
				UseProgress(_progress);
				Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
				Vector2 value = new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f * (Vector2.One - Vector2.One / Main.GameViewMatrix.Zoom);
				Vector2 value2 = (Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange)) - Main.screenPosition - value;
				UseImage(_distortionTarget, 1);
				UseImage(Main.waterTarget, 2, SamplerState.PointClamp);
				UseTargetPosition(Main.screenPosition - Main.sceneWaterPos + new Vector2(Main.offScreenRange, Main.offScreenRange) + value);
				UseImageOffset(-(value2 * 0.25f - _lastDistortionDrawOffset) / new Vector2(_distortionTarget.Width, _distortionTarget.Height));
				base.Apply();
			}
		}

		private void ValidateRenderTargets()
		{
			int backBufferWidth = Main.instance.GraphicsDevice.PresentationParameters.BackBufferWidth;
			int backBufferHeight = Main.instance.GraphicsDevice.PresentationParameters.BackBufferHeight;
			bool flag = !Main.drawToScreen;
			if (_usingRenderTargets && !flag)
			{
				ReleaseRenderTargets();
			}
			else if (!_usingRenderTargets && flag)
			{
				InitRenderTargets(backBufferWidth, backBufferHeight);
			}
			else if (_usingRenderTargets && flag && (_distortionTarget.IsContentLost || _distortionTargetSwap.IsContentLost))
			{
				_clearNextFrame = true;
			}
		}

		private void InitRenderTargets(int width, int height)
		{
			_lastScreenWidth = width;
			_lastScreenHeight = height;
			width = (int)((float)width * 0.25f);
			height = (int)((float)height * 0.25f);
			try
			{
				_distortionTarget = new RenderTarget2D(Main.instance.GraphicsDevice, width, height, mipMap: false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
				_distortionTargetSwap = new RenderTarget2D(Main.instance.GraphicsDevice, width, height, mipMap: false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
				_usingRenderTargets = true;
				_clearNextFrame = true;
			}
			catch (Exception arg)
			{
				Lighting.Mode = LightMode.Retro;
				_usingRenderTargets = false;
				Console.WriteLine("Failed to create water distortion render targets. " + arg);
			}
		}

		private void ReleaseRenderTargets()
		{
			try
			{
				if (_distortionTarget != null)
				{
					_distortionTarget.Dispose();
				}
				if (_distortionTargetSwap != null)
				{
					_distortionTargetSwap.Dispose();
				}
			}
			catch (Exception arg)
			{
				Console.WriteLine("Error disposing of water distortion render targets. " + arg);
			}
			_distortionTarget = null;
			_distortionTargetSwap = null;
			_usingRenderTargets = false;
		}

		public void QueueRipple(Vector2 position, float strength = 1f, RippleShape shape = RippleShape.Square, float rotation = 0f)
		{
			float g = strength * 0.5f + 0.5f;
			float scale = Math.Min(Math.Abs(strength), 1f);
			QueueRipple(position, new Color(0.5f, g, 0f, 1f) * scale, new Vector2(4f * Math.Max(Math.Abs(strength), 1f)), shape, rotation);
		}

		public void QueueRipple(Vector2 position, float strength, Vector2 size, RippleShape shape = RippleShape.Square, float rotation = 0f)
		{
			float g = strength * 0.5f + 0.5f;
			float scale = Math.Min(Math.Abs(strength), 1f);
			QueueRipple(position, new Color(0.5f, g, 0f, 1f) * scale, size, shape, rotation);
		}

		public void QueueRipple(Vector2 position, Color waveData, Vector2 size, RippleShape shape = RippleShape.Square, float rotation = 0f)
		{
			if (!_useRippleWaves || Main.drawToScreen)
			{
				_rippleQueueCount = 0;
			}
			else if (_rippleQueueCount < _rippleQueue.Length)
			{
				_rippleQueue[_rippleQueueCount++] = new Ripple(position, waveData, size, shape, rotation);
			}
		}
	}
}
