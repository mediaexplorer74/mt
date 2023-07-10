using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GameManager.IO
{
	public class ResourcePackList
	{
		private struct ResourcePackEntry
		{
			public string FileName;

			public bool Enabled;

			public int SortingOrder;

			public ResourcePackEntry(string name, bool enabled, int sortingOrder)
			{
				FileName = name;
				Enabled = enabled;
				SortingOrder = sortingOrder;
			}
		}

		private readonly List<ResourcePack> _resourcePacks = new List<ResourcePack>();

		public IEnumerable<ResourcePack> EnabledPacks => from pack in _resourcePacks
			where pack.IsEnabled
			orderby pack.SortingOrder, pack.Name, pack.Version, pack.FileName
			select pack;

		public IEnumerable<ResourcePack> DisabledPacks => from pack in _resourcePacks
			where !pack.IsEnabled
			orderby pack.Name, pack.Version, pack.FileName
			select pack;

		public IEnumerable<ResourcePack> AllPacks => from pack in _resourcePacks
			orderby pack.Name, pack.Version, pack.FileName
			select pack;

		public ResourcePackList()
		{
		}

		public ResourcePackList(IEnumerable<ResourcePack> resourcePacks)
		{
			_resourcePacks.AddRange(resourcePacks);
		}

		public JArray ToJson()
		{
			List<ResourcePackEntry> list = new List<ResourcePackEntry>(_resourcePacks.Count);
			list.AddRange(_resourcePacks.Select((ResourcePack pack) => new ResourcePackEntry(pack.FileName, pack.IsEnabled, pack.SortingOrder)));
			return JArray.FromObject((object)list);
		}

		public static ResourcePackList FromJson(JArray serializedState, IServiceProvider services, string searchPath)
		{
			if (!Directory.Exists(searchPath))
			{
				return new ResourcePackList();
			}
			List<ResourcePack> list = new List<ResourcePack>();
			foreach (ResourcePackEntry item2 in CreatePackEntryListFromJson(serializedState))
			{
				if (item2.FileName == null)
				{
					continue;
				}
				string text = Path.Combine(searchPath, item2.FileName);
				try
				{
					if (File.Exists(text) || Directory.Exists(text))
					{
						ResourcePack item = new ResourcePack(services, text)
						{
							IsEnabled = item2.Enabled,
							SortingOrder = item2.SortingOrder
						};
						list.Add(item);
					}
				}
				catch (Exception arg)
				{
					Console.WriteLine("Failed to read resource pack {0}: {1}", text, arg);
				}
			}
			string[] files = Directory.GetFiles(searchPath, "*.zip");
			string fileName;
			foreach (string text2 in files)
			{
				try
				{
					fileName = Path.GetFileName(text2);
					if (list.All((ResourcePack pack) => pack.FileName != fileName))
					{
						list.Add(new ResourcePack(services, text2));
					}
				}
				catch (Exception arg2)
				{
					Console.WriteLine("Failed to read resource pack {0}: {1}", text2, arg2);
				}
			}
			files = Directory.GetDirectories(searchPath);
			string folderName;
			foreach (string text3 in files)
			{
				try
				{
					folderName = Path.GetFileName(text3);
					if (list.All((ResourcePack pack) => pack.FileName != folderName))
					{
						list.Add(new ResourcePack(services, text3));
					}
				}
				catch (Exception arg3)
				{
					Console.WriteLine("Failed to read resource pack {0}: {1}", text3, arg3);
				}
			}
			return new ResourcePackList(list);
		}

		private static IEnumerable<ResourcePackEntry> CreatePackEntryListFromJson(JArray serializedState)
		{
			//IL_0014: Expected O, but got Unknown
			try
			{
				if (((JContainer)serializedState).Count != 0)
				{
					return ((JToken)serializedState).ToObject<List<ResourcePackEntry>>();
				}
			}
			catch (JsonReaderException val)
			{
				JsonReaderException arg = val;
				Console.WriteLine("Failed to parse configuration entry for resource pack list. {0}", arg);
			}
			return new List<ResourcePackEntry>();
		}
	}
}
