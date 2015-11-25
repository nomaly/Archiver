using System;
using System.Diagnostics;
using System.IO;
using Archiver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class InputFileTest
    {
        private static FileInfo m_lockedFile;
        private static FileInfo m_file;
        private static int m_blockSize = 10 * 1024;//10 Kb

        [ClassInitialize]
        public static void GetFileInfos(TestContext context)
        {
            m_lockedFile = new FileInfo(@"C:\pagefile.sys");
            m_file = new FileInfo(@"c:\Users\Nom\Documents\android-44-x86-emulator-s003.vmdk");
        }
        
        [TestMethod]
        [ExpectedException(typeof(IOException))]
        public void FailedToOpen()
        {
            new CPipedStreamReader(new FileStream(m_lockedFile.FullName, FileMode.Open)).Close();
        }

        [TestMethod]
        public void Open()
        {
            new CPipedStreamReader(new FileStream(m_file.FullName, FileMode.Open)).Close();
        }

        [TestMethod]
        public void BlockSize()
        {
            using (CPipedStreamReader cs = CPipedStreamReader.Open(m_file.FullName, m_blockSize))
            {
                var block = cs.ReadBlock();
                Assert.IsTrue(block.Length == m_blockSize);
            }
        }

        [TestMethod]
        public void TotalBlocks()
        {
            FileInfo fi = new FileInfo(m_file.FullName);
            long size = fi.Length;
            long expectedTotalBlocks = size/m_blockSize;

            using (CPipedStreamReader cs = CPipedStreamReader.Open(m_file.FullName, m_blockSize))
            {
                Assert.IsTrue(cs.TotalBlocks == expectedTotalBlocks);
            }
        }

        [TestMethod]
        public void Position()
        {
            using (CPipedStreamReader cs = CPipedStreamReader.Open(m_file.FullName, m_blockSize))
            {
                Assert.IsTrue(cs.BlockPosition == 0);

                var buffer = cs.ReadBlock();

                Assert.IsTrue(cs.BlockPosition == 1);
            }
        }

        [TestMethod]
        public void ReadAllBlocks()
        {
            using (CPipedStreamReader cs = CPipedStreamReader.Open(m_file.FullName, m_blockSize))
            {
                long totalSize = 0;
                while (cs.CanReadNextBlock)
                    totalSize += cs.ReadBlock().Length;

                Console.WriteLine("totalSize: {0}", totalSize);
                Console.WriteLine("length:    {0}", cs.Size);

                Assert.IsTrue(totalSize == cs.Size);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ReadExhausted()
        {
            using (CPipedStreamReader cs = CPipedStreamReader.Open(m_file.FullName, m_blockSize))
            {
                while (cs.CanReadNextBlock)
                    cs.ReadBlock();

                cs.ReadBlock();
            }
        }
    }
}
