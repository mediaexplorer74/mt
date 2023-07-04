﻿// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ZipOutputStream
// Assembly: Ionic.Zip.CF, Version=1.9.1.8, Culture=neutral, PublicKeyToken=edbe51ad942a3f5c
// MVID: 3D63C7F2-9FA1-46C2-9CAE-E29323A47C7A
// Assembly location: C:\Users\Admin\Desktop\re\CF.dll

using Ionic.Crc;
using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ionic.Zip
{
  public class ZipOutputStream : Stream
  {
    private EncryptionAlgorithm _encryption;
    private ZipEntryTimestamp _timestamp;
    internal string _password;
    private string _comment;
    private Stream _outputStream;
    private ZipEntry _currentEntry;
    internal Zip64Option _zip64;
    private Dictionary<string, ZipEntry> _entriesWritten;
    private int _entryCount;
    private ZipOption _alternateEncodingUsage;
    private Encoding _alternateEncoding = Encoding.GetEncoding("IBM437");
    private bool _leaveUnderlyingStreamOpen;
    private bool _disposed;
    private bool _exceptionPending;
    private bool _anyEntriesUsedZip64;
    private bool _directoryNeededZip64;
    private CountingStream _outputCounter;
    private Stream _encryptor;
    private Stream _deflater;
    private CrcCalculatorStream _entryOutputStream;
    private bool _needToWriteEntryHeader;
    private string _name;
    private bool _DontIgnoreCase;
    private int \u003CCodecBufferSize\u003Ek__BackingField;
    private CompressionStrategy \u003CStrategy\u003Ek__BackingField;
    private CompressionLevel \u003CCompressionLevel\u003Ek__BackingField;
    private CompressionMethod \u003CCompressionMethod\u003Ek__BackingField;

    public ZipOutputStream(Stream stream)
      : this(stream, false)
    {
    }

    public ZipOutputStream(string fileName) => this._Init((Stream) File.Open(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.None), false, fileName);

    public ZipOutputStream(Stream stream, bool leaveOpen) => this._Init(stream, leaveOpen, (string) null);

    private void _Init(Stream stream, bool leaveOpen, string name)
    {
      this._outputStream = stream.CanRead ? stream : (Stream) new CountingStream(stream);
      this.CompressionLevel = CompressionLevel.Default;
      this.CompressionMethod = CompressionMethod.Deflate;
      this._encryption = EncryptionAlgorithm.None;
      this._entriesWritten = new Dictionary<string, ZipEntry>((IEqualityComparer<string>) StringComparer.Ordinal);
      this._zip64 = Zip64Option.Default;
      this._leaveUnderlyingStreamOpen = leaveOpen;
      this.Strategy = CompressionStrategy.Default;
      this._name = name ?? "(stream)";
    }

    public override string ToString() => string.Format("ZipOutputStream::{0}(leaveOpen({1})))", (object) this._name, (object) this._leaveUnderlyingStreamOpen);

    public string Password
    {
      set
      {
        if (this._disposed)
        {
          this._exceptionPending = true;
          throw new InvalidOperationException("The stream has been closed.");
        }
        this._password = value;
        if (this._password == null)
        {
          this._encryption = EncryptionAlgorithm.None;
        }
        else
        {
          if (this._encryption != EncryptionAlgorithm.None)
            return;
          this._encryption = EncryptionAlgorithm.PkzipWeak;
        }
      }
    }

    public EncryptionAlgorithm Encryption
    {
      get => this._encryption;
      set
      {
        if (this._disposed)
        {
          this._exceptionPending = true;
          throw new InvalidOperationException("The stream has been closed.");
        }
        if (value == EncryptionAlgorithm.Unsupported)
        {
          this._exceptionPending = true;
          throw new InvalidOperationException("You may not set Encryption to that value.");
        }
        this._encryption = value;
      }
    }

    public int CodecBufferSize
    {
      get => this.\u003CCodecBufferSize\u003Ek__BackingField;
      set => this.\u003CCodecBufferSize\u003Ek__BackingField = value;
    }

    public CompressionStrategy Strategy
    {
      get => this.\u003CStrategy\u003Ek__BackingField;
      set => this.\u003CStrategy\u003Ek__BackingField = value;
    }

    public ZipEntryTimestamp Timestamp
    {
      get => this._timestamp;
      set
      {
        if (this._disposed)
        {
          this._exceptionPending = true;
          throw new InvalidOperationException("The stream has been closed.");
        }
        this._timestamp = value;
      }
    }

    public CompressionLevel CompressionLevel
    {
      get => this.\u003CCompressionLevel\u003Ek__BackingField;
      set => this.\u003CCompressionLevel\u003Ek__BackingField = value;
    }

    public CompressionMethod CompressionMethod
    {
      get => this.\u003CCompressionMethod\u003Ek__BackingField;
      set => this.\u003CCompressionMethod\u003Ek__BackingField = value;
    }

    public string Comment
    {
      get => this._comment;
      set
      {
        if (this._disposed)
        {
          this._exceptionPending = true;
          throw new InvalidOperationException("The stream has been closed.");
        }
        this._comment = value;
      }
    }

    public Zip64Option EnableZip64
    {
      get => this._zip64;
      set
      {
        if (this._disposed)
        {
          this._exceptionPending = true;
          throw new InvalidOperationException("The stream has been closed.");
        }
        this._zip64 = value;
      }
    }

    public bool OutputUsedZip64 => this._anyEntriesUsedZip64 || this._directoryNeededZip64;

    public bool IgnoreCase
    {
      get => !this._DontIgnoreCase;
      set => this._DontIgnoreCase = !value;
    }

    [Obsolete("Beginning with v1.9.1.6 of DotNetZip, this property is obsolete. It will be removed in a future version of the library. Use AlternateEncoding and AlternateEncodingUsage instead.")]
    public bool UseUnicodeAsNecessary
    {
      get => this._alternateEncoding == Encoding.UTF8 && this.AlternateEncodingUsage == ZipOption.AsNecessary;
      set
      {
        if (value)
        {
          this._alternateEncoding = Encoding.UTF8;
          this._alternateEncodingUsage = ZipOption.AsNecessary;
        }
        else
        {
          this._alternateEncoding = ZipOutputStream.DefaultEncoding;
          this._alternateEncodingUsage = ZipOption.Default;
        }
      }
    }

    [Obsolete("use AlternateEncoding and AlternateEncodingUsage instead.")]
    public Encoding ProvisionalAlternateEncoding
    {
      get => this._alternateEncodingUsage == ZipOption.AsNecessary ? this._alternateEncoding : (Encoding) null;
      set
      {
        this._alternateEncoding = value;
        this._alternateEncodingUsage = ZipOption.AsNecessary;
      }
    }

    public Encoding AlternateEncoding
    {
      get => this._alternateEncoding;
      set => this._alternateEncoding = value;
    }

    public ZipOption AlternateEncodingUsage
    {
      get => this._alternateEncodingUsage;
      set => this._alternateEncodingUsage = value;
    }

    public static Encoding DefaultEncoding => Encoding.GetEncoding("IBM437");

    private void InsureUniqueEntry(ZipEntry ze1)
    {
      if (this._entriesWritten.ContainsKey(ze1.FileName))
      {
        this._exceptionPending = true;
        throw new ArgumentException(string.Format("The entry '{0}' already exists in the zip archive.", (object) ze1.FileName));
      }
    }

    internal Stream OutputStream => this._outputStream;

    internal string Name => this._name;

    public bool ContainsEntry(string name) => this._entriesWritten.ContainsKey(SharedUtilities.NormalizePathForUseInZipFile(name));

    public override void Write(byte[] buffer, int offset, int count)
    {
      if (this._disposed)
      {
        this._exceptionPending = true;
        throw new InvalidOperationException("The stream has been closed.");
      }
      if (buffer == null)
      {
        this._exceptionPending = true;
        throw new ArgumentNullException(nameof (buffer));
      }
      if (this._currentEntry == null)
      {
        this._exceptionPending = true;
        throw new InvalidOperationException("You must call PutNextEntry() before calling Write().");
      }
      if (this._currentEntry.IsDirectory)
      {
        this._exceptionPending = true;
        throw new InvalidOperationException("You cannot Write() data for an entry that is a directory.");
      }
      if (this._needToWriteEntryHeader)
        this._InitiateCurrentEntry(false);
      if (count == 0)
        return;
      this._entryOutputStream.Write(buffer, offset, count);
    }

    public ZipEntry PutNextEntry(string entryName)
    {
      if (string.IsNullOrEmpty(entryName))
        throw new ArgumentNullException(nameof (entryName));
      if (this._disposed)
      {
        this._exceptionPending = true;
        throw new InvalidOperationException("The stream has been closed.");
      }
      this._FinishCurrentEntry();
      this._currentEntry = ZipEntry.CreateForZipOutputStream(entryName);
      this._currentEntry._container = new ZipContainer((object) this);
      this._currentEntry._BitField |= (short) 8;
      this._currentEntry.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
      this._currentEntry.CompressionLevel = this.CompressionLevel;
      this._currentEntry.CompressionMethod = this.CompressionMethod;
      this._currentEntry.Password = this._password;
      this._currentEntry.Encryption = this.Encryption;
      this._currentEntry.AlternateEncoding = this.AlternateEncoding;
      this._currentEntry.AlternateEncodingUsage = this.AlternateEncodingUsage;
      if (entryName.EndsWith("/"))
        this._currentEntry.MarkAsDirectory();
      this._currentEntry.EmitTimesInWindowsFormatWhenSaving = (this._timestamp & ZipEntryTimestamp.Windows) != ZipEntryTimestamp.None;
      this._currentEntry.EmitTimesInUnixFormatWhenSaving = (this._timestamp & ZipEntryTimestamp.Unix) != ZipEntryTimestamp.None;
      this.InsureUniqueEntry(this._currentEntry);
      this._needToWriteEntryHeader = true;
      return this._currentEntry;
    }

    private void _InitiateCurrentEntry(bool finishing)
    {
      this._entriesWritten.Add(this._currentEntry.FileName, this._currentEntry);
      ++this._entryCount;
      if (this._entryCount > 65534 && this._zip64 == Zip64Option.Default)
      {
        this._exceptionPending = true;
        throw new InvalidOperationException("Too many entries. Consider setting ZipOutputStream.EnableZip64.");
      }
      this._currentEntry.WriteHeader(this._outputStream, finishing ? 99 : 0);
      this._currentEntry.StoreRelativeOffset();
      if (!this._currentEntry.IsDirectory)
      {
        this._currentEntry.WriteSecurityMetadata(this._outputStream);
        this._currentEntry.PrepOutputStream(this._outputStream, finishing ? 0L : -1L, out this._outputCounter, out this._encryptor, out this._deflater, out this._entryOutputStream);
      }
      this._needToWriteEntryHeader = false;
    }

    private void _FinishCurrentEntry()
    {
      if (this._currentEntry == null)
        return;
      if (this._needToWriteEntryHeader)
        this._InitiateCurrentEntry(true);
      this._currentEntry.FinishOutputStream(this._outputStream, this._outputCounter, this._encryptor, this._deflater, this._entryOutputStream);
      this._currentEntry.PostProcessOutput(this._outputStream);
      if (this._currentEntry.OutputUsedZip64.HasValue)
        this._anyEntriesUsedZip64 |= this._currentEntry.OutputUsedZip64.Value;
      this._outputCounter = (CountingStream) null;
      this._encryptor = this._deflater = (Stream) null;
      this._entryOutputStream = (CrcCalculatorStream) null;
    }

    protected override void Dispose(bool disposing)
    {
      if (this._disposed)
        return;
      if (disposing && !this._exceptionPending)
      {
        this._FinishCurrentEntry();
        this._directoryNeededZip64 = ZipOutput.WriteCentralDirectoryStructure(this._outputStream, (ICollection<ZipEntry>) this._entriesWritten.Values, 1U, this._zip64, this.Comment, new ZipContainer((object) this));
        Stream stream;
        if (this._outputStream is CountingStream outputStream)
        {
          stream = outputStream.WrappedStream;
          outputStream.Close();
        }
        else
          stream = this._outputStream;
        if (!this._leaveUnderlyingStreamOpen)
          stream.Close();
        this._outputStream = (Stream) null;
      }
      this._disposed = true;
    }

    public override bool CanRead => false;

    public override bool CanSeek => false;

    public override bool CanWrite => true;

    public override long Length => throw new NotSupportedException();

    public override long Position
    {
      get => this._outputStream.Position;
      set => throw new NotSupportedException();
    }

    public override void Flush()
    {
    }

    public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException(nameof (Read));

    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException(nameof (Seek));

    public override void SetLength(long value) => throw new NotSupportedException();
  }
}
