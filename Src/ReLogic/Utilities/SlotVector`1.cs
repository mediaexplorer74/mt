﻿// Decompiled with JetBrains decompiler
// Type: ReLogic.Utilities.SlotVector`1
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace ReLogic.Utilities
{
  public class SlotVector<T> : IEnumerable<SlotVector<T>.ItemPair>, IEnumerable
  {
    private const uint MAX_INDEX = 65535;
    private SlotVector<T>.ItemPair[] _array;
    private uint _freeHead;
    private uint _usedSpaceLength;
    private int _count;

    public T this[int index]
    {
      get
      {
        if (index < 0 || index >= this._array.Length)
          throw new IndexOutOfRangeException();
        if (!this._array[index].Id.IsActive)
          throw new KeyNotFoundException();
        return this._array[index].Value;
      }
      set
      {
        if (index < 0 || index >= this._array.Length)
          throw new IndexOutOfRangeException();
        if (!this._array[index].Id.IsActive)
          throw new KeyNotFoundException();
        this._array[index] = new SlotVector<T>.ItemPair(value, this._array[index].Id);
      }
    }

    public T this[SlotId id]
    {
      get
      {
        uint index = id.Index;
        if (index < 0U || (long) index >= (long) this._array.Length)
          throw new IndexOutOfRangeException();
        if (!this._array[(int) index].Id.IsActive || id != this._array[(int) index].Id)
          throw new KeyNotFoundException();
        return this._array[(int) index].Value;
      }
      set
      {
        uint index = id.Index;
        if (index < 0U || (long) index >= (long) this._array.Length)
          throw new IndexOutOfRangeException();
        if (!this._array[(int) index].Id.IsActive || id != this._array[(int) index].Id)
          throw new KeyNotFoundException();
        this._array[(int) index] = new SlotVector<T>.ItemPair(value, id);
      }
    }

    public int Count => this._count;

    public int Capacity => this._array.Length;

    public SlotVector(int capacity)
    {
      this._array = new SlotVector<T>.ItemPair[capacity];
      this.Clear();
    }

    public SlotId Add(T value)
    {
      if (this._freeHead == (uint) ushort.MaxValue)
        return new SlotId((uint) ushort.MaxValue);
      uint freeHead = this._freeHead;
      SlotVector<T>.ItemPair itemPair = this._array[(int) freeHead];
      if (this._freeHead >= this._usedSpaceLength)
        this._usedSpaceLength = this._freeHead + 1U;
      this._freeHead = itemPair.Id.Index;
      this._array[(int) freeHead] = new SlotVector<T>.ItemPair(value, itemPair.Id.ToActive(freeHead));
      ++this._count;
      return this._array[(int) freeHead].Id;
    }

    public void Clear()
    {
      this._usedSpaceLength = 0U;
      this._count = 0;
      this._freeHead = 0U;
      for (uint index = 0; (long) index < (long) (this._array.Length - 1); ++index)
        this._array[(int) index] = new SlotVector<T>.ItemPair(default (T), new SlotId(index + 1U));
      this._array[this._array.Length - 1] = new SlotVector<T>.ItemPair(default (T), new SlotId((uint) ushort.MaxValue));
    }

    public bool Remove(SlotId id)
    {
      if (!id.IsActive)
        return false;
      uint index = id.Index;
      this._array[(int) index] = new SlotVector<T>.ItemPair(default (T), id.ToInactive(this._freeHead));
      this._freeHead = index;
      --this._count;
      return true;
    }

    public bool Has(SlotId id)
    {
      uint index = id.Index;
      return index >= 0U && (long) index < (long) this._array.Length && this._array[(int) index].Id.IsActive && !(id != this._array[(int) index].Id);
    }

    public bool Has(int index) => index >= 0 && index < this._array.Length && this._array[index].Id.IsActive;

    public SlotVector<T>.ItemPair GetPair(int index) => this.Has(index) ? this._array[index] : new SlotVector<T>.ItemPair(default (T), SlotId.Invalid);

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) new SlotVector<T>.Enumerator(this);

    IEnumerator<SlotVector<T>.ItemPair> IEnumerable<SlotVector<T>.ItemPair>.GetEnumerator() => (IEnumerator<SlotVector<T>.ItemPair>) new SlotVector<T>.Enumerator(this);

    public class Enumerator : IEnumerator<SlotVector<T>.ItemPair>, IDisposable, IEnumerator
    {
      private SlotVector<T> _slotVector;
      private int _index = -1;

      public Enumerator(SlotVector<T> slotVector) => this._slotVector = slotVector;

      SlotVector<T>.ItemPair IEnumerator<SlotVector<T>.ItemPair>.Current => this._slotVector.GetPair(this._index);

      object IEnumerator.Current => (object) this._slotVector.GetPair(this._index);

      public void Reset() => this._index = -1;

      public bool MoveNext()
      {
        while ((long) ++this._index < (long) this._slotVector._usedSpaceLength)
        {
          if (this._slotVector.Has(this._index))
            return true;
        }
        return false;
      }

      void IDisposable.Dispose() => this._slotVector = (SlotVector<T>) null;
    }

    public struct ItemPair
    {
      public readonly T Value;
      public readonly SlotId Id;

      public ItemPair(T value, SlotId id)
      {
        this.Value = value;
        this.Id = id;
      }
    }
  }
}
