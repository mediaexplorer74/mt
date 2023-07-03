﻿// CachedBuffer

using System.IO;

namespace GameManager.DataStructures
{
    public class CachedBuffer
    {
        private bool _isActive = true;
        public readonly byte[] Data;
        public readonly BinaryWriter Writer;
        public readonly BinaryReader Reader;
        private readonly MemoryStream _memoryStream;

        public int Length
        {
            get { return Data.Length; }
        }

        public bool IsActive
        {
            get { return _isActive; }
        }

        public CachedBuffer(byte[] data)
        {
            Data = data;
            _memoryStream = new MemoryStream(data);
            Writer = new BinaryWriter(_memoryStream);
            Reader = new BinaryReader(_memoryStream);
        }

        internal CachedBuffer Activate()
        {
            _isActive = true;
            _memoryStream.Position = 0L;
            return this;
        }

        public void Recycle()
        {
            if (_isActive)
            {
                _isActive = false;
                BufferPool.Recycle(this);
            }
        }
    }
}
