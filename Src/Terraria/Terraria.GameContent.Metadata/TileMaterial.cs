using Newtonsoft.Json;

namespace GameManager.GameContent.Metadata
{
	public class TileMaterial
	{
		[JsonProperty]
		public TileGolfPhysics GolfPhysics
		{
			get;
			private set;
		}
	}
}
