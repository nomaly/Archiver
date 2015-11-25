using System;
using System.Collections.Generic;
using System.Linq;

namespace Archiver
{
    public class CPackageSummaryInfo
    {
        private const string NODE_PACKAGESUMMARY = "PackageSummaryInfo";
        private const string NODE_CIPSECTION = "BlocksInPackage";
        private const string NODE_ALGORITHM = "Algorithm";
        private const string NODE_LIST_NUMBER = "Number";
        private const string NODE_LIST_OFFSET = "Offset";
        private const string NODE_LIST_LENGTH = "Length";

        public ECompressionAlgorithms CompressionAlgorithm { get; private set; }
        public List<CBlockInPackageInfo> BlocksInPackage { get; private set; }
        
        public CPackageSummaryInfo(ECompressionAlgorithms compressionAlgorithm, List<CBlockInPackageInfo> cipInfos )
        {
            CArguments.ThrowIfArgumentNull(cipInfos, "cipInfos");

            BlocksInPackage = new List<CBlockInPackageInfo>();

            CompressionAlgorithm = compressionAlgorithm;
            BlocksInPackage = cipInfos;
        }

        public static CPackageSummaryInfo CreateSummary(ECompressionAlgorithms compressionAlgorithm,
            List<CBlockInPackageInfo> packageInfos)
        {
            return new CPackageSummaryInfo(compressionAlgorithm, packageInfos);
        }

        public static CDomContainer Serialize(CPackageSummaryInfo summaryInfo)
        {
            CDomContainer container = new CDomContainer(NODE_PACKAGESUMMARY);
            Serialize(summaryInfo, container);
            return container;
        }

        public static void Serialize(CPackageSummaryInfo summaryInfo, CDomContainer container)
        {
            container.Set(NODE_ALGORITHM, (Int32)summaryInfo.CompressionAlgorithm);
            summaryInfo.SerializeBlockInfos(container);
        }

        private void SerializeBlockInfos(CDomContainer container)
        {
            CDomListSection blocksSection = container.CreateListSection(NODE_CIPSECTION);

            foreach (CBlockInPackageInfo info in BlocksInPackage)
            {
                CDomSection itemSection = blocksSection.CreateSection();

                itemSection.Set(NODE_LIST_NUMBER, info.Number);
                itemSection.Set(NODE_LIST_OFFSET, info.Offset);
                itemSection.Set(NODE_LIST_LENGTH, info.Length);
            }
        }
    }
}