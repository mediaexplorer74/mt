using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager.DataStructures;
using GameManager.Localization;
using GameManager.UI.Chat;

namespace GameManager.GameContent.Skies.CreditsRoll
{
	public class Segments
	{
		public class LocalizedTextSegment : ICreditsRollSegment
		{
			private const int PixelsForALine = 120;

			private LocalizedText _text;

			private float _timeToShowPeak;

			public float DedicatedTimeNeeded => 240f;

			public LocalizedTextSegment(float timeInAnimation, string textKey)
			{
				_text = Language.GetText(textKey);
				_timeToShowPeak = timeInAnimation;
			}

			public void Draw(CreditsRollInfo info)
			{
				float num = 400f;
				float num2 = 400f;
				int timeInAnimation = info.TimeInAnimation;
				float num3 = Utils.GetLerpValue(_timeToShowPeak - num, _timeToShowPeak, timeInAnimation, clamped: true) * Utils.GetLerpValue(_timeToShowPeak + num2, _timeToShowPeak, timeInAnimation, clamped: true);
				if (!(num3 <= 0f))
				{
					float num4 = _timeToShowPeak - (float)timeInAnimation;
					Vector2 position = info.AnchorPositionOnScreen + new Vector2(0f, num4 * 0.5f);
					float num5 = _timeToShowPeak / 100f % 1f;
					if (num5 < 0f)
					{
						num5 += 1f;
					}
					Color value = Main.hslToRgb(num5, 1f, 0.5f);
					string value2 = _text.Value;
					Vector2 origin = FontAssets.DeathText.Value.MeasureString(value2);
					origin *= 0.5f;
					float scale = 1f - (1f - num3) * (1f - num3);
					ChatManager.DrawColorCodedStringShadow(info.SpriteBatch, FontAssets.DeathText.Value, value2, position, value * scale * scale * 0.25f, 0f, origin, Vector2.One);
					ChatManager.DrawColorCodedString(info.SpriteBatch, FontAssets.DeathText.Value, value2, position, Color.White * scale, 0f, origin, Vector2.One);
				}
			}
		}

		public abstract class ACreditsRollSegmentWithActions<T> : ICreditsRollSegment
		{
			private int _dedicatedTimeNeeded;

			private int _lastDedicatedTimeNeeded;

			protected int _targetTime;

			private List<ICreditsRollSegmentAction<T>> _actions = new List<ICreditsRollSegmentAction<T>>();

			public float DedicatedTimeNeeded => _dedicatedTimeNeeded;

			public ACreditsRollSegmentWithActions(int targetTime)
			{
				_targetTime = targetTime;
				_dedicatedTimeNeeded = 0;
			}

			protected void ProcessActions(T obj, float localTimeForObject)
			{
				for (int i = 0; i < _actions.Count; i++)
				{
					_actions[i].ApplyTo(obj, localTimeForObject);
				}
			}

			public ACreditsRollSegmentWithActions<T> Then(ICreditsRollSegmentAction<T> act)
			{
				Bind(act);
				act.SetDelay(_dedicatedTimeNeeded);
				_actions.Add(act);
				_lastDedicatedTimeNeeded = _dedicatedTimeNeeded;
				_dedicatedTimeNeeded += act.ExpectedLengthOfActionInFrames;
				return this;
			}

			public ACreditsRollSegmentWithActions<T> With(ICreditsRollSegmentAction<T> act)
			{
				Bind(act);
				act.SetDelay(_lastDedicatedTimeNeeded);
				_actions.Add(act);
				return this;
			}

			protected abstract void Bind(ICreditsRollSegmentAction<T> act);

			public abstract void Draw(CreditsRollInfo info);
		}

		public class NPCSegment : ACreditsRollSegmentWithActions<NPC>
		{
			private NPC _npc;

			private Vector2 _anchorOffset;

			private Vector2 _normalizedOriginForHitbox;

			public NPCSegment(int targetTime, int npcId, Vector2 anchorOffset, Vector2 normalizedNPCHitboxOrigin)
				: base(targetTime)
			{
				_npc = new NPC();
				_npc.SetDefaults(npcId, new NPCSpawnParams
				{
					gameModeData = Main.RegisterdGameModes[0],
					playerCountForMultiplayerDifficultyOverride = 1,
					sizeScaleOverride = null,
					strengthMultiplierOverride = 1f
				});
				_npc.IsABestiaryIconDummy = true;
				_anchorOffset = anchorOffset;
				_normalizedOriginForHitbox = normalizedNPCHitboxOrigin;
			}

			protected override void Bind(ICreditsRollSegmentAction<NPC> act)
			{
				act.BindTo(_npc);
			}

