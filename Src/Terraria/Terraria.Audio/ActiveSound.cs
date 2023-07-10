using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace GameManager.Audio
{
	public class ActiveSound
	{
		public readonly bool IsGlobal;

		public Vector2 Position;

		public float Volume;

		public SoundEffectInstance Sound
		{
			get;
			private set;
		}

		public SoundStyle Style
		{
			get;
			private set;
		}

		public bool IsPlaying => Sound.State == SoundState.Playing;

		public ActiveSound(SoundStyle style, Vector2 position)
		{
			Position = position;
			Volume = 1f;
			IsGlobal = false;
			Style = style;
			Play();
		}

		public ActiveSound(SoundStyle style)
		{
			Position = Vector2.Zero;
			Volume = 1f;
			IsGlobal = true;
			Style = style;
			Play();
		}

		private void Play()
		{
			SoundEffectInstance soundEffectInstance = Style.GetRandomSound().CreateInstance();
			soundEffectInstance.Pitch += Style.GetRandomPitch();
			soundEffectInstance.Play();
			SoundInstanceGarbageCollector.Track(soundEffectInstance);
			Sound = soundEffectInstance;
			Update();
		}

		public void Stop()
		{
			if (Sound != null)
			{
				Sound.Stop();
			}
		}

		public void Pause()
		{
			if (Sound != null && Sound.State == SoundState.Playing)
			{
				Sound.Pause();
			}
		}

		public void Resume()
		{
			if (Sound != null && Sound.State == SoundState.Paused)
			{
				Sound.Resume();
			}
		}

		public void Update()
		{
			if (Sound != null)
			{
				Vector2 value = Main.screenPosition + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
				float num = 1f;
				if (!IsGlobal)
				{
					float value2 = (Position.X - value.X) / ((float)Main.screenWidth * 0.5f);
					value2 = MathHelper.Clamp(value2, -1f, 1f);
					Sound.Pan = value2;
					float num2 = Vector2.Distance(Position, value);
					num = 1f - num2 / ((float)Main.screenWidth * 1.5f);
				}
				num *= Style.Volume * Volume;
				switch (Style.Type)
				{
				case SoundType.Sound:
					num *= Main.soundVolume;
					break;
				case SoundType.Ambient:
					num *= Main.ambientVolume;
					break;
				case SoundType.Music:
					num *= Main.musicVolume;
					break;
				}
				num = MathHelper.Clamp(num, 0f, 1f);
				Sound.Volume = num;
			}
		}
	}
}
