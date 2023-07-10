using Microsoft.Xna.Framework;

namespace GameManager.GameContent.Skies.CreditsRoll
{
	public class Actions
	{
		public class NPCs
		{
			public interface INPCAction : ICreditsRollSegmentAction<NPC>
			{
			}

			public class Fade : INPCAction, ICreditsRollSegmentAction<NPC>
			{
				private int _duration;

				private int _alphaPerFrame;

				private float _delay;

				public int ExpectedLengthOfActionInFrames => _duration;

				public Fade(int alphaPerFrame)
				{
					_duration = 0;
					_alphaPerFrame = alphaPerFrame;
				}

				public Fade(int alphaPerFrame, int duration)
				{
					_duration = duration;
					_alphaPerFrame = alphaPerFrame;
				}

				public void BindTo(NPC obj)
				{
				}

				public void SetDelay(float delay)
				{
					_delay = delay;
				}

				public void ApplyTo(NPC obj, float localTimeForObj)
				{
					if (localTimeForObj < _delay)
					{
						return;
					}
					if (_duration == 0)
					{
						obj.alpha = Utils.Clamp(obj.alpha + _alphaPerFrame, 0, 255);
						return;
					}
					float num = localTimeForObj - _delay;
					if (num > (float)_duration)
					{
						num = _duration;
					}
					obj.alpha = Utils.Clamp(obj.alpha + (int)num * _alphaPerFrame, 0, 255);
				}
			}

			public class Move : INPCAction, ICreditsRollSegmentAction<NPC>
			{
				private Vector2 _offsetPerFrame;

				private int _duration;

				private float _delay;

				public int ExpectedLengthOfActionInFrames => _duration;

				public Move(Vector2 offsetPerFrame, int durationInFrames)
				{
					_offsetPerFrame = offsetPerFrame;
					_duration = durationInFrames;
				}

				public void BindTo(NPC obj)
				{
				}

				public void SetDelay(float delay)
				{
					_delay = delay;
				}

				public void ApplyTo(NPC obj, float localTimeForObj)
				{
					if (!(localTimeForObj < _delay))
					{
						float num = localTimeForObj - _delay;
						if (num > (float)_duration)
						{
							num = _duration;
						}
						obj.position += _offsetPerFrame * num;
						obj.velocity = _offsetPerFrame;
						if (_offsetPerFrame.X != 0f)
						{
							obj.direction = (obj.spriteDirection = ((_offsetPerFrame.X > 0f) ? 1 : (-1)));
						}
					}
				}
			}

			public class Wait : INPCAction, ICreditsRollSegmentAction<NPC>
			{
				private int _duration;

				private float _delay;

				public int ExpectedLengthOfActionInFrames => _duration;

				public Wait(int durationInFrames)
				{
					_duration = durationInFrames;
				}

				public void BindTo(NPC obj)
				{
				}

				public void ApplyTo(NPC obj, float localTimeForObj)
				{
					if (!(localTimeForObj < _delay))
					{
						obj.velocity = Vector2.Zero;
					}
				}

				public void SetDelay(float delay)
				{
					_delay = delay;
				}
			}

			public class LookAt : INPCAction, ICreditsRollSegmentAction<NPC>
			{
				private int _direction;

				private float _delay;

				public int ExpectedLengthOfActionInFrames => 0;

				public LookAt(int direction)
				{
					_direction = direction;
				}

				public void BindTo(NPC obj)
				{
				}

				public void ApplyTo(NPC obj, float localTimeForObj)
				{
					if (!(localTimeForObj < _delay))
					{
						obj.direction = (obj.spriteDirection = _direction);
					}
				}

				public void SetDelay(float delay)
				{
					_delay = delay;
				}
			}

			public class PartyHard : INPCAction, ICreditsRollSegmentAction<NPC>
			{
				public int ExpectedLengthOfActionInFrames => 0;

				public void BindTo(NPC obj)
				{
					obj.ForcePartyHatOn = true;
					obj.UpdateAltTexture();
				}

				public void ApplyTo(NPC obj, float localTimeForObj)
				{
				}

				public void SetDelay(float delay)
				{
				}
			}
		}

		public class Sprites
		{
			public interface ISpriteAction : ICreditsRollSegmentAction<Segments.LooseSprite>
			{
			}

			public class Fade : ISpriteAction, ICreditsRollSegmentAction<Segments.LooseSprite>
			{
				private int _duration;

				private float _opacityTarget;

				private float _delay;

				public int ExpectedLengthOfActionInFrames => _duration;

				public Fade(float opacityTarget)
				{
					_duration = 0;
					_opacityTarget = opacityTarget;
				}

				public Fade(float opacityTarget, int duration)
				{
					_duration = duration;
					_opacityTarget = opacityTarget;
				}

				public void BindTo(Segments.LooseSprite obj)
				{
				}

				public void SetDelay(float delay)
				{
					_delay = delay;
				}

				public void ApplyTo(Segments.LooseSprite obj, float localTimeForObj)
				{
					if (localTimeForObj < _delay)
					{
						return;
					}
					if (_duration == 0)
					{
						obj.CurrentOpacity = _opacityTarget;
						return;
					}
					float num = localTimeForObj - _delay;
					if (num > (float)_duration)
					{
						num = _duration;
					}
					obj.CurrentOpacity = MathHelper.Lerp(obj.CurrentOpacity, _opacityTarget, Utils.GetLerpValue(0f, _duration, num, clamped: true));
				}
			}

			public class Wait : ISpriteAction, ICreditsRollSegmentAction<Segments.LooseSprite>
			{
				private int _duration;

				private float _delay;

				public int ExpectedLengthOfActionInFrames => _duration;

				public Wait(int durationInFrames)
				{
					_duration = durationInFrames;
				}

				public void BindTo(Segments.LooseSprite obj)
				{
				}

				public void ApplyTo(Segments.LooseSprite obj, float localTimeForObj)
				{
					_ = _delay;
				}

				public void SetDelay(float delay)
				{
					_delay = delay;
				}
			}
		}
	}
}
