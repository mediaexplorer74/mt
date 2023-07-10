using System;
using System.IO;
using GameManager.IO;
using GameManager.Social;
using GameManager.Utilities;

namespace GameManager.Map
{
	public class WorldMap
	{
		public readonly int MaxWidth;

		public readonly int MaxHeight;

		public readonly int BlackEdgeWidth = 40;

		private MapTile[,] _tiles;

		public MapTile this[int x, int y] => _tiles[x, y];

		public WorldMap(int maxWidth, int maxHeight)
		{
			MaxWidth = maxWidth;
			MaxHeight = maxHeight;
			_tiles = new MapTile[MaxWidth, MaxHeight];
		}

		public void ConsumeUpdate(int x, int y)
		{
			_tiles[x, y].IsChanged = false;
		}

		public void Update(int x, int y, byte light)
		{
			_tiles[x, y] = MapHelper.CreateMapTile(x, y, light);
		}

		public void SetTile(int x, int y, MapTile tile)
		{
			_tiles[x, y] = tile;
		}

		public bool IsRevealed(int x, int y)
		{
			return _tiles[x, y].Light > 0;
		}

		public bool UpdateLighting(int x, int y, byte light)
		{
			MapTile other = _tiles[x, y];
			if (light == 0 && other.Light == 0)
			{
				return false;
			}
			MapTile mapTile = MapHelper.CreateMapTile(x, y, Math.Max(other.Light, light));
			if (mapTile.Equals(other))
			{
				return false;
			}
			_tiles[x, y] = mapTile;
			return true;
		}

		public bool UpdateType(int x, int y)
		{
			MapTile mapTile = MapHelper.CreateMapTile(x, y, _tiles[x, y].Light);
			if (mapTile.Equals(_tiles[x, y]))
			{
				return false;
			}
			_tiles[x, y] = mapTile;
			return true;
		}

		public void UnlockMapSection(int sectionX, int sectionY)
		{
		}

		public void Load()
		{
			Lighting.Clear();
			bool isCloudSave = Main.ActivePlayerFileData.IsCloudSave;
			if ((isCloudSave && SocialAPI.Cloud == null) || !Main.mapEnabled)
			{
				return;
			}
			string text = Main.playerPathName.Substring(0, Main.playerPathName.Length - 4) + Path.DirectorySeparatorChar;
			if (Main.ActiveWorldFileData.UseGuidAsMapName)
			{
				string arg = text;
				text = string.Concat(text, Main.ActiveWorldFileData.UniqueId, ".map");
				if (!FileUtilities.Exists(text, isCloudSave))
				{
					text = arg + Main.worldID + ".map";
				}
			}
			else
			{
				text = text + Main.worldID + ".map";
			}
			if (!FileUtilities.Exists(text, isCloudSave))
			{
				Main.MapFileMetadata = FileMetadata.FromCurrentSettings(FileType.Map);
				return;
			}
			using MemoryStream input = new MemoryStream(FileUtilities.ReadAllBytes(text, isCloudSave));
			using BinaryReader binaryReader = new BinaryReader(input);
			try
			{
				int num = binaryReader.ReadInt32();
				if (num <= 230)
				{
					if (num <= 91)
					{
						MapHelper.LoadMapVersion1(binaryReader, num);
					}
					else
					{
						MapHelper.LoadMapVersion2(binaryReader, num);
					}
					ClearEdges();
					Main.clearMap = true;
					Main.loadMap = true;
					Main.loadMapLock = true;
					Main.refreshMap = false;
				}
			}
			catch (Exception value)
			{
				using (StreamWriter streamWriter = new StreamWriter("client-crashlog.txt", append: true))
				{
					streamWriter.WriteLine(DateTime.Now);
					streamWriter.WriteLine(value);
					streamWriter.WriteLine("");
				}
				if (!isCloudSave)
				{
					File.Copy(text, text + ".bad", overwrite: true);
				}
				Clear();
			}
		}

		public void Save()
		{
			MapHelper.SaveMap();
		}

		public void Clear()
		{
			for (int i = 0; i < MaxWidth; i++)
			{
				for (int j = 0; j < MaxHeight; j++)
				{
					_tiles[i, j].Clear();
				}
			}
		}

		public void ClearEdges()
		{
			for (int i = 0; i < MaxWidth; i++)
			{
				for (int j = 0; j < BlackEdgeWidth; j++)
				{
					_tiles[i, j].Clear();
				}
			}
			for (int k = 0; k < MaxWidth; k++)
			{
				for (int l = MaxHeight - BlackEdgeWidth; l < MaxHeight; l++)
				{
					_tiles[k, l].Clear();
				}
			}
			for (int m = 0; m < BlackEdgeWidth; m++)
			{
				for (int n = BlackEdgeWidth; n < MaxHeight - BlackEdgeWidth; n++)
				{
					_tiles[m, n].Clear();
				}
			}
			for (int num = MaxWidth - BlackEdgeWidth; num < MaxWidth; num++)
			{
				for (int num2 = BlackEdgeWidth; num2 < MaxHeight - BlackEdgeWidth; num2++)
				{
					_tiles[num, num2].Clear();
				}
			}
		}
	}
}
