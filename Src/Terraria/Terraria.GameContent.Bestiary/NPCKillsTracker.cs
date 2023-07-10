using System.Collections.Generic;
using System.IO;
using GameManager.GameContent.NetModules;
using GameManager.ID;
using GameManager.Net;

namespace GameManager.GameContent.Bestiary
{
	public class NPCKillsTracker : IPersistentPerWorldContent, IOnPlayerJoining
	{
		public const int POSITIVE_KILL_COUNT_CAP = 9999;

		private Dictionary<string, int> _killCountsByNpcId;

		public NPCKillsTracker()
		{
			_killCountsByNpcId = new Dictionary<string, int>();
		}

		public void RegisterKill(NPC npc)
		{
			string bestiaryCreditId = npc.GetBestiaryCreditId();
			_killCountsByNpcId.TryGetValue(bestiaryCreditId, out var value);
			value++;
			_killCountsByNpcId[bestiaryCreditId] = Utils.Clamp(value, 0, 9999);
			if (Main.netMode == 2)
			{
				NetManager.Instance.Broadcast(NetBestiaryModule.SerializeKillCount(npc.netID, value));
			}
		}

		public int GetKillCount(NPC npc)
		{
			string bestiaryCreditId = npc.GetBestiaryCreditId();
			return GetKillCount(bestiaryCreditId);
		}

		public void SetKillCountDirectly(string persistentId, int killCount)
		{
			_killCountsByNpcId[persistentId] = Utils.Clamp(killCount, 0, 9999);
		}

		public int GetKillCount(string persistentId)
		{
			_killCountsByNpcId.TryGetValue(persistentId, out var value);
			return value;
		}

		public void Save(BinaryWriter writer)
		{
			lock (_killCountsByNpcId)
			{
				writer.Write(_killCountsByNpcId.Count);
				foreach (KeyValuePair<string, int> item in _killCountsByNpcId)
				{
					writer.Write(item.Key);
					writer.Write(item.Value);
				}
			}
		}

		public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn)
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string key = reader.ReadString();
				int value = reader.ReadInt32();
				_killCountsByNpcId[key] = value;
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
			_killCountsByNpcId.Clear();
		}

		public void OnPlayerJoining(int playerIndex)
		{
			foreach (KeyValuePair<string, int> item in _killCountsByNpcId)
			{
				int npcNetId = ContentSamples.NpcNetIdsByPersistentIds[item.Key];
				NetManager.Instance.SendToClient(NetBestiaryModule.SerializeKillCount(npcNetId, item.Value), playerIndex);
			}
		}
	}
}
