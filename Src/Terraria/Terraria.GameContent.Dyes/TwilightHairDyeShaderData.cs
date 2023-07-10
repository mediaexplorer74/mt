using Microsoft.Xna.Framework.Graphics;
using GameManager.DataStructures;
using GameManager.Graphics.Shaders;

namespace GameManager.GameContent.Dyes
{
	public class TwilightHairDyeShaderData : HairShaderData
	{
		public TwilightHairDyeShaderData(Ref<Effect> shader, string passName)
			: base(shader, passName)
		{
		}

		public override void Apply(Player player, DrawData? drawData = null)
		{
			if (drawData.HasValue)
			{
				UseTargetPosition(Main.screenPosition + drawData.Value.position);
			}
			base.Apply(player, drawData);
		}
	}
}
