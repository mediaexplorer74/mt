// Decompiled with JetBrains decompiler
// Type: ReLogic.OS.Platform
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

using Microsoft.Xna.Framework;
using ReLogic.Localization.IME;
using System;
using System.IO;
using System.Text;

namespace ReLogic.OS
{
  public abstract class Platform
  {
    public static readonly Platform Current = (Platform) new WindowsPlatform();
    public readonly PlatformType Type;

    public static bool IsWindows => Platform.Current.Type == PlatformType.Windows;

    public static bool IsOSX => Platform.Current.Type == PlatformType.OSX;

    public static bool IsLinux => Platform.Current.Type == PlatformType.Linux;

    public PlatformIme Ime { get; private set; }

    public string Clipboard
    {
      get => Platform.SanitizeClipboardText(this.GetClipboard());
      set => this.SetClipboard(value);
    }

    public Platform(PlatformType type)
    {
      this.Type = type;
      this.Ime = (PlatformIme) new UnsupportedPlatformIme();
    }

    public void InitializeIme(IntPtr windowHandle)
    {
      if (this.Ime != null)
        this.Ime.Dispose();
      this.Ime = this.CreateIme(windowHandle);
    }

    protected abstract PlatformIme CreateIme(IntPtr windowHandle);

    public string GetStoragePath(string subfolder) => Path.Combine(this.GetStoragePath(), subfolder);

    public abstract string GetStoragePath();

    protected abstract string GetClipboard();

    protected abstract void SetClipboard(string text);

    private static string SanitizeClipboardText(string clipboardText)
    {
      StringBuilder stringBuilder = new StringBuilder(clipboardText.Length);
      for (int index = 0; index < clipboardText.Length; ++index)
      {
        if (clipboardText[index] >= ' ' && clipboardText[index] != '\u007F')
          stringBuilder.Append(clipboardText[index]);
      }
      return stringBuilder.ToString();
    }

    public virtual void SetWindowUnicodeTitle(GameWindow window, string title) => window.Title = title;
  }
}
