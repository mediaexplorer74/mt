namespace GameManager
{
	public class World
	{
		public Tile[,] Tiles => Main.tile;

		public int TileColumns => Main.maxTilesX;

		public int TileRows => Main.maxTilesY;

		public Player[] Players => Main.player;
	}
}
