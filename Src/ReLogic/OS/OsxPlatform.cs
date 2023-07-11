// Decompiled with JetBrains decompiler
// Type: ReLogic.OS.OsxPlatform
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

using System;
using System.Diagnostics;

namespace ReLogic.OS
{
  internal class OsxPlatform : FnaPlatform
  {
    public OsxPlatform()
      : base(PlatformType.OSX)
    {
    }

    protected override string GetClipboard()
    {
      try
      {
        string end;
        using (Process process = new Process())
        {
          process.StartInfo = new ProcessStartInfo("pbpaste", "-pboard general")
          {
            UseShellExecute = false,
            RedirectStandardOutput = true
          };
          process.Start();
          end = process.StandardOutput.ReadToEnd();
          process.WaitForExit();
        }
        return end;
      }
      catch (Exception ex)
      {
        return "";
      }
    }

    protected override void SetClipboard(string text)
    {
      try
      {
                
        using (Process process = new Process())
        {
                /*
          process.StartInfo = new ProcessStartInfo("pbcopy", "-pboard general -Prefer txt")
          {
            UseShellExecute = false,
            RedirectStandardOutput = false,
            RedirectStandardInput = true
          };
          process.Start();
          process.StandardInput.Write(text);
          process.StandardInput.Close();
          process.WaitForExit();
                */
        }
      }
      catch (Exception ex)
      {
      }
    }

    public override string GetStoragePath()
    {
      string environmentVariable = Environment.GetEnvironmentVariable("HOME");
      return string.IsNullOrEmpty(environmentVariable) 
                ? "." 
                : environmentVariable + "/Library/Application Support";
    }
  }
}
