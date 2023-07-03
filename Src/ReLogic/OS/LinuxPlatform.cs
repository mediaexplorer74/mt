// Decompiled with JetBrains decompiler
// Type: ReLogic.OS.LinuxPlatform
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

using System;
using System.Diagnostics;

namespace ReLogic.OS
{
  internal class LinuxPlatform : FnaPlatform
  {
    public LinuxPlatform()
      : base(PlatformType.Linux)
    {
    }

    protected override string GetClipboard()
    {
      try
      {
        string end;
        using (Process process = new Process())
        {
          process.StartInfo = new ProcessStartInfo("xsel", "-o")
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

    private void ClearClipboard()
    {
      try
      {
        using (Process process = new Process())
        {
          process.StartInfo = new ProcessStartInfo("xsel", "-c")
          {
            UseShellExecute = false,
            RedirectStandardOutput = false,
            RedirectStandardInput = true
          };
          process.Start();
          process.WaitForExit();
        }
      }
      catch (Exception ex)
      {
      }
    }

    protected override void SetClipboard(string text)
    {
      if (text == "")
      {
        this.ClearClipboard();
      }
      else
      {
        try
        {
          using (Process process = new Process())
          {
            process.StartInfo = new ProcessStartInfo("xsel", "-i")
            {
              UseShellExecute = false,
              RedirectStandardOutput = false,
              RedirectStandardInput = true
            };
            process.Start();
            process.StandardInput.Write(text);
            process.StandardInput.Close();
            process.WaitForExit();
          }
        }
        catch (Exception ex)
        {
        }
      }
    }

    public override string GetStoragePath()
    {
      string storagePath = Environment.GetEnvironmentVariable("XDG_DATA_HOME");
      if (string.IsNullOrEmpty(storagePath))
      {
        string environmentVariable = Environment.GetEnvironmentVariable("HOME");
        if (string.IsNullOrEmpty(environmentVariable))
          return ".";
        storagePath = environmentVariable + "/.local/share";
      }
      return storagePath;
    }
  }
}
