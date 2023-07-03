// Decompiled with JetBrains decompiler
// Type: Steamworks.CallbackDispatcher
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System;

namespace Steamworks
{
  public static class CallbackDispatcher
  {
    public static void ExceptionHandler(Exception e) => Console.WriteLine(e.Message);
  }
}
