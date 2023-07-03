// Decompiled with JetBrains decompiler
// Type: ReLogic.OS.Windows.STATaskInvoker
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

using System;
using System.Collections.Concurrent;
using System.Threading;

namespace ReLogic.OS.Windows
{
  internal class STATaskInvoker : IDisposable
  {
    private static STATaskInvoker Instance;
    private Thread _staThread;
    private volatile bool _shouldThreadContinue;
    private BlockingCollection<Action> _staTasks = new BlockingCollection<Action>();
    private object _taskInvokeLock = new object();
    private object _taskCompletionLock = new object();
    private bool disposedValue;

    private STATaskInvoker()
    {
      this._shouldThreadContinue = true;
      this._staThread = new Thread(new ThreadStart(this.TaskThreadStart));
      this._staThread.Name = "STA Invoker Thread";
      this._staThread.SetApartmentState(ApartmentState.STA);
      this._staThread.Start();
    }

    public static void Invoke(Action action)
    {
      if (STATaskInvoker.Instance == null)
        STATaskInvoker.Instance = new STATaskInvoker();
      STATaskInvoker.Instance.InvokeAndWait(action);
    }

    public static T Invoke<T>(Func<T> action)
    {
      if (STATaskInvoker.Instance == null)
        STATaskInvoker.Instance = new STATaskInvoker();
      T output = default (T);
      STATaskInvoker.Instance.InvokeAndWait((Action) (() => output = action()));
      return output;
    }

    private void InvokeAndWait(Action action)
    {
      lock (this._taskInvokeLock)
      {
        lock (this._taskCompletionLock)
        {
          this._staTasks.Add(action);
          Monitor.Wait(this._taskCompletionLock);
        }
      }
    }

    private void TaskThreadStart()
    {
      while (this._shouldThreadContinue)
      {
        Action action = this._staTasks.Take();
        lock (this._taskCompletionLock)
        {
          action();
          Monitor.Pulse(this._taskCompletionLock);
        }
      }
    }

    private void Shutdown() => this.InvokeAndWait((Action) (() => this._shouldThreadContinue = false));

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposedValue)
        return;
      if (disposing)
        this.Shutdown();
      this.disposedValue = true;
    }

    public void Dispose() => this.Dispose(true);
  }
}
