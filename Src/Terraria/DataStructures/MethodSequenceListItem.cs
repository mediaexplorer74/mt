﻿// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.MethodSequenceListItem
// Assembly: Terraria, Version=1.3.5.3, Culture=neutral, PublicKeyToken=null
// MVID: 68659D26-2BE6-448F-8663-74FA559E6F08
// Assembly location: H:\Steam\steamapps\common\Terraria\Terraria.exe

using System;
using System.Collections.Generic;

namespace Terraria.DataStructures
{
  public class MethodSequenceListItem
  {
    public string Name;
    public MethodSequenceListItem Parent;
    public Func<bool> Method;
    public bool Skip;

    public MethodSequenceListItem(string name, Func<bool> method, MethodSequenceListItem parent = null)
    {
      this.Name = name;
      this.Method = method;
      this.Parent = parent;
    }

    public bool ShouldAct(List<MethodSequenceListItem> sequence)
    {
      if (this.Skip || !sequence.Contains(this))
        return false;
      if (this.Parent != null)
        return this.Parent.ShouldAct(sequence);
      return true;
    }

    public bool Act()
    {
      return this.Method();
    }

    public static void ExecuteSequence(List<MethodSequenceListItem> sequence)
    {
      foreach (MethodSequenceListItem sequenceListItem in sequence)
      {
        if (sequenceListItem.ShouldAct(sequence) && !sequenceListItem.Act())
          break;
      }
    }

    public override string ToString()
    {
      return "name: " + this.Name + " skip: " + this.Skip.ToString() + " parent: " + (object) this.Parent;
    }
  }
}
