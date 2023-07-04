// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.CountingStream
// Assembly: Ionic.Zip.CF, Version=1.9.1.8, Culture=neutral, PublicKeyToken=edbe51ad942a3f5c
// MVID: 3D63C7F2-9FA1-46C2-9CAE-E29323A47C7A
// Assembly location: C:\Users\Admin\Desktop\re\CF.dll

using System;
using System.IO;

namespace Ionic.Zip
{
  public class CountingStream : Stream
  {
    private Stream _s;
    private long _bytesWritten;
    private long _bytesRead;
    private long _initialOffset;

    public CountingStream(Stream stream)
    {
      this._s = stream;
      try
      {
        this._initialOffset = this._s.Position;
      }
      catch
      {
        this._initialOffset = 0L;
      }
    }

    public Stream WrappedStream => this._s;

    public long BytesWritten => this._bytesWritten;

    public long BytesRead => this._bytesRead;

    public void Adjust(long delta)
    {
      this._bytesWritten -= delta;
      if (this._bytesWritten < 0L)
        throw new InvalidOperationException();
      if (!(this._s is CountingStream))
        return;
      ((CountingStream) this._s).Adjust(delta);
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      int num = this._s.Read(buffer, offset, count);
      this._bytesRead += (long) num;
      return num;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      if (count == 0)
        return;
      this._s.Write(buffer, offset, count);
      this._bytesWritten += (long) count;
    }

    public override bool CanRead => this._s.CanRead;

    public override bool CanSeek => this._s.CanSeek;

    public override bool CanWrite => this._s.CanWrite;

    public override void Flush() => this._s.Flush();

    public override long Length => this._s.Length;

    public long ComputedPosition => this._initialOffset + this._bytesWritten;

    public override long Position
    {
      get => this._s.Position;
      set
      {
        this._s.Seek(value, SeekOrigin.Begin);
        SharedUtilities.Workaround_Ladybug318918(this._s);
      }
    }

    public override long Seek(long offset, SeekOrigin origin) => this._s.Seek(offset, origin);

    public override void SetLength(long value) => this._s.SetLength(value);
  }
}
