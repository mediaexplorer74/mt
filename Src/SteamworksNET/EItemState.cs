// Decompiled with JetBrains decompiler
// Type: Steamworks.EItemState
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System;

namespace Steamworks
{
  [Flags]
  public enum EItemState
  {
    k_EItemStateNone = 0,
    k_EItemStateSubscribed = 1,
    k_EItemStateLegacyItem = 2,
    k_EItemStateInstalled = 4,
    k_EItemStateNeedsUpdate = 8,
    k_EItemStateDownloading = 16, // 0x00000010
    k_EItemStateDownloadPending = 32, // 0x00000020
  }
}
