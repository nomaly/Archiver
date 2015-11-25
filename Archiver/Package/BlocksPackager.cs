using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Archiver
{
    public class CBlocksPackager : IDisposable
    {
        private CDisposingList m_disposingList = new CDisposingList();
        private CCompressedBlocksPackagerOptions m_options;
        private Stream m_writer;
        private CPackage m_sealedPackage;
        private bool m_sealed = false;

        private Queue<CBlockInPackageInfo> m_writtenBlocks;
        public List<CBlockInPackageInfo> BlockInPackageInfos => m_writtenBlocks.ToList();

        public CBlocksPackager(Stream outputStream, CCompressedBlocksPackagerOptions options)
        {
            CArguments.ThrowIfArgumentNull(outputStream, "outputStream");
            CArguments.ThrowIfArgumentNull(options, "options");
            CArguments.ThrowIfNot(outputStream.CanWrite && outputStream.CanSeek, "outputStream cannot write or seek");

            outputStream.Position = 0;

            m_writtenBlocks = new Queue<CBlockInPackageInfo>();
            m_writer = m_disposingList.Append(outputStream);
            m_options = options;

            ReservePlaceForHeader();
        }

        private void ReservePlaceForHeader()
        {
            byte[] nullBytes = new byte[CApplicationUtils.GetSizeOfPackageHeader()];
            m_writer.Write(nullBytes, 0, nullBytes.Length);
        }

        public void WriteBlock(CProcessingBlock processedBlock)
        {
            if(m_sealed)
                throw new InvalidOperationException("Package has been sealed");

            byte[] rawBytes = processedBlock.BlockInfo.RawBytes;

            Int64 currentPosition = m_writer.Position;
            m_writer.Write(rawBytes, 0, rawBytes.Length);

            CBlockInPackageInfo writtenBlockInfo = new CBlockInPackageInfo(
                    currentPosition,
                    rawBytes.LongLength,
                    processedBlock.BlockNum
                );

            m_writtenBlocks.Enqueue(writtenBlockInfo);
        }

        public CPackage SealPackage()
        {
            if (m_sealedPackage != null)
                return m_sealedPackage;

            CPackageSummaryInfo info = CreatePackageSummary();
            Int64 writtenSummaryBytes = WritePackageSummary(info);

            CPackageHeaderInfo headerInfo = CreateHeader(writtenSummaryBytes);
            WriteHeader(headerInfo);

            m_sealed = true;
            m_sealedPackage = new CPackage(info);
            return new CPackage(info);
        }

        private CPackageHeaderInfo CreateHeader(Int64 summaryLength)
        {
            return new CPackageHeaderInfo()
            {
                SummarySectionSize = summaryLength,
                Version = 1
            };
        }

        private void WriteHeader(CPackageHeaderInfo headerInfo)
        {
            byte[] headerBytes = CApplicationUtils.SerializeToBytes(headerInfo);
            m_writer.Position = 0;

            m_writer.Write(headerBytes, 0, headerBytes.Length);
        }

        private CPackageSummaryInfo CreatePackageSummary()
        {
            return CPackageSummaryInfo.CreateSummary(m_options.CompressionAlgorithm,
                m_writtenBlocks.ToList());
        }

        private Int64 WritePackageSummary(CPackageSummaryInfo summary)
        {
            byte[] summaryBytes = ConvertSummaryToBytes(summary);
            m_writer.Write(summaryBytes, 0, summaryBytes.Length);

            return summaryBytes.LongLength;
        }

        private byte[] ConvertSummaryToBytes(CPackageSummaryInfo summary)
        {
            CDomContainer container = CPackageSummaryInfo.Serialize(summary);
            String serializedContainer = container.SerializeToString();
            return CApplicationUtils.ConvertStringToBytes(serializedContainer);
        }

        public void Dispose()
        {
            if (!m_sealed)
                SealPackage();

            m_disposingList.Dispose();
        }
    }
}
