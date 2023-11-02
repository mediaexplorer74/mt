// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.IO;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Utilities;
using SharpDX.MediaFoundation;
using SharpDX.Win32;

namespace Microsoft.Xna.Framework.Audio
{
    /// <summary>Represents a collection of wave files.</summary>
    public class WaveBank : IDisposable
    {
        private readonly SoundEffect[] _sounds;
        private readonly StreamInfo[] _streams;
        private readonly string _bankName;
        private readonly string _waveBankFileName;
        private readonly bool _streaming;
        private readonly int _offset;
        private readonly int _packetSize;

        private readonly int _version;
        private readonly int _playRegionOffset;

        // ---------------------------------------------------------
        internal uint waveBankHandle = uint.MaxValue;
        internal uint engineHandle;
        internal FileStream pFileStream;
        internal IntPtr pCleanupData;
        internal AudioEngine parent;
        private bool _isDisposed;
        // ---------------------------------------------------------
       

        struct Segment
        {
            public int Offset;
            public int Length;
        }

        struct WaveBankEntry
        {
            public int Format;
            public Segment PlayRegion;
            public Segment LoopRegion;
            public int FlagsAndDuration;
        }

        struct WaveBankHeader
        {
            public int Version;
            public Segment[] Segments;
        }

        struct WaveBankData
        {
            public int    Flags;                                // Bank flags
            public int    EntryCount;                           // Number of entries in the bank
            public string BankName;                             // Bank friendly name
            public int    EntryMetaDataElementSize;             // Size of each entry meta-data element, in bytes
            public int    EntryNameElementSize;                 // Size of each entry name element, in bytes
            public int    Alignment;                            // Entry alignment, in bytes
            public int    CompactFormat;                        // Format data for compact bank
            public int    BuildTime;                            // Build timestamp
        }

        struct StreamInfo
        {
            public int Format;
            public int FileOffset;
            public int FileLength;
            public int LoopStart;
            public int LoopLength;
        }

        private const int Flag_EntryNames = 0x00010000; // Bank includes entry names
        private const int Flag_Compact = 0x00020000; // Bank uses compact format
        private const int Flag_SyncDisabled = 0x00040000; // Bank is disabled for audition sync
        private const int Flag_SeekTables = 0x00080000; // Bank includes seek tables.
        private const int Flag_Mask = 0x000F0000;
        
        private const int MiniFormatTag_PCM = 0x0;
        private const int MiniFormatTag_XMA = 0x1;
        private const int MiniFormatTag_ADPCM = 0x2;
        private const int MiniForamtTag_WMA = 0x3;


        /// <summary>
        /// </summary>
        public bool IsInUse { get; private set; }

        /// <summary>
        /// </summary>
        public bool IsPrepared { get; private set; }
        public bool IsDisposed { get; private set; }

        // RnD
        // CheckWaveBankHeader
        private static void CheckWaveBankHeader(string filename)
        {
            using (BinaryReader binaryReader = new BinaryReader((Stream)File.OpenRead(filename)))
            {
                bool flag = false;
                if (binaryReader.BaseStream.Length > 4L)
                {
                    byte[] numArray = binaryReader.ReadBytes(4);
                    flag = numArray[3] == (byte)68 && numArray[2] == (byte)78 
                        && numArray[1] == (byte)66 && numArray[0] == (byte)87;
                }
                if (!flag)
                {
                    Debug.WriteLine("WaveBank - CheckWaveBankHeader error: " +
                        "InvalidContentVersion");
                    throw new ArgumentNullException("InvalidContentVersion");
                }
            }
        }//CheckWaveBankHeader

     
        /// <param name="audioEngine">Instance of the AudioEngine to associate this wave bank with.</param>
        /// <param name="nonStreamingWaveBankFilename">Path to the .xwb file to load.</param>
        /// <remarks>This constructor immediately loads all wave data into memory at once.</remarks>
        public WaveBank(AudioEngine audioEngine, string nonStreamingWaveBankFilename)
        : this(audioEngine, nonStreamingWaveBankFilename, false, 0, 0)
        {
        }



