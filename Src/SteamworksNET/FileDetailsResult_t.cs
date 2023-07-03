// Decompiled with JetBrains decompiler
// Type: Steamworks.FileDetailsResult_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1023)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct FileDetailsResult_t
  {
    public const int k_iCallback = 1023;
    public EResult m_eResult;
    public ulong m_ulFileSize;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
    public byte[] m_FileSHA;
    public uint m_unFlags;
  }
}
