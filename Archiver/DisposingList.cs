using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archiver
{
    class CDisposingList : IDisposable
    {
        private bool m_disposed = false;
        private Stack<IDisposable> m_disposingList;

        public CDisposingList()
        {
            m_disposingList = new Stack<IDisposable>();
        }

        public T Append<T>(T item) where T : IDisposable
        {
            m_disposingList.Push(item);
            return item;
        }

        public void Dispose()
        {
            if (!m_disposed)
            {
                while (m_disposingList.Count > 0)
                {
                    var disposableItem = m_disposingList.Pop();

                    try
                    {
                        disposableItem.Dispose();
                    }
                    catch (Exception e)
                    {
                        CLog.Exception(e, "Failed to dispose item");
                    }
                }
            }
        }
    }
}