        // WaveBank
        private WaveBank(AudioEngine audioEngine, string waveBankFilename, 
            bool streaming, 
            int offset, 
            int packetsize)
        {
            if (audioEngine == null)
                throw new ArgumentNullException("audioEngine");

            if (string.IsNullOrEmpty(waveBankFilename))
                throw new ArgumentNullException("nonStreamingWaveBankFilename");

           // Is this a streaming wavebank?
            if (streaming)
            {
                if (offset != 0)
                    throw new ArgumentException("We only support a zero offset in streaming banks.", 
                        "offset");

                if (packetsize < 2)
                    throw new ArgumentException("The packet size must be greater than 2.", 
                        "packetsize");

                _streaming = true;
                _offset = offset;
                _packetSize = packetsize;
            }

            //XWB PARSING
            //Adapted from MonoXNA
            //Originally adaped from Luigi Auriemma's unxwb

            WaveBankHeader wavebankheader;
            WaveBankData wavebankdata;

            wavebankdata.EntryNameElementSize = 0;
            wavebankdata.CompactFormat = 0;
            wavebankdata.Alignment = 0;
            wavebankdata.BuildTime = 0;

            int wavebank_offset = 0;

            _waveBankFileName = waveBankFilename;

            BinaryReader reader = new BinaryReader(AudioEngine.OpenStream(waveBankFilename));

            reader.ReadBytes(4);

            _version = wavebankheader.Version = reader.ReadInt32();

            int last_segment = 4;
            //if (wavebankheader.Version == 1) goto WAVEBANKDATA;
            if (wavebankheader.Version <= 3) last_segment = 3;
            if (wavebankheader.Version >= 42) reader.ReadInt32();    // skip HeaderVersion

            wavebankheader.Segments = new Segment[5];

            for (int i = 0; i <= last_segment; i++)
            {
                wavebankheader.Segments[i].Offset = reader.ReadInt32();
                wavebankheader.Segments[i].Length = reader.ReadInt32();
            }

            reader.BaseStream.Seek(wavebankheader.Segments[0].Offset, SeekOrigin.Begin);

            //WAVEBANKDATA:

            wavebankdata.Flags = reader.ReadInt32();
            wavebankdata.EntryCount = reader.ReadInt32();

            if ((wavebankheader.Version == 2) || (wavebankheader.Version == 3))
            {
                wavebankdata.BankName = System.Text.Encoding.UTF8.GetString(
                    reader.ReadBytes(16), 0, 16).Replace("\0", "");
            }
            else
            {
                wavebankdata.BankName = System.Text.Encoding.UTF8.GetString(
                    reader.ReadBytes(64), 0, 64).Replace("\0", "");
            }

            _bankName = wavebankdata.BankName;

            if (wavebankheader.Version == 1)
            {
                //wavebank_offset = (int)ftell(fd) - file_offset;
                wavebankdata.EntryMetaDataElementSize = 20;
            }
            else
            {
                wavebankdata.EntryMetaDataElementSize = reader.ReadInt32();
                wavebankdata.EntryNameElementSize = reader.ReadInt32();
                wavebankdata.Alignment = reader.ReadInt32();
                wavebank_offset = wavebankheader.Segments[1].Offset; //METADATASEGMENT
            }

            if ((wavebankdata.Flags & Flag_Compact) != 0)
            {
                reader.ReadInt32(); // compact_format
            }

            _playRegionOffset = wavebankheader.Segments[last_segment].Offset;
            if (_playRegionOffset == 0)
            {
                _playRegionOffset =
                    wavebank_offset +
                    (wavebankdata.EntryCount * wavebankdata.EntryMetaDataElementSize);
            }

            int segidx_entry_name = 2;
            if (wavebankheader.Version >= 42) segidx_entry_name = 3;

            if ((wavebankheader.Segments[segidx_entry_name].Offset != 0) &&
                (wavebankheader.Segments[segidx_entry_name].Length != 0))
            {
                if (wavebankdata.EntryNameElementSize == -1) wavebankdata.EntryNameElementSize = 0;
                byte[] entry_name = new byte[wavebankdata.EntryNameElementSize + 1];
                entry_name[wavebankdata.EntryNameElementSize] = 0;
            }

            _sounds = new SoundEffect[wavebankdata.EntryCount];
            _streams = new StreamInfo[wavebankdata.EntryCount];

            reader.BaseStream.Seek(wavebank_offset, SeekOrigin.Begin);

            // The compact format requires us to load stuff differently.
            var isCompactFormat = (wavebankdata.Flags & Flag_Compact) != 0;
            if (isCompactFormat)
            {
                // Load the sound data offset table from disk.
                for (var i = 0; i < wavebankdata.EntryCount; i++)
                {
                    var len = reader.ReadInt32();
                    _streams[i].Format = wavebankdata.CompactFormat;
                    _streams[i].FileOffset = (len & ((1 << 21) - 1)) * wavebankdata.Alignment;
                }

                // Now figure out the sound data lengths.
                for (var i = 0; i < wavebankdata.EntryCount; i++)
                {
                    int nextOffset;
                    if (i == (wavebankdata.EntryCount - 1))
                        nextOffset = wavebankheader.Segments[last_segment].Length;
                    else
                        nextOffset = _streams[i + 1].FileOffset;

                    // The next and current offsets used to calculate the length.
                    _streams[i].FileLength = nextOffset - _streams[i].FileOffset;
                }
            }
            else
            {
                for (var i = 0; i < wavebankdata.EntryCount; i++)
                {
                    var info = new StreamInfo();
                    if (wavebankheader.Version == 1)
                    {
                        info.Format = reader.ReadInt32();
                        info.FileOffset = reader.ReadInt32();
                        info.FileLength = reader.ReadInt32();
                        info.LoopStart = reader.ReadInt32();
                        info.LoopLength = reader.ReadInt32();
                    }
                    else
                    {
                        var flagsAndDuration = reader.ReadInt32(); // Unused

                        if (wavebankdata.EntryMetaDataElementSize >= 8)
                            info.Format = reader.ReadInt32();
                        if (wavebankdata.EntryMetaDataElementSize >= 12)
                            info.FileOffset = reader.ReadInt32();
                        if (wavebankdata.EntryMetaDataElementSize >= 16)
                            info.FileLength = reader.ReadInt32();
                        if (wavebankdata.EntryMetaDataElementSize >= 20)
                            info.LoopStart = reader.ReadInt32();
                        if (wavebankdata.EntryMetaDataElementSize >= 24)
                            info.LoopLength = reader.ReadInt32();
                    }

                    // TODO: What is this doing?
                    if (wavebankdata.EntryMetaDataElementSize < 24)
                    {
                        if (info.FileLength != 0)
                            info.FileLength = wavebankheader.Segments[last_segment].Length;
                    }

                    _streams[i] = info;
                }
            }

            // If this isn't a streaming wavebank then load all the sounds now.
            if (!_streaming)
            {
                for (var i = 0; i < _streams.Length; i++)
                {
                    var info = _streams[i];

                    // Read the data.
                    reader.BaseStream.Seek(info.FileOffset + _playRegionOffset, SeekOrigin.Begin);
                    var audiodata = reader.ReadBytes(info.FileLength);

                    // Decode the format information.
                    MiniFormatTag codec;
                    int channels, rate, alignment;
                    DecodeFormat(info.Format, out codec, out channels, out rate, out alignment);

                    // Call the special constructor on SoundEffect to sort it out.
                    _sounds[i] = new SoundEffect(
                        codec, audiodata, channels, rate, alignment, 
                        info.LoopStart, info.LoopLength);
                }

                _streams = null;
            }

            audioEngine.Wavebanks[_bankName] = this;

            IsPrepared = true;


            /*
                      //XWB PARSING
                      //Adapted from MonoXNA
                      //Originally adaped from Luigi Auriemma's unxwb

                      WaveBankHeader wavebankheader;
                      WaveBankData wavebankdata;
                      WaveBankEntry wavebankentry;

                      wavebankdata.EntryNameElementSize = 0;
                      wavebankdata.CompactFormat = 0;
                      wavebankdata.Alignment = 0;
                      wavebankdata.BuildTime = 0;

                      wavebankentry.Format = 0;
                      wavebankentry.PlayRegion.Length = 0;
                      wavebankentry.PlayRegion.Offset = 0;

                      int wavebank_offset = 0;


                      nonStreamingWaveBankFilename = FileHelpers.NormalizeFilePathSeparators(nonStreamingWaveBankFilename);

          #if !ANDROID
                      BinaryReader reader = new BinaryReader(TitleContainer.OpenStream(nonStreamingWaveBankFilename));
          #else 
                      Stream stream = Game.Activity.Assets.Open(nonStreamingWaveBankFilename);
                      MemoryStream ms = new MemoryStream();
                      stream.CopyTo( ms );
                      stream.Close();
                      ms.Position = 0;
                      BinaryReader reader = new BinaryReader(ms);
          #endif
                      reader.ReadBytes(4);

                      wavebankheader.Version = reader.ReadInt32();

                      int last_segment = 4;
                      //if (wavebankheader.Version == 1) goto WAVEBANKDATA;
                      if (wavebankheader.Version <= 3) last_segment = 3;
                      if (wavebankheader.Version >= 42) reader.ReadInt32();    // skip HeaderVersion

                      wavebankheader.Segments = new Segment[5];

                      for (int i = 0; i <= last_segment; i++)
                      {
                          wavebankheader.Segments[i].Offset = reader.ReadInt32();
                          wavebankheader.Segments[i].Length = reader.ReadInt32();
                      }

                      reader.BaseStream.Seek(wavebankheader.Segments[0].Offset, SeekOrigin.Begin);

                      //WAVEBANKDATA:

                      wavebankdata.Flags = reader.ReadInt32();
                      wavebankdata.EntryCount = reader.ReadInt32();

                      if ((wavebankheader.Version == 2) || (wavebankheader.Version == 3))
                      {
                          wavebankdata.BankName = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(16),0,16).Replace("\0", "");
                      }
                      else
                      {
                          wavebankdata.BankName = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(64),0,64).Replace("\0", "");
                      }

                      _bankName = wavebankdata.BankName;

                      if (wavebankheader.Version == 1)
                      {
                          //wavebank_offset = (int)ftell(fd) - file_offset;
                          wavebankdata.EntryMetaDataElementSize = 20;
                      }
                      else
                      {
                          wavebankdata.EntryMetaDataElementSize = reader.ReadInt32();
                          wavebankdata.EntryNameElementSize = reader.ReadInt32();
                          wavebankdata.Alignment = reader.ReadInt32();
                          wavebank_offset = wavebankheader.Segments[1].Offset; //METADATASEGMENT
                      }

                      if ((wavebankdata.Flags & Flag_Compact) != 0)
                      {
                          reader.ReadInt32(); // compact_format
                      }

                      int playregion_offset = wavebankheader.Segments[last_segment].Offset;
                      if (playregion_offset == 0)
                      {
                          playregion_offset =
                              wavebank_offset +
                              (wavebankdata.EntryCount * wavebankdata.EntryMetaDataElementSize);
                      }

                      int segidx_entry_name = 2;
                      if (wavebankheader.Version >= 42) segidx_entry_name = 3;

                      if ((wavebankheader.Segments[segidx_entry_name].Offset != 0) &&
                          (wavebankheader.Segments[segidx_entry_name].Length != 0))
                      {
                          if (wavebankdata.EntryNameElementSize == -1) wavebankdata.EntryNameElementSize = 0;
                          byte[] entry_name = new byte[wavebankdata.EntryNameElementSize + 1];
                          entry_name[wavebankdata.EntryNameElementSize] = 0;
                      }

                      _sounds = new SoundEffect[wavebankdata.EntryCount];

                      for (int current_entry = 0; current_entry < wavebankdata.EntryCount; current_entry++)
                      {
                          reader.BaseStream.Seek(wavebank_offset, SeekOrigin.Begin);
                          //SHOWFILEOFF;

                          //memset(&wavebankentry, 0, sizeof(wavebankentry));
                          wavebankentry.LoopRegion.Length = 0;
                          wavebankentry.LoopRegion.Offset = 0;

                          if ((wavebankdata.Flags & Flag_Compact) != 0)
                          {
                              int len = reader.ReadInt32();
                              wavebankentry.Format = wavebankdata.CompactFormat;
                              wavebankentry.PlayRegion.Offset = (len & ((1 << 21) - 1)) * wavebankdata.Alignment;
                              wavebankentry.PlayRegion.Length = (len >> 21) & ((1 << 11) - 1);

                              // workaround because I don't know how to handke the deviation length
                              reader.BaseStream.Seek(wavebank_offset + wavebankdata.EntryMetaDataElementSize, SeekOrigin.Begin);

                              //MYFSEEK(wavebank_offset + wavebankdata.dwEntryMetaDataElementSize); // seek to the next
                              if (current_entry == (wavebankdata.EntryCount - 1))
                              {   // the last track
                                  len = wavebankheader.Segments[last_segment].Length;
                              }
                              else
                              {
                                  len = ((reader.ReadInt32() & ((1 << 21) - 1)) * wavebankdata.Alignment);
                              }
                              wavebankentry.PlayRegion.Length =
                                  len -                               // next offset
                                  wavebankentry.PlayRegion.Offset;  // current offset
                              goto wavebank_handle;
                          }

                          if (wavebankheader.Version == 1)
                          {
                              wavebankentry.Format = reader.ReadInt32();
                              wavebankentry.PlayRegion.Offset = reader.ReadInt32();
                              wavebankentry.PlayRegion.Length = reader.ReadInt32();
                              wavebankentry.LoopRegion.Offset = reader.ReadInt32();
                              wavebankentry.LoopRegion.Length = reader.ReadInt32();
                          }
                          else
                          {
                              if (wavebankdata.EntryMetaDataElementSize >= 4) wavebankentry.FlagsAndDuration = reader.ReadInt32();
                              if (wavebankdata.EntryMetaDataElementSize >= 8) wavebankentry.Format = reader.ReadInt32();
                              if (wavebankdata.EntryMetaDataElementSize >= 12) wavebankentry.PlayRegion.Offset = reader.ReadInt32();
                              if (wavebankdata.EntryMetaDataElementSize >= 16) wavebankentry.PlayRegion.Length = reader.ReadInt32();
                              if (wavebankdata.EntryMetaDataElementSize >= 20) wavebankentry.LoopRegion.Offset = reader.ReadInt32();
                              if (wavebankdata.EntryMetaDataElementSize >= 24) wavebankentry.LoopRegion.Length = reader.ReadInt32();
                          }

                          if (wavebankdata.EntryMetaDataElementSize < 24)
                          {   // work-around
                              if (wavebankentry.PlayRegion.Length != 0)
                              {
                                  wavebankentry.PlayRegion.Length = wavebankheader.Segments[last_segment].Length;
                              }

                          }
                          else //else if(wavebankdata.EntryMetaDataElementSize > sizeof(WaveBankEntry)) 
                          {
                              //RnD : TEMP. ON
                              // skip unused fields
                              //MYFSEEK(wavebank_offset + wavebankdata.EntryMetaDataElementSize);
                              Debug.WriteLine("WaveBank bug: " 
                                  + "wavebankdata.Flags=" + wavebankdata.Flags.ToString() + "; "
                                  + "wavebankdata.EntryMetaDataElementSize=" +
                                  wavebankdata.EntryMetaDataElementSize.ToString());
                          }

                          wavebank_handle:
                          wavebank_offset += wavebankdata.EntryMetaDataElementSize;
                          wavebankentry.PlayRegion.Offset += playregion_offset;

                          // Parse WAVEBANKMINIWAVEFORMAT

                          int codec;
                          int chans;
                          int rate;
                          int align;
                          //int bits;

                          if (wavebankheader.Version == 1)
                          {         // I'm not 100% sure if the following is correct
                              // version 1:
                              // 1 00000000 000101011000100010 0 001 0
                              // | |         |                 | |   |
                              // | |         |                 | |   wFormatTag
                              // | |         |                 | nChannels
                              // | |         |                 ???
                              // | |         nSamplesPerSec
                              // | wBlockAlign
                              // wBitsPerSample

                              codec = (wavebankentry.Format) & ((1 << 1) - 1);
                              chans = (wavebankentry.Format >> (1)) & ((1 << 3) - 1);
                              rate = (wavebankentry.Format >> (1 + 3 + 1)) & ((1 << 18) - 1);
                              align = (wavebankentry.Format >> (1 + 3 + 1 + 18)) & ((1 << 8) - 1);
                              //bits = (wavebankentry.Format >> (1 + 3 + 1 + 18 + 8)) & ((1 << 1) - 1);

                              / * } else if(wavebankheader.dwVersion == 23) { // I'm not 100% sure if the following is correct
                                  // version 23:
                                  // 1000000000 001011101110000000 001 1
                                  // | |        |                  |   |
                                  // | |        |                  |   ???
                                  // | |        |                  nChannels?
                                  // | |        nSamplesPerSec
                                  // | ???
                                  // !!!UNKNOWN FORMAT!!!

                                  //codec = -1;
                                  //chans = (wavebankentry.Format >>  1) & ((1 <<  3) - 1);
                                  //rate  = (wavebankentry.Format >>  4) & ((1 << 18) - 1);
                                  //bits  = (wavebankentry.Format >> 31) & ((1 <<  1) - 1);
                                  codec = (wavebankentry.Format                    ) & ((1 <<  1) - 1);
                                  chans = (wavebankentry.Format >> (1)             ) & ((1 <<  3) - 1);
                                  rate  = (wavebankentry.Format >> (1 + 3)         ) & ((1 << 18) - 1);
                                  align = (wavebankentry.Format >> (1 + 3 + 18)    ) & ((1 <<  9) - 1);
                                  bits  = (wavebankentry.Format >> (1 + 3 + 18 + 9)) & ((1 <<  1) - 1); * /

        }
                else
                {
                    // 0 00000000 000111110100000000 010 01
                    // | |        |                  |   |
                    // | |        |                  |   wFormatTag
                    // | |        |                  nChannels
                    // | |        nSamplesPerSec
                    // | wBlockAlign
                    // wBitsPerSample

                    codec = (wavebankentry.Format) & ((1 << 2) - 1);
                    chans = (wavebankentry.Format >> (2)) & ((1 << 3) - 1);
                    rate = (wavebankentry.Format >> (2 + 3)) & ((1 << 18) - 1);
                    align = (wavebankentry.Format >> (2 + 3 + 18)) & ((1 << 8) - 1);
                    //bits = (wavebankentry.Format >> (2 + 3 + 18 + 8)) & ((1 << 1) - 1);
                }
                
                reader.BaseStream.Seek(wavebankentry.PlayRegion.Offset, SeekOrigin.Begin);
                byte[] audiodata = reader.ReadBytes(wavebankentry.PlayRegion.Length);
                
                
                if (codec == MiniFormatTag_PCM) // PCM
                {
                    
                    //write PCM data into a wav
#if DIRECTX
                    
                    // TODO: Wouldn't storing a SoundEffectInstance like this
                    // result in the "parent" SoundEffect being garbage collected?
                    
					SharpDX.Multimedia.WaveFormat waveFormat = new SharpDX.Multimedia.WaveFormat(rate, chans);
                    var sfx = new SoundEffect(audiodata, 0, audiodata.Length, rate, (AudioChannels)chans, wavebankentry.LoopRegion.Offset, wavebankentry.LoopRegion.Length)
                        {
                            _format = waveFormat
                        };

					_sounds[current_entry] = sfx;
#else
                    _sounds[current_entry] = new SoundEffect(audiodata, rate, (AudioChannels)chans);
#endif
                } 
                else if (codec == MiniForamtTag_WMA) //"WMA"
                {                   

                    //WMA or xWMA (or XMA2)
                    byte[] wmaSig = {0x30, 0x26, 0xb2, 0x75, 0x8e, 0x66, 0xcf, 0x11, 0xa6, 0xd9, 0x0, 0xaa, 0x0, 0x62, 0xce, 0x6c};
                    
                    bool isWma = true;
                    for (int i=0; i<wmaSig.Length; i++) 
                    {
                        if (wmaSig[i] != audiodata[i]) 
                        {
                            isWma = false;
                            break;
                        }
                    }
                    
                    //Let's support m4a data as well for convenience
                    byte[][] m4aSigs = new byte[][] 
                    {
                        new byte[] {0x00, 0x00, 0x00, 0x18, 0x66, 0x74, 0x79, 0x70, 0x4D, 0x34, 0x41, 0x20, 0x00, 0x00, 0x02, 0x00},
                        new byte[] {0x00, 0x00, 0x00, 0x20, 0x66, 0x74, 0x79, 0x70, 0x4D, 0x34, 0x41, 0x20, 0x00, 0x00, 0x00, 0x00}
                    };
                    
                    bool isM4a = false;
                    for (int i=0; i<m4aSigs.Length; i++) 
                    {    
                        byte[] sig = m4aSigs[i];
                        bool matches = true;
                        for (int j=0; j<sig.Length; j++) 
                        {
                            if (sig[j] != audiodata[j]) 
                            {
                                matches = false;
                                break;
                            }
                        }
                        if (matches) 
                        {
                            isM4a = true;
                            break;
                        }
                    }

                    //RnD
                    //Dirty hack : trying to set WMA for unknow item... ^ ^
                    //if (!isWma && !isM4a)
                    //    isWma = true;

                        if (isWma || isM4a) 
                    {
                        //WMA data can sometimes be played directly
#if DIRECTX
                        Debug.WriteLine("WaveBank bug: WMA or M4A not implemented yet!");
                        //throw new NotImplementedException();
                        //***************************************************************
                        //RnD
                        //hack - NSSound can't play non-wav from data, we have to give a filename
                        string filename = Path.GetTempFileName();
                        if (isWma)
                        {
                            filename = filename.Replace(".tmp", ".wma");
                        }
                        else if (isM4a)
                        {
                            filename = filename.Replace(".tmp", ".m4a");
                        }
                        else
                        {
                            Debug.WriteLine(
                                "WaveBank warning: Its not Wma or M4A. Sadly, not implemented!");
                        }

                        using (var audioFile = File.Create(filename))
                        {
                            audioFile.Write(audiodata, 0, audiodata.Length);
                            audioFile.Seek(0, SeekOrigin.Begin);

                            _sounds[current_entry] = SoundEffect.FromStream(audioFile);
                        }
                        // **************************************************************
#elif !WINRT
                        //hack - NSSound can't play non-wav from data, we have to give a filename
                        string filename = Path.GetTempFileName();
                        if (isWma) {
                            filename = filename.Replace(".tmp", ".wma");
                        } else if (isM4a) {
                            filename = filename.Replace(".tmp", ".m4a");
                        }
                        using (var audioFile = File.Create(filename))
                        {
                            audioFile.Write(audiodata, 0, audiodata.Length);
                            audioFile.Seek(0, SeekOrigin.Begin);
       
                            _sounds[current_entry] = SoundEffect.FromStream(audioFile);
                        }
#else
						{
                           Debug.WriteLine("WaveBank bug: not implemented");
                          //throw new NotImplementedException();
                        }
#endif
                    } 
                    else 
                    {
                        //An xWMA or XMA2 file. Can't be played atm :(
                        //throw new NotImplementedException();
                        Debug.WriteLine("WaveBank bug: not implemented. " +
                            "Problem: An xWMA or XMA2 file. Can't be played atm :(");
                    }
                } 
                else if (codec == MiniFormatTag_ADPCM) 
                {
#if DIRECTX
                    _sounds[current_entry] = new SoundEffect(audiodata, rate, (AudioChannels)chans)
                    {
                        _format = new SharpDX.Multimedia.WaveFormatAdpcm(rate, chans, align)
                    };
#else
                    using (var dataStream = new MemoryStream(audiodata)) {
                        using (var source = new BinaryReader(dataStream)) {
                            _sounds[current_entry] = new SoundEffect(
                                MSADPCMToPCM.MSADPCM_TO_PCM(source, (short) chans, (short) align),
                                rate,
                                (AudioChannels)chans
                            );
                        }
                    }
#endif
                } 
                else 
                {
                    //throw new NotImplementedException();
                    Debug.WriteLine("WaveBank bug: not implemented");
                }
                
            }
			
			audioEngine.Wavebanks[_bankName] = this;
*/


        }//WaveBank



