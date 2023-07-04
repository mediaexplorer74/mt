﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.BufferUtils
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

namespace Newtonsoft.Json.Utilities
{
  internal static class BufferUtils
  {
    public static char[] RentBuffer(IArrayPool<char> bufferPool, int minSize) => bufferPool == null ? new char[minSize] : bufferPool.Rent(minSize);

    public static void ReturnBuffer(IArrayPool<char> bufferPool, char[] buffer) => bufferPool?.Return(buffer);

    public static char[] EnsureBufferSize(IArrayPool<char> bufferPool, int size, char[] buffer)
    {
      if (bufferPool == null)
        return new char[size];
      if (buffer != null)
        bufferPool.Return(buffer);
      return bufferPool.Rent(size);
    }
  }
}
