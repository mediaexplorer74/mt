using Microsoft.Xna.Framework;
using GameManager.Utilities;

namespace GameManager.GameContent.ItemDropRules
{
	public static class CommonCode
	{
		public static void DropItemFromNPC(NPC npc, int itemId, int stack, bool scattered = false)
		{
			if (itemId > 0 && itemId < 5045)
			{
				int x = (int)npc.position.X + npc.width / 2;
				int y = (int)npc.position.Y + npc.height / 2;
				if (scattered)
				{
					x = (int)npc.position.X + Main.rand.Next(npc.width + 1);
					y = (int)npc.position.Y + Main.rand.Next(npc.height + 1);
				}
				int itemIndex = Item.NewItem(x, y, 0, 0, itemId, stack, noBroadcast: false, -1);
				ModifyItemDropFromNPC(npc, itemIndex);
			}
		}

		public static void DropItemLocalPerClientAndSetNPCMoneyTo0(NPC npc, int itemId, int stack, bool interactionRequired = true)
		{
			if (itemId <= 0 || itemId >= 5045)
			{
				return;
			}
			if (Main.netMode == 2)
			{
				int num = Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, itemId, stack, noBroadcast: true, -1);
				Main.timeItemSlotCannotBeReusedFor[num] = 54000;
				for (int i = 0; i < 255; i++)
				{
					if (Main.player[i].active && (npc.playerInteraction[i] || !interactionRequired))
					{
						NetMessage.SendData(90, i, -1, null, num);
					}
				}
				Main.item[num].active = false;
			}
			else
			{
				DropItemFromNPC(npc, itemId, stack);
			}
			npc.value = 0f;
		}

		public static void DropItemForEachInteractingPlayerOnThePlayer(NPC npc, int itemId, UnifiedRandom rng, int dropsAtXOutOfY_TheX, int dropsAtXOutOfY_TheY, int stack = 1, bool interactionRequired = true)
		{
			if (itemId <= 0 || itemId >= 5045)
			{
				return;
			}
			if (Main.netMode == 2)
			{
				for (int i = 0; i < 255; i++)
				{
					Player player = Main.player[i];
					if (player.active && (npc.playerInteraction[i] || !interactionRequired) && rng.Next(dropsAtXOutOfY_TheY) < dropsAtXOutOfY_TheX)
					{
						int itemIndex = Item.NewItem(player.position, player.Size, itemId, stack, noBroadcast: false, -1);
						ModifyItemDropFromNPC(npc, itemIndex);
					}
				}
			}
			else if (rng.Next(dropsAtXOutOfY_TheY) < dropsAtXOutOfY_TheX)
			{
				DropItemFromNPC(npc, itemId, stack);
			}
			npc.value = 0f;
		}

		private static void ModifyItemDropFromNPC(NPC npc, int itemIndex)
		{
			Item item = Main.item[itemIndex];
			switch (item.type)
			{
			case 23:
				if (npc.type == 1 && npc.netID != -1 && npc.netID != -2 && npc.netID != -5 && npc.netID != -6)
				{
					item.color = npc.color;
					NetMessage.SendData(88, -1, -1, null, itemIndex, 1f);
				}
				break;
			case 319:
				switch (npc.netID)
				{
				case 542:
					item.color = new Color(189, 148, 96, 255);
					NetMessage.SendData(88, -1, -1, null, itemIndex, 1f);
					break;
				case 543:
					item.color = new Color(112, 85, 89, 255);
					NetMessage.SendData(88, -1, -1, null, itemIndex, 1f);
					break;
				case 544:
					item.color = new Color(145, 27, 40, 255);
					NetMessage.SendData(88, -1, -1, null, itemIndex, 1f);
					break;
				case 545:
					item.color = new Color(158, 113, 164, 255);
					NetMessage.SendData(88, -1, -1, null, itemIndex, 1f);
					break;
				}
				break;
			}
		}
	}
}
