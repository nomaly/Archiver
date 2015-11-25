using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Archiver
{
    class CFileCompressionPipe
    {
        private object m_lock;
        private ICompressionPipe m_compressionPipe;
        private string m_input;
        private string m_output;
        volatile private bool m_pipeStarted;

        public CFileCompressionPipe(ICompressionPipe pipe)
        {
            CArguments.ThrowIfArgumentNull(pipe, "pipe");

            m_compressionPipe = pipe;
            m_pipeStarted = false;
        }

        public void SetInput(String inputPath)
        {
            CArguments.ThrowIfNot(String.IsNullOrEmpty(inputPath), "inputPath");

            bool isStarted;
            lock (m_lock)
                isStarted = m_pipeStarted;

            if(isStarted)
                throw new InvalidOperationException("Cannot set input file path while pipe is started");

            m_input = inputPath;
        }

        public void SetOutput(String outputFile)
        {
            CArguments.ThrowIfNot(String.IsNullOrEmpty(outputFile), "outputFile");

            bool isStarted;
            lock (m_lock)
                isStarted = m_pipeStarted;

            if (isStarted)
                throw new InvalidOperationException("Cannot set output file path while pipe is started");

            m_output = outputFile;
        }

        public async void StartAsync()
        {
            bool isStarted;
            lock (m_lock)
                isStarted = m_pipeStarted;

            if (isStarted)
                throw new InvalidOperationException("Pipe is already started");

            lock (m_lock)
                m_pipeStarted = true;


        }

        public void Start()
        {
            using (CPipedStreamReader reader = new CPipedStreamReader(m_input))
            {
                var sender = m_compressionPipe.GetSender();

            }
        }

        public void WaitForEnd()
        {
            
        }
    }
}
