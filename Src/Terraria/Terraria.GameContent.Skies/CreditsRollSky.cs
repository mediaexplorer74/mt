using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager.DataStructures;
using GameManager.GameContent.Skies.CreditsRoll;
using GameManager.Graphics.Effects;

namespace GameManager.GameContent.Skies
{
	public class CreditsRollSky : CustomSky
	{
		private int _endTime;

		private int _currentTime;

		private List<ICreditsRollSegment> _segments;

		public void EnsureSegmentsAreMade()
		{
			_segments = new List<ICreditsRollSegment>();
			(new string[1])[0] = "Now, this is a story all about how";
			Segments.ACreditsRollSegmentWithActions<NPC> aCreditsRollSegmentWithActions = new Segments.NPCSegment(normalizedNPCHitboxOrigin: new Vector2(0.5f, 1f), targetTime: 0, npcId: 22, anchorOffset: new Vector2(-300f, 0f)).Then(new Actions.NPCs.Fade(255)).With(new Actions.NPCs.Fade(-5, 51)).Then(new Actions.NPCs.Move(new Vector2(1f, 0f), 60));
			DrawData data = new DrawData(TextureAssets.Extra[156].Value, Vector2.Zero, null, Color.White, 0f, TextureAssets.Extra[156].Size() / 2f, 0.25f, SpriteEffects.None, 0);
			Segments.ACreditsRollSegmentWithActions<Segments.LooseSprite> item = new Segments.SpriteSegment(0, data, new Vector2(-100f, 0f)).Then(new Actions.Sprites.Fade(0f, 0)).Then(new Actions.Sprites.Fade(1f, 60)).Then(new Actions.Sprites.Wait(60))
				.Then(new Actions.Sprites.Fade(0f, 60));
			int num = 60;
			Segments.EmoteSegment item2 = new Segments.EmoteSegment(3, (int)aCreditsRollSegmentWithActions.DedicatedTimeNeeded, num, new Vector2(-254f, -38f), SpriteEffects.FlipHorizontally);
			aCreditsRollSegmentWithActions.Then(new Actions.NPCs.Wait(num)).Then(new Actions.NPCs.Wait(60)).With(new Actions.NPCs.Fade(5, 51));
			_segments.Add(aCreditsRollSegmentWithActions);
			_segments.Add(item2);
			_segments.Add(item);
			foreach (ICreditsRollSegment segment in _segments)
			{
				_endTime += (int)segment.DedicatedTimeNeeded;
			}
			_endTime += 300;
		}

		public override void Update(GameTime gameTime)
		{
			_currentTime++;
			int num = 0;
			foreach (ICreditsRollSegment segment in _segments)
			{
				num += (int)segment.DedicatedTimeNeeded;
			}
			num++;
			if (_currentTime >= num)
			{
				_currentTime = 0;
			}
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			float num = 4.5f;
			if (!(num < minDepth) && !(num > maxDepth))
			{
				CreditsRollInfo creditsRollInfo = default(CreditsRollInfo);
				creditsRollInfo.SpriteBatch = spriteBatch;
				creditsRollInfo.AnchorPositionOnScreen = Main.ScreenSize.ToVector2() / 2f;
				creditsRollInfo.TimeInAnimation = _currentTime;
				CreditsRollInfo info = creditsRollInfo;
				for (int i = 0; i < _segments.Count; i++)
				{
					_segments[i].Draw(info);
				}
			}
		}

		public override bool IsActive()
		{
			if (_currentTime >= _endTime)
			{
				return false;
			}
			return true;
		}

		public override void Reset()
		{
			_currentTime = 0;
			EnsureSegmentsAreMade();
		}

		public override void Activate(Vector2 position, params object[] args)
		{
			_currentTime = 0;
			EnsureSegmentsAreMade();
		}

		public override void Deactivate(params object[] args)
		{
			_currentTime = 0;
		}
	}
}
