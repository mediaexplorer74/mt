// WorldMap

using System;
using System.IO;
using GameManager.IO;
using GameManager.Utilities;

namespace GameManager.Map
{
    public class WorldMap
    {
        public readonly int MaxWidth;
        public readonly int MaxHeight;
        private MapTile[,] _tiles;
        public MapTile this[int x, int y]
        {
            get
            {
                return this._tiles[x, y];
            }
        }
        public WorldMap(int maxWidth, int maxHeight)
        {
            this.MaxWidth = maxWidth;
            this.MaxHeight = maxHeight;
            this._tiles = new MapTile[this.MaxWidth, this.MaxHeight];
        }
        public void ConsumeUpdate(int x, int y)
        {
            this._tiles[x, y].IsChanged = false;
        }
        public void Update(int x, int y, byte light)
        {
            this._tiles[x, y] = MapHelper.CreateMapTile(x, y, light);
        }
        public void SetTile(int x, int y, ref MapTile tile)
        {
            this._tiles[x, y] = tile;
        }
        public bool IsRevealed(int x, int y)
        {
            return this._tiles[x, y].Light > 0;
        }
        public bool UpdateLighting(int x, int y, byte light)
        {
            MapTile mapTile = this._tiles[x, y];
            MapTile mapTile2 = MapHelper.CreateMapTile(x, y, Math.Max(mapTile.Light, light));
            if (mapTile2.Equals(ref mapTile))
            {
                return false;
            }
            this._tiles[x, y] = mapTile2;
            return true;
        }
        public bool UpdateType(int x, int y)
        {
            MapTile mapTile = MapHelper.CreateMapTile(x, y, this._tiles[x, y].Light);
            if (mapTile.Equals(ref this._tiles[x, y]))
            {
                return false;
            }
            this._tiles[x, y] = mapTile;
            return true;
        }
        public void UnlockMapSection(int sectionX, int sectionY)
        {
        }
        public void Load()
        {
            if (!Game1.mapEnabled)
            {
                return;
            }
            string text = Game1.playerPathName.Substring(0, Game1.playerPathName.Length - 4);
            string text2 = string.Concat(new object[]
			{
				text,
				Path.DirectorySeparatorChar,
				Game1.worldID,
				".map"
			});
            if (!FileUtilities.Exists(text2))
            {
                Game1.MapFileMetadata = FileMetadata.FromCurrentSettings(FileType.Map);
                return;
            }
            using (MemoryStream memoryStream = new MemoryStream(FileUtilities.ReadAllBytes(text2)))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream))
                {
                    try
                    {
                        int num = binaryReader.ReadInt32();
                        if (num <= Game1.curRelease)
                        {
                            if (num <= 91)
                            {
                                MapHelper.LoadMapVersion1(binaryReader, num);
                            }
                            else
                            {
                                MapHelper.LoadMapVersion2(binaryReader, num);
                            }
                            Game1.clearMap = true;
                            Game1.loadMap = true;
                            Game1.loadMapLock = true;
                            Game1.refreshMap = false;
                        }
                    }
                    catch (Exception value)
                    {
                        //RnD
                        //using (StreamWriter streamWriter = new StreamWriter("client-crashlog.txt", true))
                        //{
                        //    streamWriter.WriteLine(DateTime.Now);
                        //    streamWriter.WriteLine(value);
                        //    streamWriter.WriteLine("");
                        //}
                        File.Copy(text2, text2 + ".bad", true);
                        this.Clear();
                    }
                }
            }
        }
        public void Save()
        {
            MapHelper.SaveMap();
        }
        public void Clear()
        {
            for (int i = 0; i < this.MaxWidth; i++)
            {
                for (int j = 0; j < this.MaxHeight; j++)
                {
                    this._tiles[i, j].Clear();
                }
            }
        }
    }
}