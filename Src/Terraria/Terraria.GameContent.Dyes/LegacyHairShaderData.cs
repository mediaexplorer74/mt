using Microsoft.Xna.Framework;
using GameManager.Graphics.Shaders;

namespace GameManager.GameContent.Dyes
{
	public class LegacyHairShaderData : HairShaderData
	{
		public delegate Color ColorProcessingMethod(Player player, Color color, bool lighting);

		private ColorProcessingMethod _colorProcessor;

		public LegacyHairShaderData()
			: base(null, null)
		{
			_shaderDisabled = true;
		}

		public override Color GetColor(Player player, Color lightColor)
		{
			bool lighting = true;
			Color result = _colorProcessor(player, player.hairColor, lighting);
			if (lighting)
			{
				return new Color(result.ToVector4() * lightColor.ToVector4());
			}
			return result;
		}

		public LegacyHairShaderData UseLegacyMethod(ColorProcessingMethod colorProcessor)
		{
			_colorProcessor = colorProcessor;
			return this;
		}
	}
}
