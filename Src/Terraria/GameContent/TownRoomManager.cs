﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.TownRoomManager
// Assembly: Terraria, Version=1.3.5.3, Culture=neutral, PublicKeyToken=null
// MVID: 68659D26-2BE6-448F-8663-74FA559E6F08
// Assembly location: H:\Steam\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace Terraria.GameContent
{
  public class TownRoomManager
  {
    private List<Tuple<int, Point>> _roomLocationPairs = new List<Tuple<int, Point>>();
    private bool[] _hasRoom = new bool[580];

    public int FindOccupation(int x, int y)
    {
      return this.FindOccupation(new Point(x, y));
    }

    public int FindOccupation(Point tilePosition)
    {
      using (List<Tuple<int, Point>>.Enumerator enumerator = this._roomLocationPairs.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Tuple<int, Point> current = enumerator.Current;
          if (Point.op_Equality(current.Item2, tilePosition))
            return current.Item1;
        }
      }
      return -1;
    }

    public bool HasRoomQuick(int npcID)
    {
      return this._hasRoom[npcID];
    }

    public bool HasRoom(int npcID, out Point roomPosition)
    {
      if (!this._hasRoom[npcID])
      {
        roomPosition = new Point(0, 0);
        return false;
      }
      using (List<Tuple<int, Point>>.Enumerator enumerator = this._roomLocationPairs.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Tuple<int, Point> current = enumerator.Current;
          if (current.Item1 == npcID)
          {
            roomPosition = current.Item2;
            return true;
          }
        }
      }
      roomPosition = new Point(0, 0);
      return false;
    }

    public void SetRoom(int npcID, int x, int y)
    {
      this._hasRoom[npcID] = true;
      this.SetRoom(npcID, new Point(x, y));
    }

    public void SetRoom(int npcID, Point pt)
    {
      this._roomLocationPairs.RemoveAll((Predicate<Tuple<int, Point>>) (x => x.Item1 == npcID));
      this._roomLocationPairs.Add(Tuple.Create<int, Point>(npcID, pt));
    }

    public void KickOut(NPC n)
    {
      this.KickOut(n.type);
      this._hasRoom[n.type] = false;
    }

    public void KickOut(int npcType)
    {
      this._roomLocationPairs.RemoveAll((Predicate<Tuple<int, Point>>) (x => x.Item1 == npcType));
    }

    public void DisplayRooms()
    {
      using (List<Tuple<int, Point>>.Enumerator enumerator = this._roomLocationPairs.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Tuple<int, Point> current = enumerator.Current;
          Dust.QuickDust(current.Item2, Main.hslToRgb((float) ((double) current.Item1 * 0.0500000007450581 % 1.0), 1f, 0.5f));
        }
      }
    }

    public void Save(BinaryWriter writer)
    {
      writer.Write(this._roomLocationPairs.Count);
      using (List<Tuple<int, Point>>.Enumerator enumerator = this._roomLocationPairs.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Tuple<int, Point> current = enumerator.Current;
          writer.Write(current.Item1);
          writer.Write((int) current.Item2.X);
          writer.Write((int) current.Item2.Y);
        }
      }
    }

    public void Load(BinaryReader reader)
    {
      this.Clear();
      int num = reader.ReadInt32();
      for (int index1 = 0; index1 < num; ++index1)
      {
        int index2 = reader.ReadInt32();
        Point point;
        // ISSUE: explicit reference operation
        ((Point) @point).\u002Ector(reader.ReadInt32(), reader.ReadInt32());
        this._roomLocationPairs.Add(Tuple.Create<int, Point>(index2, point));
        this._hasRoom[index2] = true;
      }
    }

    public void Clear()
    {
      this._roomLocationPairs.Clear();
      for (int index = 0; index < this._hasRoom.Length; ++index)
        this._hasRoom[index] = false;
    }

    public byte GetHouseholdStatus(NPC n)
    {
      byte num = 0;
      if (n.homeless)
        num = (byte) 1;
      else if (this.HasRoomQuick(n.type))
        num = (byte) 2;
      return num;
    }
  }
}
