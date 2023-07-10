using Microsoft.Xna.Framework.Graphics;
using GameManager.DataStructures;
using GameManager.Graphics.Shaders;

namespace GameManager.GameContent.Dyes
{
	public class TwilightDyeShaderData : ArmorShaderData
	{
		public TwilightDyeShaderData(Ref<Effect> shader, string passName)
			: base(shader, passName)
		{
		}

		public override void Apply(Entity entity, DrawData? drawData)
		{
			if (drawData.HasValue)
			{
				Player player = entity as Player;
				if (player != null && !player.isDisplayDollOrInanimate && !player.isHatRackDoll)
				{
					UseTargetPosition(Main.screenPosition + drawData.Value.position);
				}
				else if (entity is Projectile)
				{
					UseTargetPosition(Main.screenPosition + drawData.Value.position);
				}
				else
				{
					UseTargetPosition(drawData.Value.position);
				}
			}
			base.Apply(entity, drawData);
		}
	}
}