        private void DecodeFormat(int format, out MiniFormatTag codec, out int channels, out int rate, out int alignment)
        {
            if (_version == 1)
            {
                // I'm not 100% sure if the following is correct
                // version 1:
                // 1 00000000 000101011000100010 0 001 0
                // | |         |                 | |   |
                // | |         |                 | |   wFormatTag
                // | |         |                 | nChannels
                // | |         |                 ???
                // | |         nSamplesPerSec
                // | wBlockAlign
                // wBitsPerSample

                codec = (MiniFormatTag)((format) & ((1 << 1) - 1));
                channels = (format >> (1)) & ((1 << 3) - 1);
                rate = (format >> (1 + 3 + 1)) & ((1 << 18) - 1);
                alignment = (format >> (1 + 3 + 1 + 18)) & ((1 << 8) - 1);
                //bits = (format >> (1 + 3 + 1 + 18 + 8)) & ((1 << 1) - 1);

                /*} else if(wavebankheader.dwVersion == 23) { // I'm not 100% sure if the following is correct
                    // version 23:
                    // 1000000000 001011101110000000 001 1
                    // | |        |                  |   |
                    // | |        |                  |   ???
                    // | |        |                  nChannels?
                    // | |        nSamplesPerSec
                    // | ???
                    // !!!UNKNOWN FORMAT!!!

                    //codec = -1;
                    //chans = (wavebankentry.Format >>  1) & ((1 <<  3) - 1);
                    //rate  = (wavebankentry.Format >>  4) & ((1 << 18) - 1);
                    //bits  = (wavebankentry.Format >> 31) & ((1 <<  1) - 1);
                    codec = (wavebankentry.Format                    ) & ((1 <<  1) - 1);
                    chans = (wavebankentry.Format >> (1)             ) & ((1 <<  3) - 1);
                    rate  = (wavebankentry.Format >> (1 + 3)         ) & ((1 << 18) - 1);
                    align = (wavebankentry.Format >> (1 + 3 + 18)    ) & ((1 <<  9) - 1);
                    bits  = (wavebankentry.Format >> (1 + 3 + 18 + 9)) & ((1 <<  1) - 1); */

            }
            else
            {
                // 0 00000000 000111110100000000 010 01
                // | |        |                  |   |
                // | |        |                  |   wFormatTag
                // | |        |                  nChannels
                // | |        nSamplesPerSec
                // | wBlockAlign
                // wBitsPerSample

                codec = (MiniFormatTag)((format) & ((1 << 2) - 1));
                channels = (format >> (2)) & ((1 << 3) - 1);
                rate = (format >> (2 + 3)) & ((1 << 18) - 1);
                alignment = (format >> (2 + 3 + 18)) & ((1 << 8) - 1);
                //bits = (info.Format >> (2 + 3 + 18 + 8)) & ((1 << 1) - 1);
            }
        }


