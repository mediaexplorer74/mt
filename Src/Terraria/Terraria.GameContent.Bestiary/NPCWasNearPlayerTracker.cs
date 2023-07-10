using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using GameManager.GameContent.NetModules;
using GameManager.ID;
using GameManager.Net;

namespace GameManager.GameContent.Bestiary
{
	public class NPCWasNearPlayerTracker : IPersistentPerWorldContent, IOnPlayerJoining
	{
		private HashSet<string> _wasNearPlayer;

		private List<Rectangle> _playerHitboxesForBestiary;

		private List<int> _wasSeenNearPlayerByNetId;

		public void PrepareSamplesBasedOptimizations()
		{
		}

		public NPCWasNearPlayerTracker()
		{
			_wasNearPlayer = new HashSet<string>();
			_playerHitboxesForBestiary = new List<Rectangle>();
			_wasSeenNearPlayerByNetId = new List<int>();
		}

		public void RegisterWasNearby(NPC npc)
		{
			string bestiaryCreditId = npc.GetBestiaryCreditId();
			bool flag = !_wasNearPlayer.Contains(bestiaryCreditId);
			_wasNearPlayer.Add(bestiaryCreditId);
			if (Main.netMode == 2 && flag)
			{
				NetManager.Instance.Broadcast(NetBestiaryModule.SerializeSight(npc.netID));
			}
		}

		public void SetWasSeenDirectly(string persistentId)
		{
			_wasNearPlayer.Add(persistentId);
		}

		public bool GetWasNearbyBefore(NPC npc)
		{
			string bestiaryCreditId = npc.GetBestiaryCreditId();
			return GetWasNearbyBefore(bestiaryCreditId);
		}

		public bool GetWasNearbyBefore(string persistentIdentifier)
		{
			return _wasNearPlayer.Contains(persistentIdentifier);
		}

		public void Save(BinaryWriter writer)
		{
			lock (_wasNearPlayer)
			{
				writer.Write(_wasNearPlayer.Count);
				foreach (string item in _wasNearPlayer)
				{
					writer.Write(item);
				}
			}
		}

		public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn)
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string item = reader.ReadString();
				_wasNearPlayer.Add(item);
			}
		}

		public void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn)
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				reader.ReadString();
			}
		}

		public void Reset()
		{
			_wasNearPlayer.Clear();
			_playerHitboxesForBestiary.Clear();
			_wasSeenNearPlayerByNetId.Clear();
		}

		public void ScanWorldForFinds()
		{
			_playerHitboxesForBestiary.Clear();
			for (int i = 0; i < 255; i++)
			{
				Player player = Main.player[i];
				if (player.active)
				{
					Rectangle hitbox = player.Hitbox;
					hitbox.Inflate(300, 200);
					_playerHitboxesForBestiary.Add(hitbox);
				}
			}
			for (int j = 0; j < 200; j++)
			{
				NPC nPC = Main.npc[j];
				if (!nPC.active || !nPC.CountsAsACritter || _wasSeenNearPlayerByNetId.Contains(nPC.netID))
				{
					continue;
				}
				Rectangle hitbox2 = nPC.Hitbox;
				for (int k = 0; k < _playerHitboxesForBestiary.Count; k++)
				{
					Rectangle value = _playerHitboxesForBestiary[k];
					if (hitbox2.Intersects(value))
					{
						_wasSeenNearPlayerByNetId.Add(nPC.netID);
						RegisterWasNearby(nPC);
					}
				}
			}
		}

		public void OnPlayerJoining(int playerIndex)
		{
			foreach (string item in _wasNearPlayer)
			{
				int npcNetId = ContentSamples.NpcNetIdsByPersistentIds[item];
				NetManager.Instance.SendToClient(NetBestiaryModule.SerializeSight(npcNetId), playerIndex);
			}
		}
	}
}
