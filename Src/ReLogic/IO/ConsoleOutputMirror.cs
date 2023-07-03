// Decompiled with JetBrains decompiler
// Type: ReLogic.IO.ConsoleOutputMirror
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

using System;
using System.IO;
using System.Text;

namespace ReLogic.IO
{
  public class ConsoleOutputMirror : IDisposable
  {
    private static ConsoleOutputMirror _instance;
    private FileStream _fileStream;
    private StreamWriter _fileWriter;
    private TextWriter _newConsoleOutput;
    private TextWriter _oldConsoleOutput;
    private bool disposedValue;

    public static void ToFile(string path)
    {
      if (ConsoleOutputMirror._instance != null)
      {
        ConsoleOutputMirror._instance.Dispose();
        ConsoleOutputMirror._instance = (ConsoleOutputMirror) null;
      }
      try
      {
        ConsoleOutputMirror._instance = new ConsoleOutputMirror(path);
      }
      catch (Exception ex)
      {
        Console.WriteLine("Unable to bind console output to file: {0}\r\nException: {1}", (object) path, (object) ex.ToString());
      }
    }

    private ConsoleOutputMirror(string path)
    {
      this._oldConsoleOutput = Console.Out;
      Directory.CreateDirectory(Directory.GetParent(path).FullName);
      this._fileStream = File.Create(path);
      this._fileWriter = new StreamWriter((Stream) this._fileStream)
      {
        AutoFlush = true
      };
      this._newConsoleOutput = (TextWriter) new ConsoleOutputMirror.DoubleWriter((TextWriter) this._fileWriter, this._oldConsoleOutput);
      Console.SetOut(this._newConsoleOutput);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposedValue)
        return;
      if (disposing)
      {
        Console.SetOut(this._oldConsoleOutput);
        if (this._fileWriter != null)
        {
          this._fileWriter.Flush();
          this._fileWriter.Close();
          this._fileWriter = (StreamWriter) null;
        }
        if (this._fileStream != null)
        {
          this._fileStream.Close();
          this._fileStream = (FileStream) null;
        }
      }
      this.disposedValue = true;
    }

    public void Dispose() => this.Dispose(true);

    private class DoubleWriter : TextWriter
    {
      private TextWriter _first;
      private TextWriter _second;

      public DoubleWriter(TextWriter first, TextWriter second)
      {
        this._first = first;
        this._second = second;
      }

      public override Encoding Encoding => this._first.Encoding;

      public override void Flush()
      {
        this._first.Flush();
        this._second.Flush();
      }

      public override void Write(char value)
      {
        this._first.Write(value);
        this._second.Write(value);
      }
    }
  }
}