			public override void Draw(CreditsRollInfo info)
			{
				if (!((float)info.TimeInAnimation > (float)_targetTime + base.DedicatedTimeNeeded))
				{
					ResetNPCAnimation(info);
					float localTimeForObject = info.TimeInAnimation - _targetTime;
					ProcessActions(_npc, localTimeForObject);
					if (_npc.alpha < 255)
					{
						_npc.FindFrame();
						Main.instance.DrawNPCDirect(info.SpriteBatch, _npc, _npc.behindTiles, Vector2.Zero);
					}
				}
			}

			private void ResetNPCAnimation(CreditsRollInfo info)
			{
				_npc.position = info.AnchorPositionOnScreen + _anchorOffset - _npc.Size * _normalizedOriginForHitbox;
				_npc.alpha = 0;
				_npc.velocity = Vector2.Zero;
			}
		}

		public class LooseSprite
		{
			private DrawData _originalDrawData;

			public DrawData CurrentDrawData;

			public float CurrentOpacity;

			public LooseSprite(DrawData data)
			{
				_originalDrawData = data;
				Reset();
			}

			public void Reset()
			{
				CurrentDrawData = _originalDrawData;
				CurrentOpacity = 1f;
			}
		}

		public class SpriteSegment : ACreditsRollSegmentWithActions<LooseSprite>
		{
			private LooseSprite _sprite;

			private Vector2 _anchorOffset;

			public SpriteSegment(int targetTime, DrawData data, Vector2 anchorOffset)
				: base(targetTime)
			{
				_sprite = new LooseSprite(data);
				_anchorOffset = anchorOffset;
			}

			protected override void Bind(ICreditsRollSegmentAction<LooseSprite> act)
			{
				act.BindTo(_sprite);
			}

			public override void Draw(CreditsRollInfo info)
			{
				if (!((float)info.TimeInAnimation > (float)_targetTime + base.DedicatedTimeNeeded))
				{
					ResetSpriteAnimation(info);
					float localTimeForObject = info.TimeInAnimation - _targetTime;
					ProcessActions(_sprite, localTimeForObject);
					DrawData currentDrawData = _sprite.CurrentDrawData;
					currentDrawData.position += info.AnchorPositionOnScreen;
					currentDrawData.color *= _sprite.CurrentOpacity;
					currentDrawData.Draw(info.SpriteBatch);
				}
			}

			private void ResetSpriteAnimation(CreditsRollInfo info)
			{
				_sprite.Reset();
			}
		}

		public class EmoteSegment : ICreditsRollSegment
		{
			private int _targetTime;

			private Vector2 _offset;

			private SpriteEffects _effect;

			private int _emoteId;

			public float DedicatedTimeNeeded
			{
				get;
				private set;
			}

			public EmoteSegment(int emoteId, int targetTime, int timeToPlay, Vector2 position, SpriteEffects drawEffect)
			{
				_emoteId = emoteId;
				_targetTime = targetTime;
				_effect = drawEffect;
				_offset = position;
				DedicatedTimeNeeded = timeToPlay;
			}

			public void Draw(CreditsRollInfo info)
			{
				int num = info.TimeInAnimation - _targetTime;
				if (num < 0 || (float)num >= DedicatedTimeNeeded)
				{
					return;
				}
				Vector2 vector = info.AnchorPositionOnScreen + _offset;
				vector = vector;
				bool flag = num < 6 || (float)num >= DedicatedTimeNeeded - 6f;
				Texture2D value = TextureAssets.Extra[48].Value;
				Rectangle value2 = value.Frame(8, 38, (!flag) ? 1 : 0);
				Vector2 origin = new Vector2(value2.Width / 2, value2.Height);
				SpriteEffects spriteEffects = _effect;
				info.SpriteBatch.Draw(value, vector, value2, Color.White, 0f, origin, 1f, spriteEffects, 0f);
				if (!flag)
				{
					int emoteId = _emoteId;
					if ((emoteId == 87 || emoteId == 89) && spriteEffects.HasFlag(SpriteEffects.FlipHorizontally))
					{
						spriteEffects &= ~SpriteEffects.FlipHorizontally;
						vector.X += 4f;
					}
					info.SpriteBatch.Draw(value, vector, GetFrame(num % 20), Color.White, 0f, origin, 1f, spriteEffects, 0f);
				}
			}

			private Rectangle GetFrame(int wrappedTime)
			{
				int num = ((wrappedTime >= 10) ? 1 : 0);
				return TextureAssets.Extra[48].Value.Frame(8, 38, _emoteId % 4 * 2 + num, _emoteId / 4 + 1);
			}
		}

		private const float PixelsToRollUpPerFrame = 0.5f;
	}
}
