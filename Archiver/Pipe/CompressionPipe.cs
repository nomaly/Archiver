using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archiver
{
    public interface ICompressionPipe
    {
        ECompressionAlgorithms Algorithm { get; }
        ICompressionBlockSender GetSender();
        ICompressionReceiver GetReceiver();
        void StopCompression();
    }

    /// <summary>
    /// Using for test purposes
    /// </summary>
    public class CNoCompressionPipe : ICompressionPipe, ICompressionBlockSender, ICompressionReceiver
    {
        public ECompressionAlgorithms Algorithm => ECompressionAlgorithms.NoCompression;

        public CNoCompressionPipe()
        {
            
        }

        public ICompressionBlockSender GetSender()
        {
            return this;
        }

        public ICompressionReceiver GetReceiver()
        {
            return this;
        }

        public void StopCompression()
        {
            throw new NotImplementedException();
        }

        void ICompressionBlockSender.Send(CProcessingBlock rawByteProcessingBlock)
        {
            throw new NotImplementedException();
        }

        CProcessingBlock ICompressionReceiver.Receive()
        {
            throw new NotImplementedException();
        }
    }
}
