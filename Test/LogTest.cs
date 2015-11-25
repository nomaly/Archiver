using System;
using System.Diagnostics;
using System.IO;
using Archiver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class LogTest
    {
        private static TextWriter m_writer;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            CLog.Init(Console.Out);
        }

        [ClassCleanup]
        public static void Close()
        {
            m_writer.Dispose();
        }

        [TestMethod]
        public void TestMessages()
        {
            TestInfo();
            TestWarning();
            TestError();
            TestExceptioin();           
        }
        
        public void TestInfo()
        {
            CLog.Message("Test info message");
        }

        public void TestWarning()
        {
            CLog.Warning("Test warning message");
        }
        
        public void TestError()
        {
            CLog.Error("Test error message");
        }
        
        public void TestExceptioin()
        {
            try
            {
                ExceptionMethod1();
            }
            catch (Exception e)
            {
                CLog.Exception(e, "Test exception message");
            }
        }

        public void ExceptionMethod1() { ExceptionMethod2(); }
        public void ExceptionMethod2() { throw new Exception("Exception from ExceptionMethod2");}
    }
}
