// Decompiled with JetBrains decompiler
// Type: ReLogic.Utilities.SlotId
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

namespace ReLogic.Utilities
{
  public struct SlotId
  {
    public static readonly SlotId Invalid = new SlotId((uint) ushort.MaxValue);
    private const uint KEY_INC = 65536;
    private const uint INDEX_MASK = 65535;
    private const uint ACTIVE_MASK = 2147483648;
    private const uint KEY_MASK = 2147418112;
    public readonly uint Value;

    public bool IsValid => ((int) this.Value & (int) ushort.MaxValue) != (int) ushort.MaxValue;

    internal bool IsActive => ((int) this.Value & int.MinValue) != 0 && this.IsValid;

    internal uint Index => this.Value & (uint) ushort.MaxValue;

    internal uint Key => this.Value & 2147418112U;

    internal SlotId ToInactive(uint freeHead) => new SlotId(this.Key | freeHead);

    internal SlotId ToActive(uint index) => new SlotId((uint) (int.MinValue | 2147418112 & (int) this.Key + 65536) | index);

    public SlotId(uint value) => this.Value = value;

    public override bool Equals(object obj) => obj is SlotId slotId && (int) slotId.Value == (int) this.Value;

    public override int GetHashCode() => this.Value.GetHashCode();

    public static bool operator ==(SlotId lhs, SlotId rhs) => (int) lhs.Value == (int) rhs.Value;

    public static bool operator !=(SlotId lhs, SlotId rhs) => (int) lhs.Value != (int) rhs.Value;

    public float ToFloat() => ReinterpretCast.UIntAsFloat(this.Value);

    public static SlotId FromFloat(float value) => new SlotId(ReinterpretCast.FloatAsUInt(value));
  }
}
