using System;
using System.Collections.Generic;
using Steamworks;
using GameManager.Social.Base;

namespace GameManager.Social.Steam
{
	public class CloudSocialModule : GameManager.Social.Base.CloudSocialModule
	{
		private const uint WRITE_CHUNK_SIZE = 1024u;

		private object ioLock = new object();

		private byte[] writeBuffer = new byte[1024];

		public override void Initialize()
		{
			base.Initialize();
		}

		public override void Shutdown()
		{
		}

		public override IEnumerable<string> GetFiles()
		{
			lock (ioLock)
			{
				int fileCount = SteamRemoteStorage.GetFileCount();
				List<string> list = new List<string>(fileCount);
				int num = default(int);
				for (int i = 0; i < fileCount; i++)
				{
					list.Add(SteamRemoteStorage.GetFileNameAndSize(i, out num));
				}
				return list;
			}
		}

		public override bool Write(string path, byte[] data, int length)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			lock (ioLock)
			{
				UGCFileWriteStreamHandle_t val = SteamRemoteStorage.FileWriteStreamOpen(path);
				for (uint num = 0u; num < length; num += 1024)
				{
					int num2 = (int)Math.Min(1024L, length - num);
					Array.Copy(data, num, writeBuffer, 0L, num2);
					SteamRemoteStorage.FileWriteStreamWriteChunk(val, writeBuffer, num2);
				}
				return SteamRemoteStorage.FileWriteStreamClose(val);
			}
		}

		public override int GetFileSize(string path)
		{
			lock (ioLock)
			{
				return SteamRemoteStorage.GetFileSize(path);
			}
		}

		public override void Read(string path, byte[] buffer, int size)
		{
			lock (ioLock)
			{
				SteamRemoteStorage.FileRead(path, buffer, size);
			}
		}

		public override bool HasFile(string path)
		{
			lock (ioLock)
			{
				return SteamRemoteStorage.FileExists(path);
			}
		}

		public override bool Delete(string path)
		{
			lock (ioLock)
			{
				return SteamRemoteStorage.FileDelete(path);
			}
		}
	}
}
