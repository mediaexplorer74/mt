﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Cinematics.FrameEventData
// Assembly: Terraria, Version=1.3.5.3, Culture=neutral, PublicKeyToken=null
// MVID: 68659D26-2BE6-448F-8663-74FA559E6F08
// Assembly location: H:\Steam\steamapps\common\Terraria\Terraria.exe

namespace Terraria.Cinematics
{
  public struct FrameEventData
  {
    private int _absoluteFrame;
    private int _start;
    private int _duration;

    public int AbsoluteFrame
    {
      get
      {
        return this._absoluteFrame;
      }
    }

    public int Start
    {
      get
      {
        return this._start;
      }
    }

    public int Duration
    {
      get
      {
        return this._duration;
      }
    }

    public int Frame
    {
      get
      {
        return this._absoluteFrame - this._start;
      }
    }

    public bool IsFirstFrame
    {
      get
      {
        return this._start == this._absoluteFrame;
      }
    }

    public bool IsLastFrame
    {
      get
      {
        return this.Remaining == 0;
      }
    }

    public int Remaining
    {
      get
      {
        return this._start + this._duration - this._absoluteFrame - 1;
      }
    }

    public FrameEventData(int absoluteFrame, int start, int duration)
    {
      this._absoluteFrame = absoluteFrame;
      this._start = start;
      this._duration = duration;
    }
  }
}
