// Decompiled with JetBrains decompiler
// Type: ReLogic.Utilities.ReinterpretCast
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

using System.Runtime.InteropServices;

namespace ReLogic.Utilities
{
  public static class ReinterpretCast
  {
    public static float UIntAsFloat(uint value) => new ReinterpretCast.UIntFloat(value).FloatValue;

    public static float IntAsFloat(int value) => new ReinterpretCast.IntFloat(value).FloatValue;

    public static uint FloatAsUInt(float value) => new ReinterpretCast.UIntFloat(value).UIntValue;

    public static int FloatAsInt(float value) => new ReinterpretCast.IntFloat(value).IntValue;

    [StructLayout(LayoutKind.Explicit)]
    private struct IntFloat
    {
      [FieldOffset(0)]
      public readonly int IntValue;
      [FieldOffset(0)]
      public readonly float FloatValue;

      public IntFloat(int value)
      {
        this.FloatValue = 0.0f;
        this.IntValue = value;
      }

      public IntFloat(float value)
      {
        this.IntValue = 0;
        this.FloatValue = value;
      }
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct UIntFloat
    {
      [FieldOffset(0)]
      public readonly uint UIntValue;
      [FieldOffset(0)]
      public readonly float FloatValue;

      public UIntFloat(uint value)
      {
        this.FloatValue = 0.0f;
        this.UIntValue = value;
      }

      public UIntFloat(float value)
      {
        this.UIntValue = 0U;
        this.FloatValue = value;
      }
    }
  }
}
