using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archiver
{
    public class CPipedStreamReader : IDisposable
    {
        private Stream m_s;
        private Boolean m_disposed = false;
        private int _mBlockSize;
        private long _mTotalBlocks;

        public int BlockSize
        {
            get { return _mBlockSize; }
        }

        public long BlockPosition
        {
            get { return m_s.Position / _mBlockSize; }
        }

        public long TotalBlocks
        {
            get { return _mTotalBlocks; }
        }

        public long Size
        {
            get { return m_s.Length; }
        }

        public bool CanReadNextBlock
        {
            get { return m_s.Position < m_s.Length; }
        }

        public CPipedStreamReader(String path) :
            this(File.OpenRead(path))
        {
        }

        public CPipedStreamReader(Stream inputStream) : this(inputStream, CDefaultValues.BlockSize)
        {
        }

        public CPipedStreamReader(Stream inputStream, int blockSize)
        {
            CArguments.ThrowIfArgumentNull(inputStream, "inputStream");
            CArguments.ThrowIfNot(inputStream.CanRead && inputStream.CanSeek, "inputStream cannot be read");

            m_s = inputStream;
            m_s.Position = 0;
            _mBlockSize = blockSize;
            _mTotalBlocks = m_s.Length/_mBlockSize;

            if (_mTotalBlocks == 0)
                _mTotalBlocks++;
        }
        
        public byte[] ReadBlock()
        {
            if (m_disposed)
                throw new ObjectDisposedException("CPipedStreamReader");
            if(m_s.Position == m_s.Length)
                throw new InvalidOperationException("Input stream has been exhausted");
            
            long remainedBytesLength = m_s.Length - m_s.Position;
            int bufferSize = remainedBytesLength < _mBlockSize ? checked((int)remainedBytesLength) : _mBlockSize;

            byte[] buffer = new byte[bufferSize];
            m_s.Read(buffer, 0, bufferSize);

            return buffer;
        }

        public void Close()
        {
            m_s.Close();
        }

        public void Dispose()
        {
            try
            {
                if (m_disposed) return;

                m_s.Dispose();
                m_disposed = true;
            }
            catch (Exception e)
            {
                CLog.Exception(e, "Failed to dispose input stream");
            }
        }

        public static CPipedStreamReader Open(String path)
        {
            return Open(path, CDefaultValues.BlockSize);
        }

        public static CPipedStreamReader Open(String path, int blockSize)
        {
            return new CPipedStreamReader(new FileStream(path, FileMode.Open), blockSize);
        }
    }
}
