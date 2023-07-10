using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace GameManager.GameContent.RGB
{
	public class GemCaveShader : ChromaShader
	{
		private readonly Vector4 _primaryColor;

		private readonly Vector4 _secondaryColor;

		private static readonly Vector4[] _gemColors = new Vector4[7]
		{
			Color.White.ToVector4(),
			Color.Yellow.ToVector4(),
			Color.Orange.ToVector4(),
			Color.Red.ToVector4(),
			Color.Green.ToVector4(),
			Color.Blue.ToVector4(),
			Color.Purple.ToVector4()
		};

		public GemCaveShader(Color primaryColor, Color secondaryColor)
			: base()
		{
			_primaryColor = primaryColor.ToVector4();
			_secondaryColor = secondaryColor.ToVector4();
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			time *= 0.25f;
			float num = time % 1f;
			bool num2 = time % 2f > 1f;
			Vector4 vector = (num2 ? _secondaryColor : _primaryColor);
			Vector4 value = (num2 ? _primaryColor : _secondaryColor);
			num *= 1.2f;
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
				float staticNoise = NoiseHelper.GetStaticNoise(canvasPositionOfIndex * 0.5f + new Vector2(0f, time * 0.5f));
				Vector4 value2 = vector;
				staticNoise += num;
				if (staticNoise > 0.999f)
				{
					float amount = MathHelper.Clamp((staticNoise - 0.999f) / 0.2f, 0f, 1f);
					value2 = Vector4.Lerp(value2, value, amount);
				}
				float dynamicNoise = NoiseHelper.GetDynamicNoise(gridPositionOfIndex.X, gridPositionOfIndex.Y, time / 100f);
				dynamicNoise = Math.Max(0f, 1f - dynamicNoise * 20f);
				value2 = Vector4.Lerp(value2, _gemColors[((gridPositionOfIndex.Y * 47 + gridPositionOfIndex.X) % _gemColors.Length + _gemColors.Length) % _gemColors.Length], dynamicNoise);
				fragment.SetColor(i, value2);
				fragment.SetColor(i, value2);
			}
		}
	}
}
