using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Archiver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class CompressionTest
    {
        public static readonly FileInfo m_fileToCompress = new FileInfo(@"c:\Dropbox\training.xls");

        [TestMethod]
        public void GZipCompressing()
        {
            var md5 = MD5.Create();
            ICompressionAlgorithm compressionAlgorithm = new GZipCompressionAlgorithm();

            var raw = File.ReadAllBytes(m_fileToCompress.FullName);
            Console.WriteLine("Raw bytes: {0}", raw.Length);

            var rawHash = md5.ComputeHash(raw);

            var compressed = compressionAlgorithm.Compress(raw);
            Console.WriteLine("Compressed bytes: {0}", compressed.Length);

            var uncompressed = compressionAlgorithm.Decompress(compressed);
            Console.WriteLine("Uncompressed bytes: {0}", uncompressed.Length);

            var uncompressedHash = md5.ComputeHash(uncompressed);
            
            Assert.IsTrue(rawHash.SequenceEqual(uncompressedHash));
        }
    }
}
