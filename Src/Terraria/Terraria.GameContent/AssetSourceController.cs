using System;
using System.Collections.Generic;
using System.Linq;
using ReLogic.Content;
using ReLogic.Content.Sources;
using GameManager.IO;

namespace GameManager.GameContent
{
	public class AssetSourceController
	{
		private readonly List<IContentSource> _staticSources;

		private readonly IAssetRepository _assetRepository;

		public ResourcePackList ActiveResourcePackList
		{
			get;
			private set;
		}

		public event Action<ResourcePackList> OnResourcePackChange;

		public AssetSourceController(IAssetRepository assetRepository, IEnumerable<IContentSource> staticSources)
		{
			_assetRepository = assetRepository;
			_staticSources = staticSources.ToList();
			UseResourcePacks(new ResourcePackList());
		}

		public void Refresh()
		{
			foreach (ResourcePack allPack in ActiveResourcePackList.AllPacks)
			{
				allPack.Refresh();
			}
			UseResourcePacks(ActiveResourcePackList);
		}

		public void UseResourcePacks(ResourcePackList resourcePacks)
		{
			if (this.OnResourcePackChange != null)
			{
				this.OnResourcePackChange(resourcePacks);
			}
			ActiveResourcePackList = resourcePacks;
			List<IContentSource> list = new List<IContentSource>(from pack in resourcePacks.EnabledPacks
				orderby pack.SortingOrder
				select pack.GetContentSource());
			list.AddRange(_staticSources);
			foreach (IContentSource item in list)
			{
				item.ClearRejections();
			}
			_assetRepository.SetSources((IEnumerable<IContentSource>)list, Main.content,(AssetRequestMode)1);
		}
	}
}
