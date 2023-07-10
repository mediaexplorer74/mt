using System.Collections.Generic;
using System.IO;
using GameManager.ID;

namespace GameManager.GameContent.Creative
{
	public class ItemsSacrificedUnlocksTracker : IPersistentPerWorldContent, IOnPlayerJoining
	{
		public const int POSITIVE_SACRIFICE_COUNT_CAP = 9999;

		private Dictionary<string, int> _sacrificeCountByItemPersistentId;

		public readonly Dictionary<int, int> SacrificesCountByItemIdCache;

		public int LastEditId
		{
			get;
			private set;
		}

		public ItemsSacrificedUnlocksTracker()
		{
			_sacrificeCountByItemPersistentId = new Dictionary<string, int>();
			SacrificesCountByItemIdCache = new Dictionary<int, int>();
			LastEditId = 0;
		}

		public int GetSacrificeCount(int itemId)
		{
			SacrificesCountByItemIdCache.TryGetValue(itemId, out var value);
			return value;
		}

		public void RegisterItemSacrifice(int itemId, int amount)
		{
			if (ContentSamples.ItemPersistentIdsByNetIds.TryGetValue(itemId, out var value))
			{
				_sacrificeCountByItemPersistentId.TryGetValue(value, out var value2);
				value2 += amount;
				int value3 = Utils.Clamp(value2, 0, 9999);
				_sacrificeCountByItemPersistentId[value] = value3;
				SacrificesCountByItemIdCache[itemId] = value3;
				MarkContentsDirty();
			}
		}

		public void SetSacrificeCountDirectly(string persistentId, int sacrificeCount)
		{
			int value = Utils.Clamp(sacrificeCount, 0, 9999);
			_sacrificeCountByItemPersistentId[persistentId] = value;
			if (ContentSamples.ItemNetIdsByPersistentIds.TryGetValue(persistentId, out var value2))
			{
				SacrificesCountByItemIdCache[value2] = value;
				MarkContentsDirty();
			}
		}

		public void Save(BinaryWriter writer)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>(_sacrificeCountByItemPersistentId);
			writer.Write(dictionary.Count);
			foreach (KeyValuePair<string, int> item in dictionary)
			{
				writer.Write(item.Key);
				writer.Write(item.Value);
			}
		}

		public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn)
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string key = reader.ReadString();
				int value = reader.ReadInt32();
				_sacrificeCountByItemPersistentId[key] = value;
				if (ContentSamples.ItemNetIdsByPersistentIds.TryGetValue(key, out var value2))
				{
					SacrificesCountByItemIdCache[value2] = value;
				}
			}
		}

		public void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn)
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				reader.ReadString();
				reader.ReadInt32();
			}
		}

		public void Reset()
		{
			_sacrificeCountByItemPersistentId.Clear();
			SacrificesCountByItemIdCache.Clear();
			MarkContentsDirty();
		}

		public void OnPlayerJoining(int playerIndex)
		{
		}

		public void MarkContentsDirty()
		{
			LastEditId++;
		}
	}
}