        /// <param name="audioEngine">Instance of the AudioEngine to associate this wave bank with.</param>
        /// <param name="streamingWaveBankFilename">Path to the .xwb to stream from.</param>
        /// <param name="offset">DVD sector-aligned offset within the wave bank data file.</param>
        /// <param name="packetsize">Stream packet size, in sectors, to use for each stream. The minimum value is 2.</param>
        /// <remarks>
        /// <para>This constructor streams wave data as needed.</para>
        /// <para>Note that packetsize is in sectors, which is 2048 bytes.</para>
        /// <para>AudioEngine.Update() must be called at least once before using data from a streaming wave bank.</para>
        /// </remarks>
        public WaveBank(AudioEngine audioEngine, string streamingWaveBankFilename, 
            int offset, short packetsize)
			: this(audioEngine, streamingWaveBankFilename)
		{

            if (audioEngine == null)
                throw new ArgumentNullException("audioEngine");
                       
            if (offset != 0) 
            {
                //throw new NotImplementedException();
                Debug.WriteLine("Wavebank bug: not implemented. Offset =" + offset.ToString());
            }
		}

        internal SoundEffect GetSoundEffect(int trackIndex)
        {
            return _sounds[trackIndex];
        }

		#region IDisposable implementation
		public void Dispose ()
		{
            if (IsDisposed)
                return;

            foreach (var s in _sounds)
                s.Dispose();

            IsDisposed = true;
        }
		#endregion
    }
}

