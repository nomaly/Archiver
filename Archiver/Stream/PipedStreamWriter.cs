using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archiver
{
    public class CPipedStreamWriter : IDisposable
    {
        private CDisposingList m_disposingList = new CDisposingList();
        private Stream m_outputStream;

        public Int64 WrittenBlocks { get; private set; } = 0;
        public Int64 Position => m_outputStream.Position;

        public CPipedStreamWriter(Stream outputStream)
        {
            CArguments.ThrowIfArgumentNull(outputStream, "outputStream");
            CArguments.ThrowIfNot(outputStream.CanWrite && outputStream.CanSeek, "outputStream cannot write or seek");

            m_outputStream = m_disposingList.Append(outputStream);

            m_outputStream.Position = 0;
        }

        public Int64 WriteBlock(CBlockInfo blockInfo)
        {
            return WriteBlock(blockInfo.RawBytes);
        }

        public Int64 WriteBlock(byte[] rawBytes)
        {
            m_outputStream.Write(rawBytes, 0, rawBytes.Length);
            WrittenBlocks++;
            return rawBytes.Length;
        }

        public void Dispose()
        {
            m_disposingList.Dispose();
        }

        public static CPipedStreamWriter CreateInMemoryWriter()
        {
            return new CPipedStreamWriter(new MemoryStream());
        }
    }
}
