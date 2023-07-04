// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.NetCfFile
// Assembly: Ionic.Zip.CF, Version=1.9.1.8, Culture=neutral, PublicKeyToken=edbe51ad942a3f5c
// MVID: 3D63C7F2-9FA1-46C2-9CAE-E29323A47C7A
// Assembly location: C:\Users\Admin\Desktop\re\CF.dll

using System;
using System.Runtime.InteropServices;

namespace Ionic.Zip
{
  internal class NetCfFile
  {
    public static int SetTimes(string filename, DateTime ctime, DateTime atime, DateTime mtime)
    {
      IntPtr fileCe = (IntPtr) NetCfFile.CreateFileCE(filename, 1073741824U, 2U, 0, 3U, 0U, 0);
      if ((int) fileCe == -1)
        return Marshal.GetLastWin32Error();
      NetCfFile.SetFileTime(fileCe, BitConverter.GetBytes(ctime.ToFileTime()), BitConverter.GetBytes(atime.ToFileTime()), BitConverter.GetBytes(mtime.ToFileTime()));
      NetCfFile.CloseHandle(fileCe);
      return 0;
    }

    public static int SetLastWriteTime(string filename, DateTime mtime)
    {
      IntPtr fileCe = (IntPtr) NetCfFile.CreateFileCE(filename, 1073741824U, 2U, 0, 3U, 0U, 0);
      if ((int) fileCe == -1)
        return Marshal.GetLastWin32Error();
      NetCfFile.SetFileTime(fileCe, (byte[]) null, (byte[]) null, BitConverter.GetBytes(mtime.ToFileTime()));
      NetCfFile.CloseHandle(fileCe);
      return 0;
    }

    [DllImport("coredll.dll", EntryPoint = "CreateFile", SetLastError = true)]
    internal static extern int CreateFileCE(
      string lpFileName,
      uint dwDesiredAccess,
      uint dwShareMode,
      int lpSecurityAttributes,
      uint dwCreationDisposition,
      uint dwFlagsAndAttributes,
      int hTemplateFile);

    [DllImport("coredll", EntryPoint = "GetFileAttributes", SetLastError = true)]
    internal static extern uint GetAttributes(string lpFileName);

    [DllImport("coredll", EntryPoint = "SetFileAttributes", SetLastError = true)]
    internal static extern bool SetAttributes(string lpFileName, uint dwFileAttributes);

    [DllImport("coredll", SetLastError = true)]
    internal static extern bool SetFileTime(
      IntPtr hFile,
      byte[] lpCreationTime,
      byte[] lpLastAccessTime,
      byte[] lpLastWriteTime);

    [DllImport("coredll.dll", SetLastError = true)]
    internal static extern bool CloseHandle(IntPtr hObject);
  }
}
